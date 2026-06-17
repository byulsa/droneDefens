using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public float spawnDelay;
    public List<SpawnEnemyData> spawnEnemies;
}

[System.Serializable]
public class EnemyPoolData // 후에 풀링할때 적 종유를 쉽게 검색하기 위한 도구
{
    public EnemyType enemyType;
    public GameObject @object;
}

[System.Serializable]
public class SpawnEnemyData
{
    public GameObject enemyPrefab;
    public int count;
}

[System.Serializable]
public class UnlockSpawnPointData // 해금된 적 스폰지점 관리
{
    public Transform SpawnPoint;
    public bool isUnlock = false;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave")]
    public List<WaveData> waveDatas;

    public int currentWave = 0;

    public int currentEnemyCount;
    public int totalSpawnCount;

    public bool isSpawning;
    public bool isStarting;

    [SerializeField]
    private EnemySpawner enemySpawner;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //StartWave();
    }

    public void StartWave()
    {
        isStarting = true;
        StartCoroutine(enemySpawner.SpawnWave());     
        Debug.Log($"{currentWave + 1} 웨이브 시작");
    }

    public void EnemyDead()
    {
        currentEnemyCount--;

        if (currentEnemyCount <= 0 && !isSpawning)
        {
            EndWave();
        }
    }

    void EndWave()
    {
        Debug.Log("Wave End");
        isStarting = false;
        Debug.Log($"{currentWave + 1} = 현재 웨이브");
        currentWave++;
        if(currentWave % 3 == 0) GameManager.Instance.maxMemory += 20;
        enemySpawner.PointUnlock();
        //StartWave();
        
        GameManager.Instance.turretRewards.OpenReward();
    }
}
