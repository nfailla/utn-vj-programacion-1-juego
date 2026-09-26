using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20f; // TODO: unhardcode
    private int damage = 25; // TODO: unhardcode
    
    // Max lifetime of the bullet if it doesn't get destroyed first by hitting something
    private float maxLifetime = 3f; // TODO: unhardcode

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit!");
        // TODO: use a superclass of Enemy1 (Enemy?)
        if (collision.gameObject.TryGetComponent(out Enemy1 enemy))
        {
            Debug.Log("Got component");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
