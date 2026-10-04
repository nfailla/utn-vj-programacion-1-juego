using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum AimDirection
    {
        Straight,
        StraightUp,
        Up,
        StraightDown,
        Ground,
    }

    public enum Direction { Left, Right }

    private const float GROUND_DETECTION_RADIUS = .10f; // TODO: unhardcode
    private const float MOVEMENT_SPEED = 5f; // TODO: unhardcode
    private const float KNOCKBACK_DURATION_SECONDS = 1f; // TODO: unhardcode

    [SerializeField] private Transform groundDetector;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D playerRigidbody2d;
    private SpriteRenderer spriteRenderer;

    private float _horizontal;
    private float _vertical;
    private bool _isAttemptingToJump;
    private bool _isGrounded;
    private bool _isKnockedBack;

    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAimingUp { get; private set; }
    public bool IsAimingDown { get; private set; }
    public Direction FacingDirection { get; private set; }
    public AimDirection AimingDirection { get; private set; }

    private void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode
        _vertical = Input.GetAxisRaw("Vertical"); // TODO: unhardcode

        IsRunning = _horizontal != 0f;
        IsAimingUp = _vertical > 0f;
        IsAimingDown = _vertical < 0f;

        bool shouldFlip = _horizontal < 0
            || (_horizontal == 0 && spriteRenderer.flipX);
        spriteRenderer.flipX = shouldFlip;
        FacingDirection = shouldFlip ? Direction.Left : Direction.Right;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isAttemptingToJump = true;
        }

        AimingDirection = _vertical switch
        {
            > 0f => IsRunning ? AimDirection.StraightUp : AimDirection.Up,
            < 0f => IsRunning ? AimDirection.StraightDown : AimDirection.Ground,
            _ => AimDirection.Straight,
        };
    }

    private void FixedUpdate()
    {
        if(!_isKnockedBack)
        {
            playerRigidbody2d.linearVelocityX = MOVEMENT_SPEED * _horizontal;
        }

        _isGrounded = Physics2D.OverlapCircle(
            groundDetector.position,
            GROUND_DETECTION_RADIUS,
            groundLayer
        );

        IsJumping = !_isGrounded;

        if (_isAttemptingToJump && _isGrounded)
        {
            IsJumping = true;
            _isAttemptingToJump = false;
            playerRigidbody2d.AddForceY(MOVEMENT_SPEED * 50);
        }
    }

    public void Knockback(Vector2 knockback)
    {
        _isKnockedBack = true;
        playerRigidbody2d.AddForce(knockback, ForceMode2D.Impulse);
        Invoke(nameof(EndKnockback), KNOCKBACK_DURATION_SECONDS);
    }
    private void EndKnockback() => _isKnockedBack = false;

}
