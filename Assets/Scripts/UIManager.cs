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

        // 关闭滑块的键盘导航，彻底解决按A/D键改变音量的问题
        VolumeSlider.navigation = new Navigation { mode = Navigation.Mode.None };
    }

    /// <summary>
    /// 切换游戏暂停/继续
    /// </summary>
    void Pauser()
    {
        bPause = !bPause;
        // 优化：仅在状态切换时设置时间流速，无需每帧重复赋值
        Time.timeScale = bPause ? 0f : 1f;
    }

    /// <summary>
    /// 滑块值变化时，线性控制混音器音量
    /// </summary>
    public void OnValueChanged(float newValue)
    {
        // 滑块 0~100 线性映射到分贝 -80dB ~ 0dB
        float dbVolume = newValue / 100f * 80f - 80f;
        dbVolume = Mathf.Clamp(dbVolume, -80f, 0f);
        AudioMixer.SetFloat("mainVolume", dbVolume);
    }
}
