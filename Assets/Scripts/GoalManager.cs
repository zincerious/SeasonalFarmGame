using TMPro;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance;
    public int moneyGoal = 10000;
    public TextMeshProUGUI goalText;
    public GameObject victoryPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateGoalUI();
        victoryPanel.SetActive(false);
    }

    void Update()
    {
        UpdateGoalUI();

        if (InventoryManager.Instance.GetMoney() >= moneyGoal)
        {
            Victory();
        }
    }

    void UpdateGoalUI()
    {
        goalText.text = $"Progress: ${InventoryManager.Instance.GetMoney()} / ${moneyGoal}";
    }

    void Victory()
    {
        Debug.Log("YOU WIN!");
        victoryPanel.SetActive(true);
        enabled = false;
    }
}
