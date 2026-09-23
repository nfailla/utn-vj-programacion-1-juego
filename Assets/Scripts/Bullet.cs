using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10f; // TODO: unhardcode
    
    // Max lifetime of the bullet if it doesn't get destroyed first by hitting something
    private float maxLifetime = 5f; // TODO: unhardcode

    private Rigidbody2D bulletRigidbody2D;

    private void Awake()
    {
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        bulletRigidbody2D.linearVelocityX = speed;
        Destroy(gameObject, maxLifetime);
    }
}
