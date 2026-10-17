using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    // this works because we only have one behavior script to enable/disable
    // we would do it differently otherwise
    [SerializeField] MonoBehaviour enemyBehaviorScript;

    private void Awake()
    {
        enemyBehaviorScript.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyBehaviorScript.enabled = true;
        }
    }
}
