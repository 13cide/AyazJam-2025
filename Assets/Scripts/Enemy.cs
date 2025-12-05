using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public LinkedListNode<GameObject> nodeInManager;
    [HideInInspector] public EnemyManager enemyManager;

    [SerializeField] int maxHp;
    int currentHp;
    [SerializeField] float speed;
    Vector2 nextPos;
    int posIndex = 0;
    Map map;

    void Start()
    {
        map = GameObject.FindAnyObjectByType<Map>();
        currentHp = maxHp;
        GetNextPos();
    }

    public void GetDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("Enemy " + gameObject.name + " " + currentHp + "/" + maxHp);
        if (currentHp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        enemyManager.enemies.Remove(nodeInManager);
        Destroy(gameObject);
        Debug.Log("Enemy " + gameObject.name + " destroyed");
    }

    void GetNextPos()
    {
        nextPos = map.GetNextPosFrom(posIndex);
        if (nextPos == Vector2.negativeInfinity)
        {
            // TODO delete Enemy and give dmg
            return;
        }
        posIndex++;
    }

    void Update()
    {
        Vector2 direction = (nextPos - (Vector2)transform.position).normalized;

        Vector2 translation = direction * speed * Time.deltaTime;
        if (translation.magnitude > Vector2.Distance(transform.position, nextPos))
        {
            transform.position = nextPos;
            GetNextPos();
        }
        else
        {
            transform.Translate(translation);
        }
    }
}
