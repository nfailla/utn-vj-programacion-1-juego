using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private GameObject initialSpawner;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float timeToRespawn;
    [SerializeField] private MainCamera mainCamera;

    private bool _respawnRequested;
    private float _respawnTime;

    public GameObject ActiveSpawner { get; set; }

    private void Awake()
    {
        ActiveSpawner = initialSpawner;
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
        _respawnRequested = false;

        Vector3 spawnPosition = ActiveSpawner.transform.position;

        GameObject instantiatedPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        mainCamera.RestartToPosition(spawnPosition);
        mainCamera.FollowTarget = instantiatedPlayer.transform;
    }

    public void RequestRespawn()
    {
        _respawnRequested = true;
        _respawnTime = Time.time + timeToRespawn;
    }
}
