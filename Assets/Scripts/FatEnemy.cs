using UnityEngine;
using UnityEngine.UI;

public class FatEnemy : Enemy
{
    [SerializeField] Image hpBar;
    [SerializeField] GameObject k;

    public override void GetDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Death();
            Instantiate(k, transform.position, Quaternion.identity);
        } else
        {
            hpBar.fillAmount = (float)currentHp / maxHp;
        }
    }
}
