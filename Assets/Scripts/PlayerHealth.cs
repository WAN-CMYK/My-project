using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 100f;
    public float repeatHurtPeriod = 0.5f;
    public float hurtForce = 100f;
    public float upForce = 10f;
    public AudioClip[] ouchClips;
    public float damageAmount = 60f;

    private float lastHurtTime = 0;
    private PlayerCtrl playerCtrl;
    private Animator anim;
    private SpriteRenderer healthBar;
    private Vector3 healthScale;
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("Health").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
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
        playerCtrl.bJump = false;
        Vector3 hurtVector = transform.position
                - enemyTran.position + Vector3.up * upForce;
        GetComponent<Rigidbody2D>().AddForce
                (hurtVector * hurtForce);
        health -= damageAmount;
        if (health < 0)
        {
            HeroDie();
            return;
        }
        UpdateHealthBar();
        int i = Random.Range(0, ouchClips.Length);
        AudioSource.PlayClipAtPoint(ouchClips[i],
                         transform.position);


    }

    private void HeroDie()
    {
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprs)
        {
            // s.sortingLayerID = 4;
            s.sortingLayerName = "UI";
        }
        playerCtrl.enabled = false;
        GetComponentInChildren<Gun>().enabled = false;
        //            
        // anim.SetTrigger("Dead");
    }

    void UpdateHealthBar()
    {
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
        healthBar.transform.localScale = new Vector3(health * 0.01f * healthScale.x, 1, 1);
    }
}
