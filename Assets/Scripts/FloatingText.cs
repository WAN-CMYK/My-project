using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    // 飘升速度
    public float speed = 50f;
    // 存在时间（秒）
    public float lifeTime = 1.5f;

    void Update()
    {
        // 让文字向上移动 (Vector3.up)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // 初始化方法，用于设置文字内容和位置
    public void Init(string text, Color color)
    {
        GetComponent<UnityEngine.UI.Text>().text = text;
        GetComponent<UnityEngine.UI.Text>().color = color;

        // 启动协程，倒计时销毁自己
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}