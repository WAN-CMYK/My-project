using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 匹配敌人的大写Tag
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hurt();
            }
        }

        // ========== 新增：命中炸弹箱，触发其自身爆炸 ==========
        if (collision.CompareTag("BombCrate"))
        {
            Bomb crateBomb = collision.GetComponent<Bomb>();
            if (crateBomb != null)
            {
                crateBomb.Explode();
            }
        }
        // ==================================================

        // 生成火箭自身爆炸特效
        if (explosion != null)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
            Instantiate(explosion, transform.position, rotation);
        }

        Destroy(gameObject);
    }
}