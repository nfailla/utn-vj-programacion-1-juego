using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Damageable : MonoBehaviour
{
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
            if (gameObject.CompareTag("Player"))
            {
                OnPlayerDied?.Invoke();
            }
            Destroy(gameObject);
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
