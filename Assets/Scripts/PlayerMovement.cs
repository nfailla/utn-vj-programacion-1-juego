using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum LookDirection { Left, Right };

    Rigidbody2D playerRigidbody2d;
    private const float MovementSpeed = 10f; // TODO: unhardcode
    private float horizontal;

    private LookDirection lastLookDirection = LookDirection.Right;
    public GameObject sprite; // TODO: better attachment

    void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode
        
        lastLookDirection = horizontal < 0
            ? LookDirection.Left
            : (horizontal > 0 ? LookDirection.Right : lastLookDirection);

        // TODO: not a big fan of this code
        sprite.TryGetComponent(out SpriteRenderer spriteRenderer);
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
