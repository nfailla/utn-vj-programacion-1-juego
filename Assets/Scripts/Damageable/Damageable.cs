using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private AudioClip[] enemyHitSFX;
    [SerializeField] private float enemyHitSfxPitchRangeFrom = 0.8f;
    [SerializeField] private float enemyHitSfxPitchRangeTo = 1.2f;

    [SerializeField] private AudioClip enemyDeathSFX;
    [SerializeField] private AudioClip playerDeathSFX;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int hitPoints = 100;
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private HitFlashBehaviour hitFlashBehaviour;

    public event Action OnPlayerDied;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount, Vector2? knockback = null)
    {
        Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);

            if (gameObject.CompareTag(playerTag))
            {
                OnPlayerDied?.Invoke();
                SFXManager.Instance.Play(playerDeathSFX);
            } else
            {
                SFXManager.Instance.Play(enemyDeathSFX);
            }
        }

        // TODO: checking this once again is a bit silly
        if (!gameObject.CompareTag(playerTag))
        {
            float enemyHitSfxPitch = UnityEngine.Random.Range(enemyHitSfxPitchRangeFrom, enemyHitSfxPitchRangeTo);
            SFXManager.Instance.Play(enemyHitSFX, enemyHitSfxPitch);
        }

        if (hitFlashBehaviour)
        {
            hitFlashBehaviour.Flash();
        }

        if (knockback != null) _rb.AddForce(knockback.Value);
    }

    public void OnLevelFinish()
    {
        enabled = false;
    }
}
