using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int maxHp;
    int currentHp;
    public int money = 20;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] GameObject loseUI;

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
            throw new Exception("Not enough money!");
        }
        money += amount;
        moneyText.text = money + "$";
        return amount;
    } 
    public void GetDamage(int amount)
    {
        if (amount >= currentHp)
        {
            loseUI.SetActive(true);
            enemyManager.enabled = false;
            return;
        }
        currentHp -= amount;
        hpText.text = currentHp.ToString();
    } 
}
