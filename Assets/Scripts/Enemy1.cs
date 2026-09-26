using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private int hitPoints = 100; // TODO: unhardcode

    private int damage = 20;

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
