using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [Tooltip("自动销毁延迟时间，单位：秒")]
    public float destroyDelay = 2f;

    void Start()
    {
        // 物体生成后，延迟指定时间自动销毁自身
        Destroy(gameObject, destroyDelay);
    }

    /// <summary>
    /// 公开销毁方法，可在动画事件、其他脚本中手动调用，立即销毁
    /// </summary>
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}