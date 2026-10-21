using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GreenMecha : MonoBehaviour
{
    private static readonly int ShootHash = Animator.StringToHash("Shoot");

    [SerializeField] private string playerTag = "Player";

    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private float shootIntervalSeconds = 2f;
    [SerializeField] private float shootAnticipationTime = 1.2f; // TODO: tie to the animator keyframes

    [SerializeField] private PlayerDetector playerDetector;

    private GameObject _player;
    private Animator _animator;

    private float _elapsedTime = 0f;
    private bool _shootRequested;
    private float _shootRequestedTime;
    private bool _facingRight;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_player) {
            // if the player died and the mecha started to shoot, cancel the shoot
            _shootRequested = false;
            return;
        }

        LookTowardsPlayer();
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= shootIntervalSeconds)
        {
            _elapsedTime -= shootIntervalSeconds;
            ShootingStance();
        }

        if (_shootRequested && Time.time >= _shootRequestedTime)
        {
            Shoot();
        }
    }

    private void LookTowardsPlayer()
    {
        if (_player == null) return;

        float lookTowardsSign = Mathf.Sign(_player.transform.position.x - transform.position.x);
        _facingRight = lookTowardsSign != -1;

        float yRotationDeg = _facingRight ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0f, yRotationDeg, 0f);
    }

    private void OnEnable()
    {
        // subscribe to the player detected event
        playerDetector.OnPlayerDetected += HandlePlayerDetected;
    }

    private void OnDisable()
    {
        // unsubscribe to the player detected event
        playerDetector.OnPlayerDetected -= HandlePlayerDetected;
    }

    private void HandlePlayerDetected(GameObject player)
    {
        _player = player;
    }

    private void ShootingStance()
    {
        _animator.SetTrigger(ShootHash);
        _shootRequested = true;
        _shootRequestedTime = Time.time + shootAnticipationTime;
    }
    
    private void Shoot()
    {
        _shootRequested = false;
        Bullet.Spawn(
            bulletPrefab,
            playerTag,
            firePoint.position,
            _facingRight ? Vector2.right : Vector2.left
        );
    }
}
