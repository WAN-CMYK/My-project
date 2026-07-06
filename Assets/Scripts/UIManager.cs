using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public Button btnPause;
    public Slider VolumeSlider;
    private bool bPause = false;
    public AudioMixer AudioMixer;

    void Start()
    {
        // 注册按钮点击事件、滑块值变化事件
        btnPause.onClick.AddListener(Pauser);
        VolumeSlider.onValueChanged.AddListener(OnValueChanged);
    }

    void Update()
    {
        // 根据暂停状态控制游戏时间流速
        if (!bPause)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    /// <summary>
    /// 切换游戏暂停/继续
    /// </summary>
    void Pauser()
    {
        bPause = !bPause;
    }

    /// <summary>
    /// 滑块值变化时，线性控制混音器音量，调节手感均匀顺滑
    /// </summary>
    public void OnValueChanged(float newValue)
    {
        // 滑块 0~100 线性映射到分贝 -80dB ~ 0dB
        // 每滑动1格对应 0.8 分贝，变化均匀可控，不会突然跳变
        float dbVolume = newValue / 100f * 80f - 80f;

        // 限制极限值，防止超出混音器合法范围
        dbVolume = Mathf.Clamp(dbVolume, -80f, 0f);

        AudioMixer.SetFloat("mainVolume", dbVolume);
    }
}