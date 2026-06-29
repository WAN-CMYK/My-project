using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 4f;
    public float HP = 2f;
    // 改成 Sprite 类型，直接拖图片资源
    public Sprite damageSprite;
    public Sprite deadSprite;
    public Transform frontCheck;
    public float spinMin = 100f;
    public float spinMax = 300f;

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

        // 血量为1切受伤贴图
        if (HP == 1 && damageSprite != null)
        {
            ren.sprite = damageSprite;
        }

        // 血量归零死亡
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
}