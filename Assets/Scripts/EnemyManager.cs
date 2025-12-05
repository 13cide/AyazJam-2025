using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform enemySpawn;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] public LinkedList<GameObject> enemies = new();

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, 1f);
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, enemySpawn.position, Quaternion.identity);
        LinkedListNode<GameObject> node = enemies.AddLast(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.nodeInManager = node;
        enemyScript.enemyManager = this;
    }
}
