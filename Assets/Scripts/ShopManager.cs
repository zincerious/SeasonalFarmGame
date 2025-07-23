using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public Transform contentArea;
    public GameObject shopItemPrefab;
    public List<PlantData> availablePlants;

    public void OpenShop()
    {
        shopPanel.SetActive(true);

        foreach (Transform child in contentArea)
            Destroy(child.gameObject);

        foreach (var plant in availablePlants)
        {
            var go = Instantiate(shopItemPrefab, contentArea);
            go.transform.Find("ItemText").GetComponent<TextMeshProUGUI>().text = plant.plantName;
            go.transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = $"${plant.buyPrice}";
            go.transform.Find("Button_Buy").GetComponent<Button>().onClick.AddListener(() => BuyPlant(plant));
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("shopPanel destroyed or not be in");
        }
    }

    void BuyPlant(PlantData plant)
    {
        if (InventoryManager.Instance.HasMoney(plant.buyPrice))
        {
            InventoryManager.Instance.SpendMoney(plant.buyPrice);
            InventoryManager.Instance.AddItem(plant.plantName + " Seed");
            UIManager.Instance.ShowMoneyPopup(shopPanel.transform.position, (0 - plant.buyPrice));
            Debug.Log("Bought: " + plant.plantName);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
