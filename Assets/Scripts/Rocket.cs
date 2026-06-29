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

        // 生成爆炸特效
        if (explosion != null)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
            Instantiate(explosion, transform.position, rotation);
        }

        Destroy(gameObject);
    }
}