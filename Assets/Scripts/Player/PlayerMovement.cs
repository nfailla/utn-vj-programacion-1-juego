using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    public enum AimDirection
    {
        Straight,
        StraightUp,
        Up,
        Down,
        StraightDown,
        Prone,
    }

    [SerializeField] private const float groundDetectionRadius = .2f;
    [SerializeField] private const float movementSpeed = 5f;

    [SerializeField] private Transform groundDetector;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 standingBulletShellParticlesPosition;
    [SerializeField] private Vector3 proneBulletShellParticlesPosition;
    [SerializeField] private ParticleSystem bulletShellParticles;

    [SerializeField] private int maxPlayerHp = 1;

    private Rigidbody2D _rb;

    private float _horizontal;
    private float _vertical;
    private bool _isAttemptingToJump;
    private bool _isGrounded;

    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAimingUp { get; private set; }
    public bool IsAimingDown { get; private set; }
    public AimDirection AimingDirection { get; private set; }

    private Damageable _damageable;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // The player always spawn and respawn in the air
        IsJumping = true;
    }

    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;

        _horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode
        _vertical = Input.GetAxisRaw("Vertical"); // TODO: unhardcode

        IsRunning = _horizontal != 0f;
        IsAimingUp = _vertical > 0f;
        IsAimingDown = _vertical < 0f;

        bool isProne = IsAimingDown && !IsRunning && !IsJumping;

        // update position of the bullet shell particles...
        // but I wonder if it would be a better idea to have two particle systems
        // in different positions and emitting in one or the other
        bulletShellParticles.transform.SetLocalPositionAndRotation(
            isProne ? proneBulletShellParticlesPosition : standingBulletShellParticlesPosition,
            bulletShellParticles.transform.rotation
        );

        bool shouldFlip = _horizontal < 0;

        if (_horizontal != 0)
        {
            float yRotationDeg = shouldFlip ? 180f : 0f;
            transform.rotation = Quaternion.Euler(0f, yRotationDeg, 0f);
        }

        if (
            Input.GetButtonDown("Jump") // TODO: unhardcode
            && _isGrounded
            )
        {
            _isAttemptingToJump = true;
        }

        AimingDirection = _vertical switch
        {
            > 0f => IsRunning ? AimDirection.StraightUp : AimDirection.Up,
            < 0f => IsRunning ? AimDirection.StraightDown : (IsJumping ? AimDirection.Down : AimDirection.Prone), // TODO: cleanup
            _ => AimDirection.Straight,
        };
    }

    private void FixedUpdate()
    {
        _rb.linearVelocityX = movementSpeed * _horizontal;

        _isGrounded = Physics2D.OverlapCircle(
            groundDetector.position,
            groundDetectionRadius,
            groundLayer
        );

        IsJumping = !_isGrounded;

        if (_isAttemptingToJump && _isGrounded)
        {
            IsJumping = true;
            _isAttemptingToJump = false;
            _rb.AddForceY(movementSpeed * 50); // TODO: unhardcode
        }
    }

    public void OnLevelFinish()
    {
        _rb.linearVelocityX = 0;
        enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // TODO: unhardcode
        {
            _damageable.TakeDamage(maxPlayerHp);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            groundDetector.position,
            groundDetectionRadius
        );
    }
}
