using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Enemies;

    [Header("生成设置")]
    // 左右生成的范围
    public float spawnRangeX = 8f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, 3f);
    }

    void SpawnEnemy()
    {
        if (Enemies.Length == 0) return;

        // 1. 随机选择一种敌人
        int index = Random.Range(0, Enemies.Length);

        // 2. 计算随机位置 (基于生成器自身的位置偏移)
        float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        // 3. 实例化敌人
        GameObject enemyObj = Instantiate(Enemies[index], spawnPosition, Quaternion.identity);

        // ========== 新增代码：随机朝向 ==========

        // 生成一个 0 到 1 之间的随机数
        float randomDirection = Random.Range(0f, 1f);

        // 获取敌人当前的缩放值
        Vector3 currentScale = enemyObj.transform.localScale;

        if (randomDirection < 0.5f)
        {
            // 50% 的概率朝左 (X轴缩放设为负数)
            // 使用 Mathf.Abs 确保无论原素材是正是负，这里都强制翻转为负
            currentScale.x = -Mathf.Abs(currentScale.x);
        }
        else
        {
            // 50% 的概率朝右 (X轴缩放设为正数)
            currentScale.x = Mathf.Abs(currentScale.x);
        }

        // 应用新的缩放
        enemyObj.transform.localScale = currentScale;

        // ======================================
    }
}