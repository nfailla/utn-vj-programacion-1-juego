using UnityEngine;

public class SpawnEnemyOnCollision : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision with " + collision.gameObject.name);

        if (collision.gameObject.name == "ColliderLeft")
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
