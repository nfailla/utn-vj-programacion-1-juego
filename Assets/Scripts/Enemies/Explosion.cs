using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float delayAfterDestroy = 2f;

    private void Start()
    {
        Destroy(gameObject, delayAfterDestroy);
    }
}
