using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int hitPoints = 100;
    [SerializeField] private GameObject particlesPrefab;

    // FIXME: I have this here for the sake of simplicity, but this is bad design:
    // this class is intended for other uses too, so coupling it to the
    // PlayerRespawn class is a bad idea.
    private PlayerRespawner playerRespawner;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // TODO: this is brittle. I leave this here just for simplicity
        playerRespawner = GameObject.Find("PlayerRespawner").GetComponent<PlayerRespawner>();
    }

    public void TakeDamage(int amount, Vector2? knockback = null)
    {
        Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            if (gameObject.CompareTag("Player") && playerRespawner)
            {
                playerRespawner.RequestRespawn();
            }
            Destroy(gameObject);
        }

        if (knockback != null) rb.AddForce(knockback.Value);
    }
}
