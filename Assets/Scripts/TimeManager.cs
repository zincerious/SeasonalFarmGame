using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimeManager : MonoBehaviour
    {
        public int currentHour = 6;
        public int currentMinute = 0;
        public int currentDay = 1;

        public float timePerMinute = 0.5f;
        private float timer = 0f;

        public TextMeshProUGUI timeText;  // hiển thị giờ phút hiện tại

        void Start()
        {
            UpdateTimeDisplay();
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= timePerMinute)
            {
                timer = 0f;
                currentMinute++;

                if (currentMinute >= 60)
                {
                    currentMinute = 0;
                    currentHour++;

                    if (currentHour >= 24)
                    {
                        currentHour = 0;
                        currentDay++;
                        GameManager.Instance.NextDay();
                        Debug.Log("A new day has started!");
                    }
                }

                UpdateTimeDisplay();
            }
        }

        void UpdateTimeDisplay()
        {
            if (timeText != null)
                timeText.text = $"Day {currentDay}, {currentHour:00}:{currentMinute:00}";
        }
    }
}
