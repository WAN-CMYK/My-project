using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // 血条相对于玩家的位置偏移，可在Inspector直接调整
    public Vector3 offset = new Vector3(0, 1, 0);

    // 玩家Transform引用
    Transform HeroTran;


    void Start()
    {
        // 通过标签查找玩家物体并获取其Transform
        // HeroTran = GameObject.Find("Hero").transform;
        HeroTran = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        // 每帧更新血条位置：玩家位置 + 偏移量
        transform.position = HeroTran.position + offset;
    }
}