using UnityEngine;

public class Pauser : MonoBehaviour
{
    // 使用属性来封装暂停逻辑，确保每次赋值时只执行一次 Time.timeScale 的设置
    public bool IsPaused
    {
        get { return paused; }
        set
        {
            paused = value;
            // 只有在状态真正改变时才设置时间缩放，避免每帧重复赋值
            Time.timeScale = paused ? 0f : 1f;
        }
    }

    private bool paused = false;

    void Update()
    {
        // 保留键盘 P 键的控制功能
        if (Input.GetKeyUp(KeyCode.P))
        {
            // 切换暂停状态
            IsPaused = !IsPaused;
        }
    }

    // 【关键】这是你需要添加的方法，供 UI 按钮调用
    public void SetPause()
    {
        // 点击按钮时，直接切换暂停状态
        IsPaused = !IsPaused;
    }
}