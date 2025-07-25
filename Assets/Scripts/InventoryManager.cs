using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private int money = 1000;
    public delegate void MoneyChanged(int newAmount);
    public event MoneyChanged OnMoneyChanged;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory[itemName] = 1;
        }

        Debug.Log($"Added into inventory: {itemName} x{inventory[itemName]}");
    }

    public Dictionary<string, int> GetAllItems()
    {
        return inventory;
    }

    public int GetItemCount(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
    }

    public bool RemoveItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName) && inventory[itemName] >= amount)
        {
            inventory[itemName] -= amount;
            if (inventory[itemName] <= 0)
                inventory.Remove(itemName);

            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone.");
            ShowInventory();
        }
    }
    private void ShowInventory()
    {
        // Logic to show inventory UI
        Debug.Log("Showing inventory UI");
        // This would typically involve activating a UI panel and populating it with items
    }

    public int GetMoney() => money;

    public bool HasMoney(int amount) => money >= amount;

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money earned: " + amount + " | Current: " + money);
        OnMoneyChanged?.Invoke(money);
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            Debug.Log("Money spent: " + amount + " | Current: " + money);
            OnMoneyChanged?.Invoke(money);
            return true;
        }

        Debug.Log("Not enough money.");
        return false;
    }


    public bool SellItem(string itemName, int amount, int sellPrice)
    {
        if (RemoveItem(itemName, amount))
        {
            AddMoney(sellPrice * amount);
            Debug.Log($"Sold {amount} x {itemName} for ${sellPrice * amount}");
            return true;
        }

        Debug.Log("Not enough items to sell");
        return false;
    }

}