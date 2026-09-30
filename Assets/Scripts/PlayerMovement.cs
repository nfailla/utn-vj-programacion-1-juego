using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float GROUND_DETECTION_RADIUS = .10f; // TODO: unhardcode
    private const float MOVEMENT_SPEED = 5f; // TODO: unhardcode
    private enum LookDirection { Left, Right };

    [SerializeField] private Transform groundDetector;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D playerRigidbody2d;
    private SpriteRenderer spriteRenderer;

    private float horizontal;
    private LookDirection lastLookDirection = LookDirection.Right;
    private bool _isAttemptingToJump;
    private bool _isGrounded;

    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }

    private void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode

        IsRunning = horizontal != 0f;

        // TODO: lastLookDirection handling (flipping the spriteRenderer)
        // should be PlayerAnimation's responsibility
        lastLookDirection = horizontal < 0
            ? LookDirection.Left
            : (horizontal > 0 ? LookDirection.Right : lastLookDirection);

        spriteRenderer.flipX = lastLookDirection == LookDirection.Left;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isAttemptingToJump = true;
        }
    }
    private void FixedUpdate()
    {
        playerRigidbody2d.linearVelocityX = MOVEMENT_SPEED * horizontal;

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
}
