using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private const string ENEMY_TAG = "Enemy"; // TODO: tag handle or whatever

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePointStraight;
    [SerializeField] private Transform firePointStraightUp;
    [SerializeField] private Transform firePointStraightDown;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointProne;

    private PlayerMovement movement;

    private int CLIP_SIZE = 8; // TODO: unhardcode
    private int clip;

    private float reloadingDuration = 2f; // TODO: unhardcode
    private float cooldownDuration = .2f; // TODO: unhardcode

    private float reloadingEndTime;
    private float cooldownEndTime;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        clip = CLIP_SIZE;
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
            Shoot(movement.AimingDirection);
        }
    }

    void Shoot(PlayerMovement.AimDirection direction)
    {
        if (IsReloading() || IsCoolingDown()) return;

        // TODO: in fact, probably each firePoint should know how to shoot to its direction
        (Transform, Vector2) firePointAndDirection = direction switch {
            PlayerMovement.AimDirection.Straight =>         (firePointStraight, firePointStraight.right),
            PlayerMovement.AimDirection.StraightUp =>       (firePointStraightUp, firePointStraightUp.up + firePointStraightUp.right),
            PlayerMovement.AimDirection.StraightDown =>     (firePointStraightDown, -firePointStraightDown.up + firePointStraightDown.right),
            PlayerMovement.AimDirection.Up =>               (firePointUp, firePointStraightDown.up),
            PlayerMovement.AimDirection.Prone =>            (firePointProne, firePointStraightDown.right),
            _ => (firePointStraight, Vector2.right), // TODO: it should actually do nothing in this case!
        };

        Transform firePoint = firePointAndDirection.Item1;
        Vector2 bulletDirection = firePointAndDirection.Item2;

        Bullet.Spawn(bulletPrefab, ENEMY_TAG, firePoint.position, bulletDirection);

        clip -= 1;

        if (clip < 1)
        {
            StartReloading();
        }
        else
        {
            StartCooldown();
        }
    }

    private bool IsReloading()
    {
        return Time.time < reloadingEndTime;
    }

    private bool IsCoolingDown()
    {
        return Time.time < cooldownEndTime;
    }

    private void StartReloading()
    {
        reloadingEndTime = Time.time + reloadingDuration;
        clip = CLIP_SIZE;
    }

    private void StartCooldown()
    {
        cooldownEndTime = Time.time + cooldownDuration;
    }
}
