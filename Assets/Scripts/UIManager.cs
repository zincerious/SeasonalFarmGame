using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject moneyPopupPrefab;
    public Transform popupCanvas;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMoneyPopup(Vector3 worldPosition, int amount)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        GameObject go = Instantiate(moneyPopupPrefab, popupCanvas);
        go.transform.position = screenPos;

        var popup = go.GetComponent<MoneyPopupUI>();
        popup.Setup(amount);
    }
}
