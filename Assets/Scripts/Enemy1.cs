using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private int hitPoints = 100; // TODO: unhardcode

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        Debug.Log("hp: " + hitPoints);
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
