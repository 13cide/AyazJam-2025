using UnityEngine;
using UnityEngine.UI;

public class FatEnemy : Enemy
{
    [SerializeField] Image hpBar;

    public override void GetDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Death();
        } else
        {
            hpBar.fillAmount = (float)currentHp / maxHp;
        }
    }
}
