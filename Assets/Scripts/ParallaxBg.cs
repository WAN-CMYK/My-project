using UnityEngine;

public class ParallaxBg : MonoBehaviour
{
    [Header("拖拽主相机")]
    public Transform mainCam;
    [Header("运动补偿系数(0~1，越小动越慢)")]
    [Range(0f, 1f)] public float paraRate;

    private Vector3 camOldPos;

    void Start()
    {
        //初始化记录相机初始坐标
        camOldPos = mainCam.position;
    }

    void LateUpdate()
    {
        //1.获取相机本帧移动距离
        Vector3 camDelta = mainCam.position - camOldPos;
        //2.背景位移 = 相机位移 × 补偿系数（差异化关键）
        transform.position += new Vector3(camDelta.x * paraRate, 0, 0);
        //3.刷新上一帧相机位置
        camOldPos = mainCam.position;
    }
}