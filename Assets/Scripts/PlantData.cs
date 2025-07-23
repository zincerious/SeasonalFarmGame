using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantData", menuName = "Farming/Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public string seedName;
    public string productName;
    public int buyPrice;
    public int baseSellPrice;
    public float growTime; // in game seconds
    public Season[] growSeasons;
    public AnimatorOverrideController plantAnimator;

    // Gacha-based sell price
    public int GetSellPrice()
    {
        float rand = Random.value;
        float multiplier;

        if (plantName == "Pumpkin" || plantName == "Fairy Rose")
        {
            if (rand < 0.5f) multiplier = 1.0f;
            else if (rand < 0.8f) multiplier = 1.2f;
            else if (rand < 0.95f) multiplier = 1.5f;
            else multiplier = 2.0f;
        }
        else
        {
            if (rand < 0.5f) multiplier = 0.8f;
            else if (rand < 0.8f) multiplier = 1.0f;
            else if (rand < 0.95f) multiplier = 1.2f;
            else multiplier = 1.5f;
        }

        if (WeatherSystem.Instance != null && WeatherSystem.Instance.isRaining)
        {
            multiplier *= 0.8f;
        }

        return Mathf.RoundToInt(baseSellPrice * multiplier);
    }
}

public enum Season { Spring, Summer, Fall, Winter }
