using UnityEngine;

public class SpawnEnemyOnCollisionWithTrigger : MonoBehaviour
{
    [SerializeField] private string triggerTag;
    [SerializeField] private GameObject enemyPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(triggerTag))
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
