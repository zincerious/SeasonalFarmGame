using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightController : MonoBehaviour
{
    [Header("Overlay")]
    public Image nightOverlay;
    public float fadeDuration = 10f;
    private float targetAlpha = 0f;
    private float currentAlpha = 0f;

    [Header("Time")]
    public float currentHour = 6f;
    public float timeScale = 30f;
    private bool isNight = false;

    void Update()
    {
        currentHour += Time.deltaTime / timeScale;
        if (currentHour >= 24f) currentHour = 0f;

        if (currentHour >= 18f && !isNight)
        {
            TriggerNight();
        }
        else if (currentHour >= 6f && currentHour < 12f && isNight)
        {
            TriggerDay();
        }

        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime / fadeDuration);
        if (nightOverlay != null)
        {
            Color c = nightOverlay.color;
            c.a = currentAlpha;
            nightOverlay.color = c;
        }
    }

    public void TriggerNight()
    {
        targetAlpha = 0.76f;
        isNight = true;
    }

    public void TriggerDay()
    {
        targetAlpha = 0f;
        isNight = false;
    }

    public bool IsNight => isNight;
}
