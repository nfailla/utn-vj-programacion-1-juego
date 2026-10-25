using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private TMP_Text livesRemainingText;
    [SerializeField] private TMP_Text ammoText;

    private void OnEnable()
    {
        gameStateManager.OnLivesRemainingChanged += SetLivesRemaining;
        gameStateManager.OnAmmoChanged += SetAmmo;
    }

    private void OnDisable()
    {
        gameStateManager.OnLivesRemainingChanged -= SetLivesRemaining;
        gameStateManager.OnAmmoChanged -= SetAmmo;
    }

    private void SetLivesRemaining(int lives)
    {
        livesRemainingText.text = $"Lives x {lives}";
    }

    private void SetAmmo(int current, int max)
    {
        ammoText.text = $"AMMO {current} / {max}";
    }

    //private void SetReloading(bool reloading)
    //{

    //}
}
