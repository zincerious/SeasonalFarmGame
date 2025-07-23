using Assets.Scripts;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem Instance { get; private set; }
    public TimeManager timeManager;
    public ParticleSystem rainEffect;

    private bool forecastRain = false;
    public bool isRaining = false;

    private int rainStartHour;
    private int rainEndHour;

    private int lastCheckedDay = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        // Reset mỗi ngày mới
        if (timeManager.currentDay != lastCheckedDay)
        {
            lastCheckedDay = timeManager.currentDay;
            GenerateWeather();
        }

        int hour = timeManager.currentHour;

        if (forecastRain)
        {
            if (!isRaining && hour >= rainStartHour && hour < rainEndHour)
            {
                StartRain();
            }
            else if (isRaining && (hour < rainStartHour || hour >= rainEndHour))
            {
                StopRain();
            }
        }
        else if (isRaining)
        {
            StopRain();
        }
    }

    void GenerateWeather()
    {
        forecastRain = Random.value < 0.5f;

        if (forecastRain)
        {
            rainStartHour = Random.Range(6, 20);
            rainEndHour = Mathf.Min(rainStartHour + 4, 24);

            Debug.Log($"[Weather Forecast] Rain expected from {rainStartHour}:00 to {rainEndHour}:00.");
        }
        else
        {
            Debug.Log("[Weather Forecast] No rain today.");
        }
    }

    void StartRain()
    {
        isRaining = true;
        if (!rainEffect.isPlaying)
            rainEffect.Play();
        Debug.Log("Rain started!");
    }

    void StopRain()
    {
        isRaining = false;
        if (rainEffect.isPlaying)
            rainEffect.Stop();
        Debug.Log("Rain stopped.");
    }

    public bool IsRainingNow()
    {
        return isRaining;
    }
}
