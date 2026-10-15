using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string cameraBoundaryTag;

    private float speed = 20f; // TODO: unhardcode
    private int damage = 25; // TODO: unhardcode
    
    // Max lifetime of the bullet if it doesn't get destroyed first by hitting something
    private float maxLifetime = 3f; // TODO: unhardcode; instead of controlling lifetime, make them disappear when going out of the screen
    private string _targetTag; // TODO: should probably be a list
    private Rigidbody2D bulletRigidbody2D;
    private float _knockbackMagnitude; // TODO: make parameterizable!

    public static void Spawn(
        Bullet bulletPrefab,
        string targetTag,
        Vector3 position,
        Vector2 direction,
        float knockBackMagnitude = 100f
    )
    {
        Bullet bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet._targetTag = targetTag;
        bullet._knockbackMagnitude = knockBackMagnitude;
        bullet.Shoot(direction);
    }

    private void Awake()
    {
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(cameraBoundaryTag))
        {
            // Just destroy the bullet if it goes beyond the camera boundaries
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag(_targetTag))
        {
            Damageable damageable = collision.gameObject.GetComponent<Damageable>();
            damageable.TakeDamage(damage, bulletRigidbody2D.linearVelocity.normalized * _knockbackMagnitude);
            Destroy(gameObject);
        }
    }

    private void Shoot(Vector2 direction)
    {
        // multiply the direction vector by the speed scalar
        bulletRigidbody2D.linearVelocity = direction * speed;
    }
}
