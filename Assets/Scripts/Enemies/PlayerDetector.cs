using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    public event Action<GameObject> OnPlayerDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            OnPlayerDetected?.Invoke(collision.gameObject);
        }
    }
}
