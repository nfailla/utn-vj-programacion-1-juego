using UnityEngine;

public class RedMob : MonoBehaviour
{
    private const string PLAYER_TAG = "Player"; // TODO: tag handle or whatever
    
    [SerializeField] private int damage = 20;
    [SerializeField] private float knockbackMagnitude = 5f;
    [SerializeField] private float movementSpeed = 1f;

    private GameObject player;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.Find("Player"); // TODO: this is too brittle. Change the way we find the player here
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FollowPlayer();
    }

    /*
     * Basic horizontal follow script
     */
    private void FollowPlayer()
    {
        if (player == null) return;

        // the target position is the player's horizontal position.
        // We don't want the enemy to try to reach the player vertically by
        // this, so the vertical position is always the current RedMob's vertical
        // position, always.
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = transform.position.y;

        float movementSign = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocityX = movementSpeed * movementSign;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            Damageable damageablePlayer = collision.gameObject.GetComponent<Damageable>();
            PlayerMovement playerMovement = damageablePlayer.GetComponent<PlayerMovement>();

            float knockbackSign = Mathf.Sign(damageablePlayer.transform.position.x - transform.position.x);
            Vector2 knockbackDirection = new Vector2(knockbackSign, 0.5f).normalized;

            playerMovement.Knockback(knockbackDirection * knockbackMagnitude);
            damageablePlayer.TakeDamage(damage);
        }
    }
}
