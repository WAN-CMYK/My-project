using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // 火箭预制体（在Unity中把你的Rocket预制体拖到这里）
    public Rigidbody2D rocket;
    // 火箭发射速度（可在Inspector面板调整）
    public float shootSpeed = 15f;

    // 引用玩家控制器
    private PlayerCtrl playerCtrl;
    // 音频源组件，用于播放开枪音效
    private AudioSource audioSource;

    void Start()
    {
        // 获取玩家根对象上的PlayerCtrl脚本
        playerCtrl = transform.root.GetComponent<PlayerCtrl>();
        // 获取当前Gun物体上挂载的AudioSource组件
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 空引用保护，防止组件缺失时报错崩溃
        if (playerCtrl == null || rocket == null || audioSource == null) return;

        // 检测鼠标左键按下
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 播放发射音效
            audioSource.Play();

            // 玩家朝右时
            if (playerCtrl.bFaceRight)
            {
                Rigidbody2D rocketInstance = Instantiate(rocket, transform.position, Quaternion.identity);
                rocketInstance.velocity = new Vector2(shootSpeed, 0);
            }
            // 玩家朝左时
            else
            {
                Rigidbody2D rocketInstance = Instantiate(rocket, transform.position, Quaternion.Euler(0, 180, 0));
                rocketInstance.velocity = new Vector2(-shootSpeed, 0);
            }
        }
    }
}