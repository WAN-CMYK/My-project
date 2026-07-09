using System.Collections;
using System.Collections.Generic;
using UnityEngine; // 【关键修复1】必须加上这个，否则无法使用 Destroy 和 MonoBehaviour
using UnityEngine.Audio; // 【关键修复2】如果你想用代码指定 Mixer，需要这个（虽然下面用的是组件法，但加上保险）

public class KillTrigger : MonoBehaviour
{
    // 在 Inspector 面板里把 waterSplash 音频拖到这里
    public AudioClip splashVoice;

    // 获取 Audio Source 组件的变量
    private AudioSource myAudioSource;

    void Start()
    {
        // 游戏开始时，获取挂载在这个物体上的 Audio Source 组件
        myAudioSource = GetComponent<AudioSource>();

        // 如果忘了挂 Audio Source 组件，这里会报错，所以加个判断
        if (myAudioSource == null)
        {
            Debug.LogError("KillTrigger 物体上缺少 AudioSource 组件！");
        }
    }

    // 【关键修复3】确保这个函数是在 class 的直接层级，不要包在其他函数里
    void OnTriggerEnter2D(Collider2D col)
    {
        // 1. 播放声音逻辑
        // 只有当 Audio Source 和 音频片段都存在时才播放
        if (myAudioSource != null && splashVoice != null)
        {
            // 将音频片段赋值给组件并播放
            // 这样声音就会走你在 Inspector 面板里设置好的 "WATER" 输出通道了
            myAudioSource.clip = splashVoice;
            myAudioSource.Play();
        }

        // 2. 碰撞检测逻辑
        // 如果碰到的是玩家（假设玩家的 Tag 是 "Player"）
        if (col.CompareTag("Player"))
        {
            // 这里写你原来的游戏逻辑，比如扣分、重置位置等
            // 示例：
            // FindObjectOfType<GameManager>().ResetLevel(); 

            // 如果你需要销毁这个触发器本身（看报错你之前写了 Destroy）
            // Destroy(gameObject); 
        }
    }
}