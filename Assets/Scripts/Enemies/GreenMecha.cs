using UnityEngine;

public class GreenMecha : MonoBehaviour
{
    private const string PLAYER_TAG = "Player"; // TODO: tag handle or whatever

    [SerializeField] private float shootIntervalSeconds = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float knockbackMagnitude = 200f;

    private GameObject player;
    private Animator animator;

    private float SHOOT_ANTICIPATION_TIME = 1.2f; // TODO: unhardcode; tie to the animator keyframes

    private float _elapsedTime = 0f;
    private bool _shootRequested;
    private float _shootRequestedTime;
    private bool _facingRight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player"); // TODO: this is too brittle. Change the way we find the player here
    }

    private void Update()
    {
        LookTowardsPlayer();
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

    private void LookTowardsPlayer()
    {
        if (player == null) return;

        float lookTowardsSign = Mathf.Sign(player.transform.position.x - transform.position.x);
        _facingRight = lookTowardsSign != -1;

        float yRotationDeg = _facingRight ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0f, yRotationDeg, 0f);
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
        Bullet.Spawn(
            bulletPrefab,
            PLAYER_TAG,
            firePoint.position,
            _facingRight ? Vector2.right : Vector2.left,
            knockbackMagnitude
        );
    }
}
