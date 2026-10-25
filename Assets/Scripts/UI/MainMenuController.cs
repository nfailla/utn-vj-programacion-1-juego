using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string initialSceneName = "Level_1";
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    public void Play()
    {
        SceneManager.LoadScene(initialSceneName);
    }

    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
