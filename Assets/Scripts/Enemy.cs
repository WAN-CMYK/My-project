using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 4f;
    public float HP = 2f;
    public Sprite damageSprite;
    public Sprite deadSprite;
    public Transform frontCheck;
    public float spinMin = 100f;
    public float spinMax = 300f;

    [Header("击杀得分")]
    public int scoreValue = 100;

    // ========== 新增部分开始 ==========
    [Header("飘字设置")]
    public GameObject floatingTextPrefab; // 需要挂载飘字预制体
    public Color scoreColor = Color.yellow; // 飘字颜色
    // ========== 新增部分结束 ==========

    SpriteRenderer ren;
    Rigidbody2D enemyBody;
    bool dead = false;
    int obstacleLayer;

    void Start()
    {
        Transform body = transform.Find("Body");
        if (body != null)
        {
            ren = body.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("找不到子物体 'Body'，请检查命名");
        }

        enemyBody = GetComponent<Rigidbody2D>();

        if (frontCheck == null)
        {
            frontCheck = transform.Find("FrontCheck");
        }

        obstacleLayer = 1 << LayerMask.NameToLayer("Obstacle");
    }

    void Update()
    {
        if (dead || enemyBody == null || ren == null) return;

        enemyBody.velocity = new Vector2(speed * transform.localScale.x, enemyBody.velocity.y);

        if (frontCheck != null)
        {
            Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, obstacleLayer);
            foreach (Collider2D hit in frontHits)
            {
                if (hit.CompareTag("Tower"))
                {
                    Flip();
                    break;
                }
            }
        }

        if (HP == 1 && damageSprite != null)
        {
            ren.sprite = damageSprite;
        }

        if (HP <= 0 && !dead)
        {
            if (deadSprite != null)
            {
                ren.sprite = deadSprite;
            }
            Death();
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Hurt()
    {
        if (dead) return;
        HP--;
    }

    void Death()
    {
        dead = true;

        // 1. 增加分数逻辑
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        // ========== 新增部分：生成飘字 ==========
        SpawnFloatingText();
        // =========================================

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        enemyBody.freezeRotation = false;
        enemyBody.AddTorque(Random.Range(spinMin, spinMax));

        SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprs)
        {
            s.sortingLayerName = "UI";
        }

        Destroy(gameObject, 0.5f);
    }

    // ========== 新增部分：生成飘字的具体逻辑 ==========
    void SpawnFloatingText()
    {
        // 如果没有设置预制体或Canvas，则不执行
        if (floatingTextPrefab == null) return;

        // 找到场景中的 Canvas，确保生成的文字属于 UI 层级
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        // 实例化预制体，父物体设为 Canvas
        GameObject textObj = Instantiate(floatingTextPrefab, canvas.transform);

        // 获取我们写的 FloatingText 组件
        FloatingText fText = textObj.GetComponent<FloatingText>();

        if (fText != null)
        {
            // 关键步骤：将敌人的世界坐标转换为屏幕坐标（因为 UI 是基于屏幕的）
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            textObj.transform.position = screenPos;

            // 初始化文字内容（例如 "+100"）和颜色
            fText.Init("+" + scoreValue.ToString(), scoreColor);
        }
    }
    // ================================================
}