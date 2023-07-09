using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hourCount;
    [SerializeField] private GameObject dayEndPanel;
    [SerializeField] private TMP_Text dayCount;
    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private int hoursPerDay = 8;
    [SerializeField] private int secondPerHour = 300; // 5 Minutes
    [SerializeField] private float rent = 100;
    private int hourCounter;
    private int dayCounter;

    private void Start()
    {
        hourCounter = hoursPerDay;
        hourCount.text = "Hours left: " + hourCounter;
    }

    public void UpdateHours()
    {
        hourCounter -= 1;
        hourCount.text = "Hours left: " + hourCounter;

        if (hourCounter <= 0)
            OnDayEnd();
    }

    public void OnDayEnd()
    {
        dayCounter++;
        dayEndPanel.SetActive(true);
        dayCount.text = "Day " + dayCounter;
        float money = moneyManager.GetMoney();
        moneyManager.SubMoney(rent);
        moneyCount.text = "Cash: " + money + "\nRent: " + rent + "\nFinal: " + moneyManager.GetMoney();
        Time.timeScale = 0.0f;
    }

    public void OnDayContinue()
    {
        Time.timeScale = 1.0f;
        hourCounter = hoursPerDay;
        hourCount.text = "Hours left: " + hourCounter;
        dayEndPanel.SetActive(false);
    }
}
