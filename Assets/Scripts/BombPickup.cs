using UnityEngine;
using System.Collections;

public class BombPickup : MonoBehaviour
{
    public AudioClip pickupClip;
    private Animator anim;
    private bool landed = false;
    // 缓存自身AudioSource组件，复用CREATE声道配置
    private AudioSource _audioSource;

    void Awake()
    {
        anim = transform.root.GetComponent<Animator>();
        // 获取当前物体挂载的AudioSource
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LayBombs layBombs = other.GetComponent<LayBombs>();
            layBombs.bombCount++; // 主角炸弹数量增加1

            // 同步更新UI显示
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.SetBombCount(layBombs.bombCount);
            }

            // 复用自身AudioSource播放音效，自动进入CREATE声道
            if (_audioSource != null && pickupClip != null)
            {
                _audioSource.PlayOneShot(pickupClip);
                // 延迟销毁根物体，保证音效完整播放完毕
                Destroy(transform.root.gameObject, pickupClip.length);
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
