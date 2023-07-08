using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private float startMoney = 100.0f;
    [SerializeField] private UnityEvent onMoneyChange;
    private float m_Money;
    private float money {
        get {
            return m_Money;
        }

        set {
            m_Money = value;
            onMoneyChange?.Invoke();
        }
    }

    private void Start()
    {
        onMoneyChange.AddListener(UpdateText);
        money = startMoney;
    }

    /// Update the money count text. Prevent from repeating the same code over and over.
    private void UpdateText() {
        moneyCount.text = $"Cash: {money}";
    }

    public float GetMoney()
    {
        return money;
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }

    public void SubMoney(float amount)
    {
        money -= amount;
    }
}
