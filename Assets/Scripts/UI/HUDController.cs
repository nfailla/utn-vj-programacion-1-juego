using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text livesRemainingText;
    [SerializeField] private TMP_Text ammoText;

    public void SetLivesRemaining(int lives)
    {
        livesRemainingText.text = lives == 0 ? "GAME OVER" : $"Lives x {livesRemainingText}";
    }

    public void SetAmmo(int current, int max)
    {
        ammoText.text = $"AMMO {current} / {max}";
    }

    //public void SetReloading(bool reloading)
    //{

    //}
}
