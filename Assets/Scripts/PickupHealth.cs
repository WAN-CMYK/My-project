using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    public float healthBonus;
    public AudioClip collect;
    private Animator anim;  //需要在初始化				
    private bool landed = false;

    private void Awake()
    {
        anim = transform.root.GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.health += healthBonus;
            playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);
            playerHealth.UpdateHealthBar();
            AudioSource.PlayClipAtPoint(collect, transform.position);
            Destroy(transform.root.gameObject);
        }
        else if (other.tag == "Ground" && !landed)
        {
            anim.SetTrigger("Land");
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }
    }

}
