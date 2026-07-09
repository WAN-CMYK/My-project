using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public float bombRadius = 10f;      //伤害范围	
    public float bombForce = 100f;      //冲击力	
    public AudioClip boom;
    public AudioClip fuse;
    public float fuseTime = 1.5f;
    public GameObject explosion;


    private LayBombs layBombs;
    private PickupSpawner pickupSpawner;
    private ParticleSystem explosionFX;


    void Awake()
    {
        // 全部加空判断，炸弹箱场景找不到对象也不会报错
        if (GameObject.FindGameObjectWithTag("ExplosionFX"))
        {
            explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();
        }
        if (GameObject.Find("PickupManager"))
        {
            pickupSpawner = GameObject.Find("PickupManager").GetComponent<PickupSpawner>();
        }
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
        }
    }

    void Start()
    {
        // 只有独立的玩家炸弹才会自动倒计时爆炸
        // 炸弹箱子物体不会自动爆炸，只能被火箭引爆
        if (transform.root == transform)
            StartCoroutine(BombDetonation());
    }


    IEnumerator BombDetonation()
    {
        AudioSource.PlayClipAtPoint(fuse, transform.position);

        // 引信燃烧fuseTime秒.
        yield return new WaitForSeconds(fuseTime);

        // 爆炸
        Explode();
    }

    public void Explode()
    {
        // 仅玩家投掷的炸弹才执行这些逻辑，炸弹箱调用时跳过
        if (layBombs != null)
        {
            layBombs.bombLaid = false;
        }

        if (pickupSpawner != null)
        {
            // 启动协程，产生下一个道具.
            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
        }

        // 在炸弹的杀伤范围内查找敌人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));

        // 遍历杀伤的敌人
        foreach (Collider2D en in enemies)
        {
            Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
            if (rb != null && rb.CompareTag("Enemy"))
            {
                rb.gameObject.GetComponent<Enemy>().HP = 0;

                // 设置爆炸受力向量.
                Vector3 deltaPos = rb.transform.position - transform.position;

                // 受力向量方向添加爆炸力
                Vector3 force = deltaPos.normalized * bombForce;
                rb.AddForce(force);
            }
        }

        // 爆炸粒子效果（找不到就跳过）
        if (explosionFX != null)
        {
            explosionFX.transform.position = transform.position;
            explosionFX.Play();
        }

        // ========== 核心：生成爆炸圈特效（你配置的ExplosionCircle） ==========
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        AudioSource.PlayClipAtPoint(boom, transform.position);

        // 销毁整个根物体：炸弹箱会连同降落伞一起消失，玩家炸弹也正常销毁
        Destroy(transform.root.gameObject);
    }
}