using System;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    public event Action OnReached;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            OnReached?.Invoke();
        }
    }
}
