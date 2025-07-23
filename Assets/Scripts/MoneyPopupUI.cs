using TMPro;
using UnityEngine;

public class MoneyPopupUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float floatSpeed = 40f;
    public float duration = 1.2f;

    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private float timer;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.position = startPos + Vector3.up * (floatSpeed * timer);

        canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / duration);

        if (timer >= duration)
            Destroy(gameObject);
    }

    public void Setup(int amount)
    {
        text.text = $"{(amount >= 0 ? "+" : "")}{amount}g";
        startPos = transform.position;
    }
}
