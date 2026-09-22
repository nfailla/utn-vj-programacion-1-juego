using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigidbody2d;
    private const float MovementSpeed = 700f;

    void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 finalForce = MovementSpeed * Time.deltaTime * horizontal * new Vector2(1, 0);
        Debug.Log("final force:" + finalForce);

        playerRigidbody2d.AddForce(finalForce);
    }
}
