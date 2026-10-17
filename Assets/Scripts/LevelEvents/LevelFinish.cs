using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // finish all player custom scripts
            // TODO: these don't belong in here. I did it this way for the sake of simplicity.
            // Maybe this is a good candidate for using signals
            collision.gameObject.TryGetComponent(out PlayerMovement playerMovement);
            collision.gameObject.TryGetComponent(out PlayerGun playerGun);
            collision.gameObject.TryGetComponent(out Damageable damageable);
            collision.gameObject.TryGetComponent(out PlayerAnimation playerAnimation);

            playerMovement.OnLevelFinish();
            playerGun.OnLevelFinish();
            damageable.OnLevelFinish();
            playerAnimation.OnLevelFinish();
        }
    }
}
