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
    [SerializeField] private GameObject victoryMenuCanvas;

    [SerializeField] private float gameOverDelay = 4f;
    [SerializeField] private float victoryDelay = 4f;

    public static PauseManager Instance { get; private set; }
    public bool IsPaused { get; private set; }

    public event Action<bool> OnPause;

    private bool _gameOverMenuOpenRequested;
    private float _gameOverMenuOpenTime;

    private bool _victoryMenuOpenRequested;
    private float _victoryMenuOpenTime;

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
        gameStateManager.OnVictory += HandleVictory;
    }

    private void OnDisable()
    {
        gameStateManager.OnGameOver -= HandleGameOver;
        gameStateManager.OnVictory -= HandleVictory;
    }

    private void Update()
    {
        // Victory
        if (_victoryMenuOpenRequested && Time.time > _victoryMenuOpenTime)
        {
            if (victoryMenuCanvas.activeInHierarchy)
            {
                return; // Nothing else to do once the player won and the victory menu is open!
            }

            Time.timeScale = TIME_SCALE_GAME_PAUSED;
            victoryMenuCanvas.SetActive(true);
        }


        // Game Over
        if (_gameOverMenuOpenRequested && Time.time > _gameOverMenuOpenTime)
        {
            if (gameOverMenuCanvas.activeInHierarchy)
            {
                return; // Nothing else to do once the game is over and the game over menu is open!
            }

            Time.timeScale = TIME_SCALE_GAME_PAUSED;
            gameOverMenuCanvas.SetActive(true);
        }

        // Pause
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

    private void HandleGameOver()
    {
        _gameOverMenuOpenTime = Time.time + gameOverDelay;
        _gameOverMenuOpenRequested = true;
    }

    private void HandleVictory()
    {
        _victoryMenuOpenTime = Time.time + victoryDelay;
        _victoryMenuOpenRequested = true;
    }
}
