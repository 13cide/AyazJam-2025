using UnityEngine;

public class SealTower : Tower
{
    protected override void Attack()
    {
        base.Attack();
        Enemy e = target.GetComponent<Enemy>();
        e.GetDamage(damage);
    }
}
