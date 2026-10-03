using UnityEngine;

public class GreenMecha : MonoBehaviour
{
    [SerializeField] private float shootIntervalSeconds = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;
    
    private Animator animator;

    private float SHOOT_ANTICIPATION_TIME = 1.2f; // TODO: unhardcode; tie to the animator keyframes

    private float _elapsedTime = 0f;
    private bool _shootRequested;
    private float _shootRequestedTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= shootIntervalSeconds)
        {
            _elapsedTime -= shootIntervalSeconds;
            ShootingStance();
        }

        if (_shootRequested && Time.time >= _shootRequestedTime)
        {
            Shoot();
        }
    }

    private void ShootingStance()
    {
        animator.SetTrigger("Shoot");
        _shootRequested = true;
        _shootRequestedTime = Time.time + SHOOT_ANTICIPATION_TIME;
    }
    
    private void Shoot()
    {
        _shootRequested = false;
        Bullet.Spawn(bulletPrefab, firePoint.position, Vector2.left);
    }
}
