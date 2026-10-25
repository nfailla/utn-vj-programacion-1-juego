using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private const float TIME_SCALE_GAME_RUNNING = 1f;
    private const float TIME_SCALE_GAME_PAUSED = 0f;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private GameObject pauseMenuCanvas;

    [SerializeField] private GameObject gameOverMenuCanvas;

    [SerializeField] private float gameOverDelay = 4f;

    public static PauseManager Instance { get; private set; }
    public bool IsPaused { get; private set; }

    public event Action<bool> OnPause;

    private bool _pauseAfterGameOverRequested;
    private float _pauseAfterGameOverEndTime;

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

    private void OnEnable()
    {
        gameStateManager.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        gameStateManager.OnGameOver -= HandleGameOver;
    }

    private void Update()
    {
        if (_pauseAfterGameOverRequested && Time.time > _pauseAfterGameOverEndTime)
        {
            if (gameOverMenuCanvas.activeInHierarchy)
            {
                return; // Nothing else to do once the game is over and the game over menu is open!
            }

            Time.timeScale = TIME_SCALE_GAME_PAUSED;
            gameOverMenuCanvas.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.P)) // TODO: unhardcode
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
        OnPause?.Invoke(IsPaused);

        pauseMenuCanvas.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = TIME_SCALE_GAME_RUNNING;
        OnPause?.Invoke(IsPaused);

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

    public void HandleGameOver()
    {
        // Once the game is over, pause the game after a moment and prevent the player to pause
        _pauseAfterGameOverEndTime = Time.time + gameOverDelay;
        _pauseAfterGameOverRequested = true;
    }
}
