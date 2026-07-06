using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public float repeatHurtPeriod = 0.5f;
    public float hurtForce = 100f;
    public float upForce = 10f;
    public AudioClip[] ouchClips;
    public float damageAmount = 60f;
    // 建议直接把Hierarchy里的Health物体拖到这里赋值，比运行时Find更稳定
    public SpriteRenderer healthBar;

    private float lastHurtTime = 0;
    private PlayerCtrl playerCtrl;
    private Animator anim;
    private Vector3 healthScale;
    private bool isDead = false; // 死亡标记，避免死亡后继续执行伤害逻辑

    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        anim = GetComponent<Animator>();

        // Inspector未赋值时，再尝试自动查找
        if (healthBar == null)
        {
            GameObject healthObj = GameObject.Find("Health");
            if (healthObj != null)
            {
                healthBar = healthObj.GetComponent<SpriteRenderer>();
            }
            else
            {
                Debug.LogWarning("未找到名为Health的血条对象，请检查Hierarchy中的对象名称");
            }
        }

        if (healthBar != null)
        {
            healthScale = healthBar.transform.localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 死亡后直接拦截所有碰撞伤害
        if (isDead) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Time.time > lastHurtTime + repeatHurtPeriod)
            {
                if (health > 0)
                {
                    TakeDamage(collision.gameObject.transform);
                }
                else
                {
                    HeroDie();
                }
            }
        }
    }

    private void TakeDamage(Transform enemyTran)
    {
        if (isDead) return;

        playerCtrl.bJump = false;
        Vector3 hurtVector = transform.position - enemyTran.position + Vector3.up * upForce;
        GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

        health -= damageAmount;
        lastHurtTime = Time.time; // 更新受伤时间戳

        if (health <= 0)
        {
            health = 0;
            HeroDie();
            return;
        }
        UpdateHealthBar();

        // 音效播放前做空判断
        if (ouchClips != null && ouchClips.Length > 0)
        {
            int i = Random.Range(0, ouchClips.Length);
            AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
        }
    }

    private void HeroDie()
    {
        isDead = true;

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprs)
        {
            s.sortingLayerName = "UI";
        }

        if (playerCtrl != null)
            playerCtrl.enabled = false;

        Gun gun = GetComponentInChildren<Gun>();
        if (gun != null)
            gun.enabled = false;
    }

    public void UpdateHealthBar()
    {
        // 核心修复：访问前先判断血条是否存在、未被销毁
        if (healthBar == null) return;

        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
        healthBar.transform.localScale = new Vector3(health * 0.01f * healthScale.x, 1, 1);
    }
}