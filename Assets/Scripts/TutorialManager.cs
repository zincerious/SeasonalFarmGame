using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject gameUI;
    public Button closeButton;
    public Button helpButton;

    void Start()
    {
        tutorialPanel.SetActive(true);
        gameUI.SetActive(false);
        closeButton.onClick.AddListener(CloseTutorial);
        helpButton.onClick.AddListener(OpenTutorial);
    }

    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        gameUI.SetActive(true);
    }

    void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        gameUI.SetActive(false);
    }
}
