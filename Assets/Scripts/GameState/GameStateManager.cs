using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    // TODO: probably we should have a single Player script that interfaces to the rest of them
    // and exposes the public API of the player to this manager
    [SerializeField] private PlayerGun playerGun;
    [SerializeField] private Damageable playerDamageable;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject playerRespawnPoint;

    [SerializeField] private float timeToRespawn;

    // TODO:
    // 1. Subscribe to Player. When it dies:
    //   - If there are lives left, respawn in respawn point (ref to it) and subtract to the remaining lives
    //   - If there are no lives left, show GAME OVER after .5f sec.
    // 2. Know the player gun's bullets
    // 3. Know if the player gun is reloading

    public event Action<GameObject> OnPlayerRespawn;
    public event Action<int> OnLivesRemainingChanged;
    public event Action<int, int> OnAmmoChanged;
    public event Action<bool> OnReloading;

    // General game state
    private int _livesRemaining = 2;

    // Internal state
    private bool _respawnRequested;
    private float _respawnTime;

    private void OnEnable()
    {
        playerDamageable.OnPlayerDied += HandlePlayerDied;
        playerGun.OnAmmoChanged += HandleAmmoChanged;
        playerGun.OnReloading += HandleReloading;
    }

    private void OnDisable()
    {
        playerDamageable.OnPlayerDied -= HandlePlayerDied;
        playerGun.OnAmmoChanged -= HandleAmmoChanged;
        playerGun.OnReloading -= HandleReloading;
    }

    private void Update()
    {
        if (_respawnRequested && Time.time > _respawnTime)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // We call it here instead of calling it on HandlePlayerDied so lives
        // don't go down immediately, which is less appealing
        OnLivesRemainingChanged(_livesRemaining);

        _respawnRequested = false;

        GameObject instantiatedPlayer = Instantiate(playerPrefab, playerRespawnPoint.transform.position, Quaternion.identity);
        OnPlayerRespawn?.Invoke(instantiatedPlayer);

        playerGun = instantiatedPlayer.GetComponent<PlayerGun>();
        playerDamageable = instantiatedPlayer.GetComponent<Damageable>();
        
        playerDamageable.OnPlayerDied += HandlePlayerDied;
    }

    private void HandlePlayerDied()
    {
        if (_livesRemaining > 0)
        {
            _livesRemaining--;

            _respawnRequested = true;
            _respawnTime = Time.time + timeToRespawn;

            playerDamageable.OnPlayerDied -= HandlePlayerDied;
        }
        else
        {
            // TODO: Game Over
        }
    }

    private void HandleAmmoChanged(int current, int max)
    {
        OnAmmoChanged?.Invoke(current, max);
    }

    private void HandleReloading(bool isReloading)
    {
        OnReloading?.Invoke(isReloading);
    }
}
