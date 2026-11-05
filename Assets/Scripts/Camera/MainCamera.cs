using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private string playerTag = "Player";

    [SerializeField] private float cameraShakeOnPlayerDeathDuration = .5f;
    [SerializeField] private float cameraShakeOnPlayerDeathIntensity = .1f;
    [SerializeField] private float cameraShakeOnExplosionDuration = .1f;
    [SerializeField] private float cameraShakeOnExplosionIntensity = .1f;

    public Transform FollowTarget { get; set; }

    private float _minXPosition;

    private float _cameraShakeRemainingTime;
    private float _cameraShakeCurrentIntensity;
    private Vector3 _shakeOffset;

    private void Awake()
    {
        AttachPlayer(GameObject.FindGameObjectWithTag(playerTag));
    }

    private void OnEnable()
    {
        gameStateManager.OnPlayerRespawn += HandlePlayerRespawn;
    }

    private void OnDisable()
    {
        gameStateManager.OnPlayerRespawn -= HandlePlayerRespawn;

        // detach OnPlayerDied event
        if (FollowTarget != null)
        {
            FollowTarget.TryGetComponent(out Damageable playerDamageable);
            if (playerDamageable != null)
            {
                playerDamageable.OnPlayerDied -= ShakeOnPlayerDied;
            }
        }
    }

    private void HandlePlayerRespawn(GameObject player)
    {
        AttachPlayer(player);
    }

    private void AttachPlayer(GameObject player)
    {
        FollowTarget = player.transform;

        player.TryGetComponent(out Damageable playerDamageable);

        if (playerDamageable != null)
        {
            playerDamageable.OnPlayerDied += ShakeOnPlayerDied;
        }
    }

    private void ShakeOnPlayerDied()
    {
        ShakeCamera(cameraShakeOnPlayerDeathDuration, cameraShakeOnPlayerDeathIntensity);
    }

    private void Start()
    {
        _minXPosition = transform.position.x;
    }

    private void LateUpdate()
    {
        // Reset the offset from the camera shake
        transform.position -= _shakeOffset;

        // Camera shake
        _shakeOffset = Vector2.zero;

        if (_cameraShakeRemainingTime > 0f)
        {
            _cameraShakeRemainingTime -= Time.deltaTime;

            Vector2 offset = Random.insideUnitCircle * _cameraShakeCurrentIntensity;
            _shakeOffset = new Vector2(offset.x, offset.y);

            transform.position += _shakeOffset; // add shake to the previously calculated position
        }

        if (!FollowTarget) return;

        // Follow the FollowTarget
        float targetXPosition = Mathf.Max(_minXPosition, FollowTarget.transform.position.x);

        _minXPosition = targetXPosition; // so the camera never goes back

        transform.position = new Vector3(
            targetXPosition,
            transform.position.y, // don't move on Y
            transform.position.z // don't move on Z
        );
    }

    public void ShakeCamera(float duration, float intensity)
    {
        _cameraShakeRemainingTime = duration;
        _cameraShakeCurrentIntensity = intensity;
    }

    public void RestartToPosition(Vector3 position)
    {
        // update the current minXPosition accordingly
        _minXPosition = position.x;

        transform.position = new Vector3(
            position.x,
            position.y,
            transform.position.z // never move on Z!
        );
    }

    public void SomethingExploded()
    {
        ShakeCamera(cameraShakeOnExplosionDuration, cameraShakeOnExplosionIntensity);
    }
}
