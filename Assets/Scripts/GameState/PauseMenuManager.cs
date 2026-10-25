using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private const float TIME_SCALE_GAME_RUNNING = 1f;
    private const float TIME_SCALE_GAME_PAUSED = 0f;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject hudCanvas;

    public static PauseManager Instance { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Time.timeScale = TIME_SCALE_GAME_RUNNING;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = TIME_SCALE_GAME_PAUSED;
        pauseMenuCanvas.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = TIME_SCALE_GAME_RUNNING;
        pauseMenuCanvas.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
