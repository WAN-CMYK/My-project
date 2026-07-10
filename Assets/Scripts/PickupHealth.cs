using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    public float healthBonus;
    public AudioClip collect;
    private Animator anim;
    private bool landed = false;
    // 缓存自身挂载的AudioSource，复用CREATE声道配置
    private AudioSource _audioSource;

    private void Awake()
    {
        anim = transform.root.GetComponent<Animator>();
        // 获取当前物体上的AudioSource组件
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.health += healthBonus;
            playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);
            playerHealth.UpdateHealthBar();

            // 复用自身AudioSource播放音效，自动走CREATE混音声道
            if (_audioSource != null && collect != null)
            {
                _audioSource.PlayOneShot(collect);
                // 延迟销毁根物体，等音效完整播放完毕再销毁
                Destroy(transform.root.gameObject, collect.length);
            }
            else
            {
                Destroy(transform.root.gameObject);
            }
        }
        else if (other.CompareTag("Ground") && !landed)
        {
            anim.SetTrigger("Land");
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }
    }
}
