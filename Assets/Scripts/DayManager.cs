using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hourCount;
    [SerializeField] private int hoursPerDay = 8;
    [SerializeField] private int secondPerHour = 300; // 5 Minutes
    private int hourCounter;

    private void Start()
    {
        hourCounter = hoursPerDay;
        hourCount.text = "Hours left: " + hourCounter;
        InvokeRepeating("UpdateHours", secondPerHour, secondPerHour);
    }

    private void UpdateHours()
    {
        hourCounter -= 1;
        hourCount.text = "Hours left: " + hourCounter;
    }
}
