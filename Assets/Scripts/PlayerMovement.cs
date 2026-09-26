using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigidbody2d;
    private const float MovementSpeed = 10f; // TODO: unhardcode
    private float horizontal;

    void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // TODO: unhardcode
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
