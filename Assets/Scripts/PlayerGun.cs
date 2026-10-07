using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private const string ENEMY_TAG = "Enemy"; // TODO: tag handle or whatever
    private const int MAX_CLIP_SIZE = 8; // TODO: unhardcode

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePointStraight;
    [SerializeField] private Transform firePointStraightUp;
    [SerializeField] private Transform firePointStraightDown;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointProne;
    [SerializeField] private ParticleSystem bulletShellParticles;

    private PlayerMovement movement;


    private int _clip;
    private float reloadingDuration = 2f; // TODO: unhardcode
    private float cooldownDuration = .2f; // TODO: unhardcode

    private float _reloadingEndTime;
    private float _cooldownEndTime;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _clip = MAX_CLIP_SIZE;
    }

    void Update()
    {
        if (movement.IsJumping)
        {
            // Do nothing while jumping: no shooting, no reloading
            return;
        }

        // TODO: unhardcode the input keys
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReloading();
            return;
        }

        if (Input.GetKey(KeyCode.J))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (IsReloading() || IsCoolingDown()) return;

        Transform firePoint = movement.AimingDirection switch {
            PlayerMovement.AimDirection.Straight =>     firePointStraight,
            PlayerMovement.AimDirection.StraightUp =>   firePointStraightUp,
            PlayerMovement.AimDirection.StraightDown => firePointStraightDown,
            PlayerMovement.AimDirection.Up =>           firePointUp,
            PlayerMovement.AimDirection.Prone =>        firePointProne,
            _ => firePointStraight, // TODO: it should actually do nothing in this case!
        };

        Vector2 bulletDirection = firePoint.right; // always to the right relative to the firePoint. Each firePoint is rotated accordingly

        Bullet.Spawn(bulletPrefab, ENEMY_TAG, firePoint.position, bulletDirection);
        bulletShellParticles.Emit(1);

        _clip -= 1;

        if (_clip < 1)
        {
            // TODO: autoreload, or indicate the player that they should reload (by a sound and a visual indicator)?????
            StartReloading();
        }
        else
        {
            StartCooldown();
        }
    }

    private bool IsReloading()
    {
        return Time.time < _reloadingEndTime;
    }

    private bool IsCoolingDown()
    {
        return Time.time < _cooldownEndTime;
    }

    private void StartReloading()
    {
        _reloadingEndTime = Time.time + reloadingDuration;
        _clip = MAX_CLIP_SIZE;
    }

    private void StartCooldown()
    {
        _cooldownEndTime = Time.time + cooldownDuration;
    }
}
