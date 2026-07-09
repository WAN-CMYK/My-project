using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("分数显示")]
    public Text scoreText;
    private int currentScore = 0;

    [Header("炸弹数量显示")]
    public Text bombCountText;
    private int currentBombCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 如果脚本挂在文本物体上，自动获取组件（可选）
        // scoreText = GetComponent<Text>();
    }

    void Start()
    {
        UpdateScoreText();
        UpdateBombText();
    }

    // ========== 分数相关方法 ==========
    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "分数：" + currentScore;
        }
    }

    // ========== 炸弹相关方法 ==========
    // 增加炸弹数量（拾取炸弹时调用）
    public void AddBomb(int count = 1)
    {
        currentBombCount += count;
        UpdateBombText();
    }

    // 消耗炸弹（扔炸弹时调用，返回是否还有炸弹可用）
    public bool UseBomb()
    {
        if (currentBombCount > 0)
        {
            currentBombCount--;
            UpdateBombText();
            return true;
        }
        return false; // 炸弹数量为0，无法使用
    }

    // 直接设置炸弹数量（初始化/重置时用）
    public void SetBombCount(int count)
    {
        currentBombCount = Mathf.Max(0, count);
        UpdateBombText();
    }

    void UpdateBombText()
    {
        if (bombCountText != null)
        {
            bombCountText.text = "炸弹：" + currentBombCount;
        }
    }
}