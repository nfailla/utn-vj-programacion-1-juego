using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RedMob : MonoBehaviour
{
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");

    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 8f;

    [SerializeField] private PlayerDetector playerDetector;

    private Rigidbody2D _rb;
    private Animator _animator;
    private GameObject _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
        _animator.SetBool(IsWalkingHash, true);
        _player = player;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    /*
     * Basic horizontal follow script
     */
    private void FollowPlayer()
    {
        if (_player == null) {
            _animator.SetBool(IsWalkingHash, false);
            return;
        }

        // the target position is the player's horizontal position.
        // We don't want the enemy to try to reach the player vertically by
        // this, so the target vertical position is the current RedMob's vertical
        // position, always.
        Vector3 targetPosition = _player.transform.position;
        targetPosition.y = transform.position.y;

        float movementSign = Mathf.Sign(_player.transform.position.x - transform.position.x);

        float targetSpeed = movementSpeed * movementSign;

        float accelerationRate = Mathf.Abs(targetSpeed) < 0.01f ? deceleration : acceleration;

        _rb.linearVelocityX = Mathf.MoveTowards(
            _rb.linearVelocityX,
            targetSpeed,
            accelerationRate * Time.fixedDeltaTime // this maxDelta is the maximum change for that given fixed interval
        );
    }
}
