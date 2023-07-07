using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private float startMoney = 100.0f;
    private float money;

    private void Start()
    {
        money = startMoney;
        moneyCount.text = "Cash: " + startMoney;
    }

    public float GetMoney()
    {
        return money;
    }

    public void AddMoney(float amount)
    {
        money += amount;
        moneyCount.text = "Cash: " + money;
    }

    public void SubMoney(float amount)
    {
        money -= amount;
        moneyCount.text = "Cash: " + money;
    }
}
