using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int maxHp;
    int currentHp;
    private int money = 100;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text moneyText;

    void Start()
    {
        currentHp = maxHp;
        hpText.text = currentHp.ToString();
        moneyText.text = money + "$";
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }
    public int TakeMoney(int amount)
    {
        if (amount < 0 && math.abs(amount) > money)
        {
            Debug.Log("Not enough money!");
            return -1;
        }
        money += amount;
        moneyText.text = money + "$";
        return amount;
    } 
    public void GetDamage(int amount)
    {
        if (amount > currentHp)
        {
            Debug.Log("You are dead!");
        }
        currentHp -= amount;
        hpText.text = currentHp.ToString();
    } 
}
