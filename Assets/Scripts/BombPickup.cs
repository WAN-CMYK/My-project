using UnityEngine;
using System.Collections;

public class BombPickup : MonoBehaviour
{
    public AudioClip pickupClip;
    private Animator anim;
    private bool landed = false;
    void Awake()
    {
        anim = transform.root.GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);
            other.GetComponent<LayBombs>().bombCount++; //主角炸弹数量增加1
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
