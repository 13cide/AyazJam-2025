using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_3 = new WaitForSeconds(0.3f);
    public Transform enemySpawn;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] public LinkedList<GameObject> enemies = new();
    bool isSpawnFinished = true;
    bool isWaveStarted = false;

    void Update()
    {
        if (isWaveStarted && isSpawnFinished && enemies.Count == 0)
        {
            isWaveStarted = false;
            gameplayManager.WaveFinished();
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, enemySpawn.position, Quaternion.identity);
        LinkedListNode<GameObject> node = enemies.AddLast(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.nodeInManager = node;
        enemyScript.enemyManager = this;
    }

    internal IEnumerator SpawnWaveRoutine(WaveScriptableObject wave)
    {
        isSpawnFinished = false;
        isWaveStarted = true;
        for (int i = 0; i < wave.stages.Length; i++)
        {
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(wave.stages[i]);
            } else
            {
                for (int j = 0; j < (int)wave.stages[i]; j++)
                {
                    SpawnEnemy();
                    yield return _waitForSeconds0_3;
                }
            }
        }
        isSpawnFinished = true;
    }

}
