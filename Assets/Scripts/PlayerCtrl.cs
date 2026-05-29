using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // 移动相关变量
    public float moveForce = 100f;    // 移动推力
    public float maxSpeed = 5f;       // 最大移动速度

    // 跳跃相关变量
    public float jumpForce = 350f;    // 跳跃力（可在Inspector调整手感）
    public Transform mGroundCheck;    // 地面检测点（需要在Unity中赋值）
    private bool bJump = false;       // 跳跃触发标志

    // 角色朝向变量
    public bool bFaceRight = true;    // 初始朝向（默认向右）

    // 组件引用
    Rigidbody2D playerBody;           // 角色刚体组件

    // Awake：游戏启动前获取组件
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Start：游戏开始时初始化
    void Start()
    {
        // 自动查找子物体中的GroundCheck（如果没手动赋值）
        if (mGroundCheck == null)
        {
            mGroundCheck = transform.Find("GroundCheck");
        }
    }

    // Update：每帧调用，处理输入和非物理逻辑
    void Update()
    {
        // 1. 地面检测：判断角色是否站在地面上
        // 从角色位置向GroundCheck点发射射线，只检测"Ground"层
        bool isGrounded = Physics2D.Linecast(
            transform.position,
            mGroundCheck.position,
            1 << LayerMask.NameToLayer("Ground")
        );

        // 2. 跳跃输入检测（空格键）：只有在地面上才能跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            bJump = true;
        }

        // 3. 角色朝向自动翻转
        float h = Input.GetAxis("Horizontal");
        if (h > 0 && !bFaceRight)
        {
            Flip();
        }
        else if (h < 0 && bFaceRight)
        {
            Flip();
        }
    }

    // FixedUpdate：固定物理帧更新，处理所有物理相关逻辑
    private void FixedUpdate()
    {
        if (playerBody == null) return;

        // 原有水平移动逻辑
        float h = Input.GetAxis("Horizontal");
        if (h * playerBody.velocity.x < maxSpeed)
        {
            playerBody.AddForce(h * Vector2.right * moveForce);
        }

        // 限制水平最大速度
        if (Mathf.Abs(playerBody.velocity.x) > maxSpeed)
        {
            playerBody.velocity = new Vector2(
                Mathf.Sign(playerBody.velocity.x) * maxSpeed,
                playerBody.velocity.y
            );
        }

        // 跳跃物理逻辑
        if (bJump)
        {
            playerBody.AddForce(Vector2.up * jumpForce);
            bJump = false; // 跳跃后立即重置标志，防止空中连跳
        }
    }

    // 角色翻转函数
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        bFaceRight = !bFaceRight; // 关键：更新朝向标志
    }
}