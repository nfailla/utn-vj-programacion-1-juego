using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PauseManager pauseManager;

    [SerializeField] private TMP_Text livesRemainingText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private GameObject reloadingText;
    [SerializeField] private GameObject pauseMenuHint;

    private void OnEnable()
    {
        gameStateManager.OnLivesRemainingChanged += SetLivesRemaining;
        gameStateManager.OnAmmoChanged += SetAmmo;
        gameStateManager.OnReloading += SetReloading;

        pauseManager.OnPause += HandlePause;
    }

    private void OnDisable()
    {
        gameStateManager.OnLivesRemainingChanged -= SetLivesRemaining;
        gameStateManager.OnAmmoChanged -= SetAmmo;
        gameStateManager.OnReloading -= SetReloading;

        pauseManager.OnPause -= HandlePause;
    }

    private void SetLivesRemaining(int lives)
    {
        livesRemainingText.text = $"Lives x {lives}";
    }

    private void SetAmmo(int current, int max)
    {
        ammoText.text = $"AMMO {current} / {max}";
    }

    private void SetReloading(bool reloading)
    {
        reloadingText.SetActive(reloading);
    }

    private void HandlePause(bool pause)
    {
        pauseMenuHint.SetActive(!pause); // hide pause hint if pause menu is open
    }
}
