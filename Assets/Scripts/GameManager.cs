using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int moneyGoal = 12000;
    public int maxGameDays = 10;
    public int currentGameDay = 1;

    public TextMeshProUGUI goalText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI deadlineText;

    public GameObject losePanel;
    public GameObject victoryPanel;
    public string currentSceneName => SceneManager.GetActiveScene().name;

    private int lastMoney = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        Instance = this;
        currentGameDay = 1;

        InventoryManager.Instance.OnMoneyChanged += HandleMoneyChanged;
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnMoneyChanged -= HandleMoneyChanged;
    }

    void HandleMoneyChanged(int newMoneyAmount)
    {
        UpdateGoalUI();
        if (newMoneyAmount >= moneyGoal && victoryPanel.activeSelf == false)
        {
            Victory();
        }
    }

    void Update()
    {
        int currentMoney = InventoryManager.Instance.GetMoney();
        if (currentMoney != lastMoney)
        {
            lastMoney = currentMoney;
            UpdateGoalUI();

            if (InventoryManager.Instance.GetMoney() >= moneyGoal)
            {
                Victory();
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "SpringFarmScene")
        {
            SceneManager.LoadScene("FallFarmScene");
        }
        else
        {
            Debug.LogWarning("No next level defined for current scene: " + currentSceneName);
        }
    }

    public void NextDay()
    {
        currentGameDay++;
        if (currentGameDay > maxGameDays)
        {
            Lose();
        }
        else
        {
            UpdateGoalUI();
        }
    }

    void UpdateGoalUI()
    {
        if (goalText != null)
            goalText.text = $"Progress: ${InventoryManager.Instance.GetMoney()} / ${moneyGoal}";

        if (dayText != null)
            dayText.text = $"Day {currentGameDay} / {maxGameDays}";

        if (deadlineText != null)
            deadlineText.text = $"Deadline: Day {maxGameDays}, 23:59";
    }

    void Victory()
    {
        victoryPanel.SetActive(true);
    }

    void Lose()
    {
        losePanel.SetActive(true);
    }
}
