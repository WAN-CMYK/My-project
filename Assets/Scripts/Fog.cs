using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [Header("水平移动设置")]
    [Tooltip("水平移动速度，正数向右飘，负数向左飘")]
    public float moveSpeed = 0.8f;

    [Header("上下浮动设置")]
    [Tooltip("是否开启上下起伏的漂浮效果")]
    public bool enableFloat = true;
    [Tooltip("上下浮动的速度")]
    public float floatSpeed = 0.5f;
    [Tooltip("上下浮动的最大距离（幅度）")]
    public float floatAmplitude = 0.6f;

    [Header("循环滚动设置")]
    [Tooltip("是否开启无限循环：飘出屏幕后从另一侧回来")]
    public bool loopScroll = true;
    [Tooltip("重置位置的距离，根据场景宽度调整")]
    public float resetDistance = 30f;

    // 记录物体初始位置，作为浮动的基准
    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        // 1. 水平方向匀速移动
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 2. 上下正弦浮动，模拟云层自然起伏
        if (enableFloat)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(
                transform.position.x,
                initialPos.y + yOffset,
                transform.position.z
            );
        }

        // 3. 循环滚动逻辑：飘出范围后从另一侧回到画面
        if (loopScroll)
        {
            // 向右飘：超出右侧边界后重置到左侧
            if (moveSpeed > 0 && transform.position.x > initialPos.x + resetDistance)
            {
                transform.position = new Vector3(
                    initialPos.x - resetDistance,
                    transform.position.y,
                    transform.position.z
                );
            }
            // 向左飘：超出左侧边界后重置到右侧
            else if (moveSpeed < 0 && transform.position.x < initialPos.x - resetDistance)
            {
                transform.position = new Vector3(
                    initialPos.x + resetDistance,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }
}