// HouseTrigger.cs
using UnityEngine;
using TMPro;

public class HouseTrigger : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform contentArea;
    public GameObject itemTextPrefab;
    public GameObject moneyTextPrefab;

    public InventoryManager inventoryManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player came in.");

        if (other.CompareTag("Player"))
        {
            Debug.Log("It's Player");

            ShowInventory();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited.");
            Debug.Log($"Inventory Panel Null? {inventoryPanel == null}");
            CloseInventory();
        }
    }



    void ShowInventory()
    {
        if (inventoryPanel == null || contentArea == null || itemTextPrefab == null || moneyTextPrefab == null)
        {
            Debug.LogError("UI references not set properly in HouseTrigger.");
            return;
        }

        inventoryPanel.SetActive(true);

        foreach (Transform child in contentArea)
            Destroy(child.gameObject);

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance is NULL!");
            return;
        }

        var items = InventoryManager.Instance.GetAllItems();
        Debug.Log(items.Count);
        if (items.Count <= 0)
        {
            var go = Instantiate(itemTextPrefab, contentArea);
            go.GetComponent<TextMeshProUGUI>().text = "No plants";
        }

        foreach (var pair in items)
        {
            var go = Instantiate(itemTextPrefab, contentArea);
            go.GetComponent<TextMeshProUGUI>().text = $"{pair.Key} x{pair.Value}";
            if (go != null)
            {
                Debug.Log($"{pair.Key} x{pair.Value}");
                Debug.Log("Created item line.");
            }
            else
            {
                Debug.LogError("Prefab not TextMeshProUGUI");
            }
        }

        var money = inventoryManager.GetMoney();
        var m = Instantiate(moneyTextPrefab, contentArea);
        m.GetComponent<TextMeshProUGUI>().text = $"Money: {money}";
    }

    void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

}
