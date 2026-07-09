using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("爆炸动画持续时间")]
    public float lifeTime = 0.6f;

    void Start()
    {
        // 动画播完自动销毁
        Destroy(gameObject, lifeTime);
    }
}