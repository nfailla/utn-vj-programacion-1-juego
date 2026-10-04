using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private const string ENEMY_TAG = "Enemy"; // TODO: tag handle or whatever

    enum ShootDirection
    {
        Right,
        UpRight,
        DownRight,
        UpFacingRight,
        UpFacingLeft,
        Left,
        UpLeft,
        DownLeft,
        GroundRight,
        GroundLeft,
    }

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePointRight;
    [SerializeField] private Transform firePointUpRight;
    [SerializeField] private Transform firePointDownRight;
    [SerializeField] private Transform firePointUpFacingRight;
    [SerializeField] private Transform firePointUpFacingLeft;
    [SerializeField] private Transform firePointUpLeft;
    [SerializeField] private Transform firePointDownLeft;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointGroundRight;
    [SerializeField] private Transform firePointGroundLeft;

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
            // TODO: Cleanup. Probably map directly to a firePoint.
            // Probably the PlayerMovement should answer where it's
            // aiming at 100%, no two properties
            ShootDirection shootDirection = movement.AimingDirection switch
            {
                PlayerMovement.AimDirection.Straight =>
                    movement.FacingDirection == PlayerMovement.Direction.Left ? ShootDirection.Left : ShootDirection.Right,
                PlayerMovement.AimDirection.StraightUp =>
                    movement.FacingDirection == PlayerMovement.Direction.Left ? ShootDirection.UpLeft : ShootDirection.UpRight,
                PlayerMovement.AimDirection.StraightDown =>
                    movement.FacingDirection == PlayerMovement.Direction.Left ? ShootDirection.DownLeft : ShootDirection.DownRight,
                PlayerMovement.AimDirection.Up =>
                    movement.FacingDirection == PlayerMovement.Direction.Left ? ShootDirection.UpFacingLeft : ShootDirection.UpFacingRight,
                PlayerMovement.AimDirection.Ground =>
                    movement.FacingDirection == PlayerMovement.Direction.Left ? ShootDirection.GroundLeft : ShootDirection.GroundRight,
                _ => ShootDirection.Right,
            };
            Shoot(shootDirection);
        }
    }

    void Shoot(ShootDirection direction)
    {
        if (IsReloading() || IsCoolingDown()) return;

        // TODO: in fact, probably each firePoint should know how to shoot to its direction
        (Transform, Vector2) firePointAndDirection = direction switch {
            ShootDirection.Right =>         (firePointRight, Vector2.right),
            ShootDirection.UpRight =>       (firePointUpRight, Vector2.up + Vector2.right),
            ShootDirection.DownRight =>     (firePointDownRight, Vector2.down + Vector2.right),
            ShootDirection.UpFacingRight => (firePointUpFacingRight, Vector2.up),
            ShootDirection.UpFacingLeft =>  (firePointUpFacingLeft, Vector2.up),
            ShootDirection.UpLeft =>        (firePointUpLeft, Vector2.up + Vector2.left),
            ShootDirection.DownLeft =>      (firePointDownLeft, Vector2.down + Vector2.left),
            ShootDirection.Left =>          (firePointLeft, Vector2.left),
            ShootDirection.GroundRight =>   (firePointGroundRight, Vector2.right),
            ShootDirection.GroundLeft =>    (firePointGroundLeft, Vector2.left),
            _ => (firePointRight, Vector2.right), // TODO: it should actually do nothing in this case!
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
