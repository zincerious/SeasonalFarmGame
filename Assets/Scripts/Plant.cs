using UnityEngine;

public class Plant : MonoBehaviour
{
    public float waterLimitHours = 6f;
    public float inGameHourLength = 30f;

    private float waterTimer = 0f;
    private float growTimer = 0f;

    private bool sprouted = false;
    private bool isMature = false;
    private bool isDead = false;

    private Vector3Int gridPosition;
    private SoilManager soilManager;
    private Animator animator;
    private PlantData plantData;
    private float fertilizerBonus = 0f;

    public void ApplyFertilizer(float reduceSeconds)
    {
        if (!isMature && !isDead)
        {
            plantData.growTime = Mathf.Max(0f, plantData.growTime - reduceSeconds);
            Debug.Log($"New growTime: {plantData.growTime} seconds.");
        }
    }

    public void Init(Vector3Int pos, SoilManager manager, PlantData data)
    {
        gridPosition = pos;
        soilManager = manager;
        plantData = data;
    }

    public void SetData(PlantData data)
    {
        plantData = data;
        if (data.plantAnimator != null)
        {
            GetComponent<Animator>().runtimeAnimatorController = data.plantAnimator;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || soilManager == null || isMature) return;

        if (!soilManager.IsTileWatered(gridPosition))
        {
            waterTimer += Time.deltaTime;

            if (waterTimer >= waterLimitHours * inGameHourLength)
            {
                Debug.Log("Plant died (no water for 6 hours).");
                isDead = true;

                animator.SetInteger("GrowthStage", -1);
                Invoke(nameof(DestroySelf), 1.5f);
                return;
            }
        }
        else
        {
            if (!sprouted)
            {
                sprouted = true;
                animator.SetInteger("GrowthStage", 1);
                Debug.Log("Sprouted at " + gridPosition);
            }

            growTimer += Time.deltaTime;

            if (plantData != null && growTimer >= plantData.growTime - fertilizerBonus)
            {
                isMature = true;
                animator.SetInteger("GrowthStage", 2);
                Debug.Log("Matured at " + gridPosition);
            }
        }
    }

    void DestroySelf()
    {
        soilManager.OnHarvest(gridPosition);
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        if (soilManager.currentMode != FarmMode.Harvest)
        {
            Debug.Log("Not in Harvest Mode");
            return;
        }

        if (isMature)
        {
            Debug.Log("Harvested.");

            string productName = plantData.productName;
            int sellPrice = plantData.GetSellPrice();

            InventoryManager.Instance.AddMoney(sellPrice);
            UIManager.Instance.ShowMoneyPopup(transform.position, sellPrice);
            Debug.Log($"Harvested and sold {productName} for ${sellPrice}");

            soilManager.OnHarvest(gridPosition);
            Destroy(gameObject);
        }

        else if (isDead)
        {
            Debug.Log("This plant has withered.");
        }
        else
        {
            Debug.Log("Not ready yet.");
        }
    }

}
