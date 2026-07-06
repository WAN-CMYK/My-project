using UnityEngine;

public class Swan : MonoBehaviour
{
    public float moveSpeed = 3f;       // 向左移动速度
    public float leftBoundary = -15f;  // 左边界：小于这个X坐标就重置
    public float rightResetX = 15f;    // 重置位置：回到右边的X坐标

    void Update()
    {
        // 持续向左移动
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // 超出左边界后，重置到右边位置
        if (transform.position.x <= leftBoundary)
        {
            Vector3 pos = transform.position;
            pos.x = rightResetX;
            transform.position = pos;
        }
    }
}