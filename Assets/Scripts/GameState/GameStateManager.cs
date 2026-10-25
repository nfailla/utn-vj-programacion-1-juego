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

    // General game state


    // Internal state
    private bool _respawnRequested;
    private float _respawnTime;

    private void OnEnable() => playerDamageable.OnPlayerDied += HandlePlayerDied;
    private void OnDisable() => playerDamageable.OnPlayerDied -= HandlePlayerDied;

    private void Update()
    {
        if (_respawnRequested && Time.time > _respawnTime)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        _respawnRequested = false;

        GameObject instantiatedPlayer = Instantiate(playerPrefab, playerRespawnPoint.transform.position, Quaternion.identity);
        OnPlayerRespawn?.Invoke(instantiatedPlayer);

        playerGun = instantiatedPlayer.GetComponent<PlayerGun>();
        playerDamageable = instantiatedPlayer.GetComponent<Damageable>();
        playerDamageable.OnPlayerDied += HandlePlayerDied;
    }

    private void HandlePlayerDied()
    {
        _respawnRequested = true;
        _respawnTime = Time.time + timeToRespawn;
        playerDamageable.OnPlayerDied -= HandlePlayerDied;
    }
}
