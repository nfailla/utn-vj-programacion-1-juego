using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int hitPoints = 100;
    [SerializeField] private GameObject particlesPrefab;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount, Vector2? knockback = null)
    {
        Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
        if (knockback != null) rb.AddForce(knockback.Value);
    }
}
