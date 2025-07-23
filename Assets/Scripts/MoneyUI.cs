using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnMoneyChanged += UpdateMoneyUI;
            UpdateMoneyUI(InventoryManager.Instance.GetMoney());
        }
    }

    void UpdateMoneyUI(int newAmount)
    {
        moneyText.text = $"Money: ${newAmount}";
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnMoneyChanged -= UpdateMoneyUI;
    }
}
