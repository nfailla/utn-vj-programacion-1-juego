using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    // TODO: probably we should have a single Player script that interfaces to the rest of them
    // and exposes the public API of the player to this manager
    [SerializeField] private PlayerGun playerGun;
    [SerializeField] private Damageable playerDamageable;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimation playerAnimation;
    
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject playerRespawnPoint;

    [SerializeField] private float timeToRespawn;

    [SerializeField] private LevelFinish levelFinish; // TODO: not a good name

    [SerializeField] private AudioClip respawnSFX;

    public event Action<GameObject> OnPlayerRespawn;
    public event Action<int> OnLivesRemainingChanged;
    public event Action<int, int> OnAmmoChanged;
    public event Action<bool> OnReloading;
    public event Action OnGameOver;
    public event Action OnVictory;

    private int _livesRemaining = 2;

    private bool _respawnRequested;
    private float _respawnTime;

    private void OnEnable()
    {
        playerDamageable.OnPlayerDied += HandlePlayerDied;
        playerGun.OnAmmoChanged += HandleAmmoChanged;
        playerGun.OnReloading += HandleReloading;

        levelFinish.OnReached += HandleLevelFinishReached;
    }

    private void OnDisable()
    {
        playerDamageable.OnPlayerDied -= HandlePlayerDied;
        playerGun.OnAmmoChanged -= HandleAmmoChanged;
        playerGun.OnReloading -= HandleReloading;

        levelFinish.OnReached -= HandleLevelFinishReached;
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

        SFXManager.Instance.Play(respawnSFX);

        OnPlayerRespawn?.Invoke(instantiatedPlayer);

        playerMovement = instantiatedPlayer.GetComponent<PlayerMovement>();
        playerGun = instantiatedPlayer.GetComponent<PlayerGun>();
        playerDamageable = instantiatedPlayer.GetComponent<Damageable>();
        playerAnimation = instantiatedPlayer.GetComponent<PlayerAnimation>();
        
        playerDamageable.OnPlayerDied += HandlePlayerDied;
        playerGun.OnAmmoChanged += HandleAmmoChanged;
        playerGun.OnReloading += HandleReloading;

        // Restart ammo indicator
        OnAmmoChanged?.Invoke(playerGun.MaxClipSize, playerGun.MaxClipSize);
    }

    private void HandlePlayerDied()
    {
        if (_livesRemaining > 0)
        {
            _livesRemaining--;

            _respawnRequested = true;
            _respawnTime = Time.time + timeToRespawn;

            playerDamageable.OnPlayerDied -= HandlePlayerDied;

            OnReloading?.Invoke(false); // The reloading indicator must disappear when the player is loading
        }
        else
        {
            OnGameOver?.Invoke();
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

    private void HandleLevelFinishReached()
    {
        // TODO: destroy all enemies!!!!!

        // TODO: Probably having a singletone for the game state manager would be better
        // so the player subscribes itself to the victory event and handles this
        playerMovement.OnLevelFinish();
        playerGun.OnLevelFinish();
        playerDamageable.OnLevelFinish();
        playerAnimation.OnLevelFinish();

        OnVictory?.Invoke();
    }
}
