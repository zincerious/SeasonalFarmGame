using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModeUIController : MonoBehaviour
{
    public TextMeshProUGUI modeText;
    public Image modeIcon;
    public Sprite tillIcon;
    public Sprite waterIcon;
    public Sprite plantIcon;

    public void SetMode(string mode)
    {
        modeText.text = "Mode: " + mode;

        switch (mode.ToLower())
        {
            case "till":
                modeIcon.sprite = tillIcon;
                break;
            case "water":
                modeIcon.sprite = waterIcon;
                break;
            case "plant":
                modeIcon.sprite = plantIcon;
                break;
            default:
                modeIcon.sprite = null;
                break;
        }
    }
}
