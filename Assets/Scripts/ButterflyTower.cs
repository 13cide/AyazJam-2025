using UnityEngine;

public class ButterflyTower : Tower
{
    [SerializeField] private GameObject potionPrefab;
    protected override void Attack()
    {
        base.Attack();
        Enemy e = target.GetComponent<Enemy>();
        GameObject potionGO = Instantiate(potionPrefab, transform.position, Quaternion.identity);
        Potion potion = potionGO.GetComponent<Potion>();
        potion.Seek(e.transform);
        audioSource.PlayOneShot(attackound);
    }
}
