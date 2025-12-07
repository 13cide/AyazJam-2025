using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_3 = new WaitForSeconds(0.3f);
    public Transform enemySpawn;
    [SerializeField] GameplayManager gameplayManager;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject fatEnemyPrefab;
    [SerializeField] public LinkedList<GameObject> enemies = new();
    [SerializeField] private Sprite[] faces;
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

    void SpawnEnemy(bool isFat = false)
    {
        GameObject enemy = Instantiate(isFat ? fatEnemyPrefab : enemyPrefab, enemySpawn.position, Quaternion.identity);
        if (!isFat) enemy.GetComponent<SpriteRenderer>().sprite = faces[UnityEngine.Random.Range(0, faces.Length)];
        LinkedListNode<GameObject> node = enemies.AddLast(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.nodeInManager = node;
        enemyScript.enemyManager = this;
    }

    public void EndWave()
    {
        isSpawnFinished = true;
        StartCoroutine(deleteMobs());
    }

    IEnumerator deleteMobs()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
    }

    internal IEnumerator SpawnWaveRoutine(WaveScriptableObject wave)
    {
        isSpawnFinished = false;
        isWaveStarted = true;
        for (int i = 0; i < wave.stages.Length; i++)
        {
            if (isSpawnFinished) break;
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(wave.stages[i]);
            } else
            {
                if (wave.stages[i] == -13)
                {
                    SpawnEnemy(true);
                    yield return _waitForSeconds0_3;
                }
                else
                {
                    for (int j = 0; j < (int)wave.stages[i]; j++)
                    {
                        SpawnEnemy();
                        yield return _waitForSeconds0_3;
                    }
                }
                
            }
        }
        isSpawnFinished = true;
    }

}
