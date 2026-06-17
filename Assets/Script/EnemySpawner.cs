using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Point")]
    public List<UnlockSpawnPointData> spawnPoints;

    [Header("Enemy")]
    public List<EnemyPoolData> poolingEnemy;

    private void Start()
    {
        spawnPoints[0].isUnlock = true;
    }

    public IEnumerator SpawnWave()
    {
        WaveManager.Instance.isSpawning = true;

        int totalWaves = WaveManager.Instance.waveDatas.Count;
        int currentWave = WaveManager.Instance.currentWave;

        if (currentWave < totalWaves)
        {
            // 정상 웨이브
            WaveData waveData = WaveManager.Instance.waveDatas[currentWave];
            yield return StartCoroutine(SpawnSingleWave(waveData));
        }
        else
        {
            // 웨이브 소진 - 랜덤 2개 동시 스폰
            int indexA = Random.Range(0, totalWaves);
            int indexB;
            do { indexB = Random.Range(0, totalWaves); }
            while (indexB == indexA); // 같은 웨이브 중복 방지

            WaveData waveA = WaveManager.Instance.waveDatas[indexA];
            WaveData waveB = WaveManager.Instance.waveDatas[indexB];

            Debug.Log($"랜덤 동시 웨이브: {indexA + 1}번 + {indexB + 1}번");

            // 두 웨이브 동시 실행
            Coroutine coroutineA = StartCoroutine(SpawnSingleWave(waveA));
            Coroutine coroutineB = StartCoroutine(SpawnSingleWave(waveB));

            yield return coroutineA;
            yield return coroutineB;
        }

        WaveManager.Instance.isSpawning = false;
    }

    private IEnumerator SpawnSingleWave(WaveData waveData)
    {
        foreach (SpawnEnemyData spawnData in waveData.spawnEnemies)
        {
            for (int i = 0; i < spawnData.count; i++)
            {
                Transform spawnPoint = GetRandomSpawnPoint();
                Instantiate(spawnData.enemyPrefab, spawnPoint.position, Quaternion.identity);
                WaveManager.Instance.currentEnemyCount++;
                yield return new WaitForSeconds(waveData.spawnDelay);
            }
        }
    }

    void SpawnEnemy(EnemyData enemyData)
    {
        Transform spawnPoint = GetRandomSpawnPoint();

        GameObject prefab = GetEnemyPrefab(enemyData.enemyType);

        Instantiate(
            prefab,
            spawnPoint.position,
            Quaternion.identity);
    }
    Transform GetRandomSpawnPoint()
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (UnlockSpawnPointData point in spawnPoints)
        {
            if (point.isUnlock)
            {
                availablePoints.Add(point.SpawnPoint);
            }
        }

        return availablePoints[
            Random.Range(0, availablePoints.Count)];
    }

    public void PointUnlock()
    {
        if (WaveManager.Instance.currentWave >= 3)
        {
            spawnPoints[1].isUnlock = true;
        }

        if (WaveManager.Instance.currentWave >= 5)
        {
            spawnPoints[2].isUnlock = true;
        }
    }
    GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        foreach (EnemyPoolData poolData in poolingEnemy)
        {
            if (poolData.enemyType == enemyType)
            {
                return poolData.@object;
            }
        }

        return null;
    }
}