using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public LinkedListNode<GameObject> nodeInManager;
    [HideInInspector] public EnemyManager enemyManager;

    [SerializeField] protected int maxHp;
    protected int currentHp;
    [SerializeField] float speed;
    [SerializeField] int moneyReward;
    [SerializeField] int damageToBase = 10;
    private EconomyManager economyManager;
    Vector2 nextPos;
    int posIndex = 0;
    Map map;

    void Start()
    {
        map = GameObject.FindAnyObjectByType<Map>();
        economyManager = GameObject.FindAnyObjectByType<EconomyManager>();
        currentHp = maxHp;
        GetNextPos();
    }

    public virtual void GetDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        economyManager.TakeMoney(moneyReward);
        enemyManager.enemies.Remove(nodeInManager);
        Destroy(gameObject);
    }

    void GetNextPos()
    {
        nextPos = map.GetNextPosFrom(posIndex);
        if (nextPos.x == -1488f)
        {
            enemyManager.enemies.Remove(nodeInManager);
            economyManager.GetDamage(damageToBase);
            Destroy(gameObject);
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
