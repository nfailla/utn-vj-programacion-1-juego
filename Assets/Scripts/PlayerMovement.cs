using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum LookDirection { Left, Right };

    Rigidbody2D playerRigidbody2d;
    private const float MovementSpeed = 5f; // TODO: unhardcode
    private float horizontal;

    private LookDirection lastLookDirection = LookDirection.Right;
    public SpriteRenderer spriteRenderer;

    private Animator animator;

    void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode

        animator.SetBool("isRunning", horizontal != 0f); // TODO: unhardcode

        lastLookDirection = horizontal < 0
            ? LookDirection.Left
            : (horizontal > 0 ? LookDirection.Right : lastLookDirection);

        spriteRenderer.flipX = lastLookDirection == LookDirection.Left;

        TestJump();
    }

    /*
     * Just for testing purposes
     */
    private void TestJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody2d.AddForceY(MovementSpeed * 50);
        }
    }

    private void FixedUpdate()
    {
        playerRigidbody2d.linearVelocityX = MovementSpeed * horizontal;
    }
}
