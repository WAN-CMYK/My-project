using UnityEngine;

public class BombCrate : MonoBehaviour
{
    [Header("炸弹箱爆炸特效")]
    public GameObject explosionPrefab; // 把 ExplosionCircle 预制体拖到这里

    // 被火箭命中时调用，执行炸弹箱自身的爆炸逻辑
    public void Explode()
    {
        // 在箱子位置生成自身的爆炸动画
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 销毁整个炸弹箱（包含父物体、降落伞等全部组件）
        Destroy(transform.root.gameObject);
    }
}