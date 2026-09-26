using UnityEngine;

/**
 * Manages player state, such as HP and cooldowns
 */
public class PlayerState : MonoBehaviour
{
    // TODO: this script is a copy of Enemy1. We should have a different one
    // that handles this specifically. A possible difference is in the destroy phase.
    // that's a good case for subclassing.

    private int hitPoints = 100; // TODO: unhardcode

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        Debug.Log("player hp: " + hitPoints);
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
