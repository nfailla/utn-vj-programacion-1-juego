using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float timeToRespawn;
    [SerializeField] private MainCamera mainCamera;

    private bool _respawnRequested;
    private float _respawnTime;

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

        GameObject instantiatedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        mainCamera.FollowTarget = instantiatedPlayer.transform;
    }

    public void RequestRespawn()
    {
        _respawnRequested = true;
        _respawnTime = Time.time + timeToRespawn;
    }
}
