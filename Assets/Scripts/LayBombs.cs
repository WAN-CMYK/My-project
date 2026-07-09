using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LayBombs : MonoBehaviour
{
    [HideInInspector]
    public bool bombLaid = false;
    public int bombCount = 0;
    public AudioClip bombsAway;
    public GameObject bomb;

    // 旧的HUD代码已停用，统一用ScoreManager管理UI
    // private Text bombHUD;

    void Start()
    {
        // ========== 新增：开局初始化炸弹数量UI ==========
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetBombCount(bombCount);
        }
        // =============================================
    }

    void Awake()
    {
        // 旧的查找HUD代码，已停用
        // bombHUD = GameObject.Find("ui_bombHUD").GetComponent<Text>();
    }

    void Update()
    {
        // 释放炸弹
        if (Input.GetButtonDown("Fire2") && !bombLaid && bombCount > 0)
        {
            bombCount--;
            bombLaid = true;
            AudioSource.PlayClipAtPoint(bombsAway, transform.position);
            Instantiate(bomb, transform.position, transform.rotation); //实例化炸弹

            // ========== 新增：扔炸弹后同步更新UI ==========
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.SetBombCount(bombCount);
            }
            // ===========================================
        }
    }
}