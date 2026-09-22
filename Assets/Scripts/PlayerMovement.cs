using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigidbody2d;
    private const float MovementSpeed = 10f;
    private float horizontal;

    void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        playerRigidbody2d.linearVelocityX = MovementSpeed * horizontal;
    }
}
