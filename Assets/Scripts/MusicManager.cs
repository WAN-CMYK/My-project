using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    // 拖拽你的 AudioMixer 到这个字段
    public AudioMixer audioMixer;

    void Update()
    {
        // 空引用保护，防止未赋值时报错
        if (audioMixer == null) return;

        // 按下下方向键：音量降低10分贝
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            float volume;
            audioMixer.GetFloat("mainVolume", out volume);
            volume -= 10f;
            volume = Mathf.Clamp(volume, -80f, 20f);
            audioMixer.SetFloat("mainVolume", volume);
        }
        // 按下上方向键：音量升高10分贝
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            float volume;
            audioMixer.GetFloat("mainVolume", out volume);
            volume += 10f;
            volume = Mathf.Clamp(volume, -80f, 20f);
            audioMixer.SetFloat("mainVolume", volume);
        }
    }
}