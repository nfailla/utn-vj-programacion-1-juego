using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // finish all player custom scripts
            // TODO: these don't belong in here. Did it this way for the sake of simplicity
            collision.gameObject.TryGetComponent(out PlayerMovement playerMovement);
            collision.gameObject.TryGetComponent(out PlayerGun playerGun);
            collision.gameObject.TryGetComponent(out Damageable damageable);
            collision.gameObject.TryGetComponent(out PlayerAnimation playerAnimation);

            playerMovement.OnLevelFinish();
            playerGun.OnLevelFinish();
            damageable.OnLevelFinish();
            playerAnimation.OnLevelFinish();

            Debug.Log("Ganaste papáaaaa!");
        }
    }
}
