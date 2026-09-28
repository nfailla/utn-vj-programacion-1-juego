using UnityEngine;

public class ProximitySpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // TODO: attach on Awake

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: look for a better way to match the player.
        // Also, a general purpose spawner should know
        // what's the object that triggers it; it should not
        // be hardcoded to the player only
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            // this spawner is disposable, so we destroy it as soon as we spawn
            Destroy(gameObject);
        }
    }
}
