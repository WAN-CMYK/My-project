using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillTrigger : MonoBehaviour
{
    public GameObject splash;
    public AudioClip splashVoice;

    void OnTriggerEnter2D(Collider2D col)
    {
        AudioSource.PlayClipAtPoint(splashVoice, col.transform.position);

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
            // 修复变量名大小写错误，生成后自动销毁
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