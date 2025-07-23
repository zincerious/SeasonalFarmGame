using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Button closeButton;
    public Button helpButton;

    void Start()
    {
        tutorialPanel.SetActive(true);

        closeButton.onClick.AddListener(CloseTutorial);
        helpButton.onClick.AddListener(OpenTutorial);
    }

    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }
}
