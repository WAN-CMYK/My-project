using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("爆炸设置")]
    [Tooltip("爆炸特效预制体，在Inspector面板拖进来")]
    public GameObject explosion;

    void Start()
    {
        // （可选）给火箭设置初始速度，如果需要在这里写
    }

    // 碰撞检测（当火箭碰到其他带Collider2D的物体时触发）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果爆炸预制体不为空，就生成爆炸效果
        if (explosion != null)
        {
            // 随机旋转爆炸特效，让效果更自然
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 180)));
            Instantiate(explosion, transform.position, rotation);
        }

        // 销毁自己（火箭碰到东西就消失）
        Destroy(gameObject);
    }

    void Update()
    {
        // 这里可以写火箭的其他逻辑，比如移动、旋转等
    }
}