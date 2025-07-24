using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SoilManager : MonoBehaviour
{
    public Image modeIcon;
    public Sprite tillIcon;
    public Sprite plantIcon;
    public Sprite waterIcon;
    public Sprite harvestIcon;
    public Tilemap soilTilemap;
    public TileBase soilTile;
    public TileBase tilledSoilTile;
    public TileBase wateredSoilTile;
    public GameObject plantPrefab;
    private FarmMode _currentMode = FarmMode.Till;
    public FarmMode currentMode
    {
        get => _currentMode;
        set
        {
            _currentMode = value;
            UpdateModeText();
            UpdateSelectedPlantText();
        }
    }

    public TextMeshProUGUI modeText;
    public TextMeshProUGUI selectedPlantText;

    private HashSet<Vector3Int> wateredTiles = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, Plant> plantedPlants = new Dictionary<Vector3Int, Plant>();
    private Dictionary<string, PlantData> plantDataDictionary = new();

    [HideInInspector]
    public PlantData selectedPlantData;

    public void SetSelectedPlant(PlantData plantData)
    {
        selectedPlantData = plantData;
        Debug.Log("Selected plant: " + selectedPlantData.plantName);

        UpdateSelectedPlantText();
    }

    void UpdateSelectedPlantText()
    {
        if (selectedPlantText != null && currentMode == FarmMode.Plant)
        {
            selectedPlantText.text = $"Planting: {selectedPlantData?.plantName ?? "None"}";
        }
        else if (selectedPlantText != null)
        {
            selectedPlantText.text = "";
        }
    }


    void Start()
    {
        LoadPlantData();
    }

    void LoadPlantData()
    {
        plantDataDictionary.Clear();
        var allPlants = Resources.LoadAll<PlantData>("Plants");
        foreach (var plant in allPlants)
        {
            plantDataDictionary[plant.seedName] = plant;
            Debug.Log("Loaded plant data: " + plant.seedName);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMode = (FarmMode)(((int)_currentMode + 1) % 4);
            Debug.Log("Switched to mode: " + currentMode);

            UpdateModeText();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        HandleHotKeySelection();
    }

    void HandleHotKeySelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySelectSeed("Amaranth Seed");
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySelectSeed("Corn Seed");
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySelectSeed("Eggplant Seed");
        if (Input.GetKeyDown(KeyCode.Alpha4)) TrySelectSeed("Fairy Rose Seed");
        if (Input.GetKeyDown(KeyCode.Alpha5)) TrySelectSeed("Grape Seed");
        if (Input.GetKeyDown(KeyCode.Alpha6)) TrySelectSeed("Pumpkin Seed");
        if (Input.GetKeyDown(KeyCode.Alpha7)) TrySelectSeed("Sunflower Seed");
        if (Input.GetKeyDown(KeyCode.Alpha8)) TrySelectSeed("Wheat Seed");
    }

    void TrySelectSeed(string seedName)
    {
        if (!InventoryManager.Instance.GetAllItems().ContainsKey(seedName))
        {
            Debug.LogWarning("Seed not found in inventory: " + seedName);
            return;
        }

        if (plantDataDictionary.TryGetValue(seedName, out var plantData))
        {
            SetSelectedPlant(plantData);
            Debug.Log("Selected seed: " + seedName);
        }
        else
        {
            Debug.LogWarning("Plant data not found for seed: " + seedName);
        }
    }

    void HandleMouseClick()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3Int gridPos = soilTilemap.WorldToCell(worldPos);

        var currentTile = soilTilemap.GetTile(gridPos);

        switch (currentMode)
        {
            case FarmMode.Till:
                if (currentTile == soilTile)
                {
                    soilTilemap.SetTile(gridPos, tilledSoilTile);
                    SFXManager.Instance.PlayDig();
                    Debug.Log(">> Tilled at: " + gridPos);
                }
                break;

            case FarmMode.Plant:
                if (currentTile == tilledSoilTile && !plantedPlants.ContainsKey(gridPos))
                {
                    if (selectedPlantData == null)
                    {
                        Debug.LogWarning("No seed selected!");
                        return;
                    }

                    if (!InventoryManager.Instance.RemoveItem(selectedPlantData.plantName + " Seed", 1))
                    {
                        Debug.LogWarning("No seed in inventory!");
                        return;
                    }

                    Vector3 plantWorldPos = soilTilemap.GetCellCenterWorld(gridPos);
                    GameObject plantGO = Instantiate(plantPrefab, plantWorldPos, Quaternion.identity);

                    OnPlant(gridPos);

                    var animator = plantGO.GetComponent<Animator>();
                    animator.runtimeAnimatorController = selectedPlantData.plantAnimator;

                    Plant plant = plantGO.GetComponent<Plant>();
                    plant.Init(gridPos, this, selectedPlantData);

                    plantedPlants.Add(gridPos, plant);
                    SFXManager.Instance.PlayPlant();
                    Debug.Log("Planted: " + selectedPlantData.plantName + " at " + gridPos);
                }
                break;


            case FarmMode.Water:
                if (currentTile == tilledSoilTile && plantedPlants.ContainsKey(gridPos))
                {
                    soilTilemap.SetTile(gridPos, wateredSoilTile);
                    wateredTiles.Add(gridPos);
                    SFXManager.Instance.PlayWater();
                    Debug.Log("Watered at: " + gridPos);
                }
                else
                {
                    Debug.LogWarning("Can't water here: no plant or not tilled soil.");
                }
                break;


            case FarmMode.Harvest:
                Debug.Log("Harvest Mode not handled here (handled in Plant.cs)");
                break;
        }
    }
    public void OnPlant(Vector3Int tilePos)
    {
        if (wateredTiles.Contains(tilePos))
            wateredTiles.Remove(tilePos);
    }


    public void OnHarvest(Vector3Int gridPos)
    {
        if (plantedPlants.ContainsKey(gridPos))
        {
            plantedPlants.Remove(gridPos);
        }
        var currentTile = soilTilemap.GetTile(gridPos);
        Debug.Log("Current tile before reset: " + currentTile?.name);

        soilTilemap.SetTile(gridPos, soilTile);
        SFXManager.Instance.PlayHarvest();
        Debug.Log(">> Soil reset after harvest at: " + gridPos);
    }

    public bool IsTileWatered(Vector3Int gridPos)
    {
        return wateredTiles.Contains(gridPos);
    }

    void UpdateModeText()
    {
        if (modeText != null)
        {
            modeText.text = $"Mode: {currentMode}";
        }

        if (modeIcon != null)
        {
            switch (currentMode)
            {
                case FarmMode.Till:
                    modeIcon.sprite = tillIcon;
                    break;
                case FarmMode.Plant:
                    modeIcon.sprite = plantIcon;
                    break;
                case FarmMode.Water:
                    modeIcon.sprite = waterIcon;
                    break;
                case FarmMode.Harvest:
                    modeIcon.sprite = harvestIcon;
                    break;
            }
        }
    }

}

public enum FarmMode
{
    Till,
    Plant,
    Water,
    Harvest
}
