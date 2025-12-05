using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tower : MonoBehaviour
{

    [SerializeField] float attackRange;
    [SerializeField] GameObject attackZone;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int damage = 1;
    EnemyManager enemyManager;

    bool isPlaced = false;
    bool isSelected = false;
    GameObject target;
    
    void Start()
    {
        float range = (attackRange/transform.localScale.x)*2f;
        attackZone.transform.localScale = new Vector3(range, range, 1);
        enemyManager = GameObject.FindAnyObjectByType<EnemyManager>();
        InvokeRepeating(nameof(FindTarget), 0f, 0.25f);
    }

    public void Placed()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 1.0f;
        renderer.color = color;

        attackZone.SetActive(false);
    StartCoroutine(AttackRoutine());
        isPlaced = true;

    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);

            if (!isPlaced) continue;
            if (target == null) continue;

            Enemy e = target.GetComponent<Enemy>();
            if (e != null)
            {
                e.GetDamage(damage);
            }
            else
            {
                Debug.LogWarning("Tower tried to attack a target without an Enemy component.");
            }
        }
    }

    public void Select(bool shouldSelect) {
        isSelected = shouldSelect;
        attackZone.SetActive(shouldSelect);
    }

    void FindTarget() {
        if (target != null && Vector2.Distance(transform.position, target.transform.position) <= attackRange) return;

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemyManager.enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (shortestDistance <= attackRange)
        {
            target = nearestEnemy;
            Debug.Log("Target acquired: " + target.name);
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (!isPlaced) return;
    }
}
        /* among us DONT DELETE OR I FUCK YOUR MOM
        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣤⣤⣤⣀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣼⠟⠉⠉⠉⠉⠉⠉⠉⠙⠻⢶⣄⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀    ⠀⠙⣷⡀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⡟⠀⣠⣶⠛⠛⠛⠛⠛⠛⠳⣦⡀⠀⠘⣿⡄⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⠁⠀⢹⣿⣦⣀⣀⣀⣀⣀⣠⣼⡇⠀⠀⠸⣷⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⡏⠀⠀⠀⠉⠛⠿⠿⠿⠿⠛⠋⠁⠀⠀⠀⠀⣿
⠀⠀       ⢠⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡇⠀
        ⠀⣸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀
⠀⠀⠀⠀⠀⠀⠀⠀⣿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣧⠀
⠀⠀⠀⠀⠀⠀⠀⢸⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⠀
⠀⠀⠀⠀⠀⠀⠀⣾⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ ⠀⣿⠀
⠀⠀⠀⠀⠀⠀⠀⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀  ⠀⠀⠀⠀⣿⠀
⠀⠀⠀⠀⠀⠀⢰⣿⠀⠀⠀⠀⣠⡶⠶⠿⠿⠿⠿⢷⣦⠀⠀⠀⠀⠀    ⣿⠀
⠀⠀⣀⣀⣀⠀⣸⡇⠀⠀⠀⠀⣿⡀⠀⠀⠀⠀⠀⠀⣿⡇⠀⠀⠀⠀⠀⠀⣿⠀
⣠⡿⠛⠛⠛⠛⠻⠀⠀⠀⠀⠀⢸⣇⠀⠀⠀⠀⠀⠀⣿⠇⠀⠀⠀⠀⠀ ⠀⣿⠀
⢻⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣼⡟⠀⠀⢀⣤⣤⣴⣿⠀⠀⠀⠀⠀⠀  ⠀⣿⠀
⠈⠙⢷⣶⣦⣤⣤⣤⣴⣶⣾⠿⠛⠁⢀⣶⡟⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡟⠀
                  ⠈⣿⣆⡀⠀⠀⠀⠀⠀⠀⢀⣠⣴⡾⠃⠀
⠀                 ⠈⠛⠻⢿⣿⣾⣿⡿⠿⠟⠋⠁⠀⠀⠀
        */