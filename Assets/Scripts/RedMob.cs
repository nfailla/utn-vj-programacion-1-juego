using UnityEngine;

public class RedMob : MonoBehaviour
{
    private const string PLAYER_TAG = "Player"; // TODO: tag handle or whatever
    
    private int damage = 20;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player"); // TODO: this is too brittle. Change the way we find the player here
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
        // this, so the vertical position is the current enemy's vertical
        // position, always.
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = transform.position.y;

        float speed = 10f; // TODO: unhardcode. Currently: same speed as the player.

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * .001f // TODO: unhardcode
         );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            Damageable player = collision.gameObject.GetComponent<Damageable>();
            player.TakeDamage(damage);
        }
    }
}
