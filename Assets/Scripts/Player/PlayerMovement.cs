using UnityEngine;

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

    private const float GROUND_DETECTION_RADIUS = .2f; // TODO: unhardcode
    private const float MOVEMENT_SPEED = 5f; // TODO: unhardcode
    private const float KNOCKBACK_DURATION_SECONDS = 1f; // TODO: unhardcode

    [SerializeField] private Transform groundDetector;
    [SerializeField] private LayerMask groundLayer;
    
    private BoxCollider2D hitbox;
    [SerializeField] private Vector2 standingHitboxOffset;
    [SerializeField] private Vector2 standingHitboxSize;
    
    [SerializeField] private Vector2 proneHitboxOffset;
    [SerializeField] private Vector2 proneHitboxSize;

    [SerializeField] private Vector2 jumpingHitboxOffset;
    [SerializeField] private Vector2 jumpingHitboxSize;

    [SerializeField] private Vector3 standingBulletShellParticlesPosition;
    [SerializeField] private Vector3 proneBulletShellParticlesPosition;
    [SerializeField] private ParticleSystem bulletShellParticles;

    private Rigidbody2D playerRigidbody2d;

    private float _horizontal;
    private float _vertical;
    private bool _isAttemptingToJump;
    private bool _isGrounded;
    private bool _isKnockedBack;

    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAimingUp { get; private set; }
    public bool IsAimingDown { get; private set; }
    public AimDirection AimingDirection { get; private set; }

    private void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode
        _vertical = Input.GetAxisRaw("Vertical"); // TODO: unhardcode

        IsRunning = _horizontal != 0f;
        IsAimingUp = _vertical > 0f;
        IsAimingDown = _vertical < 0f;
        
        // TODO: a state machine would improve this code dramatically

        bool isProne = IsAimingDown && !IsRunning && !IsJumping;

        // update hitbox
        if (IsJumping)
        {
            // set hitbox for the player when jumping
            hitbox.size = jumpingHitboxSize;
            hitbox.offset = jumpingHitboxOffset;
        }
        else
        {
            // set hitbox for the player when standing or prone
            hitbox.size = isProne ? proneHitboxSize : standingHitboxSize;
            hitbox.offset = isProne ? proneHitboxOffset : standingHitboxOffset;
        }

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

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
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

    public void OnLevelFinish()
    {
        // HACK: since the celebration stance is standing, if
        // the hitbox is different than the standing one for whatever
        // reason it looks buggy. I have to find a better way to do this.
        hitbox.size = standingHitboxSize;
        hitbox.offset = standingHitboxOffset;

        playerRigidbody2d.linearVelocityX = 0;
        enabled = false;
    }
}
