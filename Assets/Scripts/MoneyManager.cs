using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private float startMoney = 100.0f;
    private float Money {
        get {
            return Money;
        }

        // I don't know if this works, but it should invoke onMoneyChange when setting the value
        set {
            Money = value;
            onMoneyChange?.Invoke();
        }
    }

    UnityEvent onMoneyChange;

    private void Start()
    {
        Money = startMoney;
        onMoneyChange.AddListener(UpdateText);
    }

    /// <summary>
    /// Update the money count text. Prevent from repeating the same code over and over.
    /// </summary>
    private void UpdateText() {
        moneyCount.text = $"Cash: {Money}";
    }

    public float GetMoney()
    {
        return Money;
    }

    public void AddMoney(float amount)
    {
        Money += amount;
        //onMoneyChange?.Invoke();
    }

    public void SubMoney(float amount)
    {
        Money -= amount;
        //onMoneyChange?.Invoke();
    }
}
