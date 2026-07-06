using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 【新增】引入事件系统库，用于检测是否点击了UI
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    public Rigidbody2D rocket;
    public float shootSpeed = 15f;

    private PlayerCtrl playerCtrl;
    private AudioSource audioSource;
    private Animator playerAnim;

    void Start()
    {
        playerCtrl = transform.root.GetComponent<PlayerCtrl>();
        audioSource = GetComponent<AudioSource>();
        playerAnim = transform.root.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerCtrl == null || rocket == null || audioSource == null || playerAnim == null) return;

        // 检测鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            // 【核心修改】检测鼠标当前是否停留在任何 UI 元素上（如按钮、滑块）
            // 如果 EventSystem.current.IsPointerOverGameObject() 返回 true，说明点到了 UI，直接 return 不执行射击
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // --- 下面是原本的射击逻辑 ---

            // 触发射击动画
            playerAnim.SetTrigger("Shoot");

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