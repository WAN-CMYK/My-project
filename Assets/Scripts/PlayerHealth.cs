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
    public SpriteRenderer healthBar;

    private float lastHurtTime = 0;
    private PlayerCtrl playerCtrl;
    private Animator anim;
    private Vector3 healthScale;
    private bool isDead = false;
    // 新增：缓存自身的AudioSource组件
    private AudioSource _audioSource;

    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        anim = GetComponent<Animator>();
        // 获取物体上挂载的AudioSource，直接复用Inspector里配置好的PLAYER声道
        _audioSource = GetComponent<AudioSource>();

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
        lastHurtTime = Time.time;

        if (health <= 0)
        {
            health = 0;
            HeroDie();
            return;
        }
        UpdateHealthBar();

        // 替换原有播放方式：用自身AudioSource播放，复用声道配置
        if (ouchClips != null && ouchClips.Length > 0 && _audioSource != null)
        {
            int i = Random.Range(0, ouchClips.Length);
            _audioSource.PlayOneShot(ouchClips[i]);
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
        if (healthBar == null) return;

        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
        healthBar.transform.localScale = new Vector3(health * 0.01f * healthScale.x, 1, 1);
    }
}
