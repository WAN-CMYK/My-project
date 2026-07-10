using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillTrigger : MonoBehaviour
{
    public GameObject splash;
    public AudioClip splashVoice;

    // 缓存自身的AudioSource组件，避免每次触发都查找
    private AudioSource _audioSource;

    void Awake()
    {
        // 获取物体上挂载的AudioSource组件
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 用自身AudioSource播放，复用Inspector里的声道/2D/音量设置
        _audioSource.PlayOneShot(splashVoice);

        if (col.CompareTag("Player"))
        {
            // 停止相机跟随
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;

            // 关闭血条
            GameObject healthBar = GameObject.FindGameObjectWithTag("HealthBar");
            if (healthBar.activeSelf)
            {
                healthBar.SetActive(false);
            }

            // 生成水花，1秒后自动销毁
            GameObject splashInstance = Instantiate(splash, col.transform.position, transform.rotation);
            Destroy(splashInstance, 1f);

            Destroy(col.gameObject);
            StartCoroutine("ReloadGame");
        }
        else
        {
            GameObject splashInstance = Instantiate(splash, col.transform.position, transform.rotation);
            Destroy(splashInstance, 1f);

            Destroy(col.gameObject);
        }
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
