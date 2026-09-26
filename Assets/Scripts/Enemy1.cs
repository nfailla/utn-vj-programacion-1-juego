using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private int hitPoints = 100; // TODO: unhardcode

    private int damage = 20;

    public GameObject player; // TODO: make private and attach on Awake

    private void Update()
    {
        FollowPlayer();
    }

    /*
     * Basic horizontal follow script
     */
    private void FollowPlayer()
    {
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

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        Debug.Log("enemy hp: " + hitPoints);
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerState playerState)) // Mmmm... is it good design? Probably just using Player is better.
        {
            playerState.TakeDamage(damage);
        }
    }
}
