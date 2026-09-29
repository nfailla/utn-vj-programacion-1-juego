using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20f; // TODO: unhardcode
    private int damage = 25; // TODO: unhardcode
    
    // Max lifetime of the bullet if it doesn't get destroyed first by hitting something
    private float maxLifetime = 3f; // TODO: unhardcode

    [SerializeField] private GameObject bulletParticlesPrefab;

    private Rigidbody2D bulletRigidbody2D;
    private float knockbackMagnitude; // TODO: make parameterizable!

    public static void Spawn(Bullet bulletPrefab, Vector3 position, Vector2 direction)
    {
        Bullet bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.Shoot(direction);
    }

    private void Awake()
    {
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        knockbackMagnitude = 100;
        Destroy(gameObject, maxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: use a superclass of Enemy1 (Enemy?)
        if (collision.gameObject.TryGetComponent(out Enemy1 enemy))
        {
            enemy.TakeDamage(damage, bulletRigidbody2D.linearVelocity.normalized * knockbackMagnitude);
            Instantiate(bulletParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Shoot(Vector2 direction)
    {
        // multiply the direction vector by the speed scalar
        bulletRigidbody2D.linearVelocity = direction * speed;
    }
}
