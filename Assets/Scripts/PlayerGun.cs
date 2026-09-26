using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    enum ShootDirection
    {
        Right,
        UpRight,
        Up,
        UpLeft,
        Left,
    }

    // TODO: attach these fields programmatically during Awake

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePointRight;
    [SerializeField] private Transform firePointUpRight;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointUpLeft;
    [SerializeField] private Transform firePointLeft;

    private int ClipSize = 8; // TODO: unhardcode
    private int clip;

    private float reloadingDuration = 2f; // TODO: unhardcode
    private float cooldownDuration = .2f; // TODO: unhardcode


    private float reloadingEndTime;
    private float cooldownEndTime;

    private void Start()
    {
        clip = ClipSize;
    }

    void Update()
    {
        // TODO: unhardcode all these input keys

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReloading();
            return;
        }

        if (Input.GetKey(KeyCode.L))
        {
            Shoot(ShootDirection.Right);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            Shoot(ShootDirection.UpRight);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            Shoot(ShootDirection.Up);
        }
        else if (Input.GetKey(KeyCode.U))
        {
            Shoot(ShootDirection.UpLeft);
        }
        else if (Input.GetKey(KeyCode.J))
        {
            Shoot(ShootDirection.Left);
        }
    }

    void Shoot(ShootDirection direction)
    {
        if (IsReloading() || IsCoolingDown()) return;

        // TODO: add the other directions
        Transform firePoint = direction switch {
            ShootDirection.Right => firePointRight,
            ShootDirection.UpRight => firePointUpRight,
            ShootDirection.Up => firePointUp,
            ShootDirection.UpLeft => firePointUpLeft,
            ShootDirection.Left => firePointLeft,
            _ => firePointRight, // TODO: it should actually do nothing in this case!
        };

        Vector2 bulletDirection = direction switch
        {
            ShootDirection.Right => Vector2.right,
            ShootDirection.UpRight => Vector2.up + Vector2.right,
            ShootDirection.Up => Vector2.up,
            ShootDirection.UpLeft => Vector2.up + Vector2.left,
            ShootDirection.Left => Vector2.left,
            _ => Vector2.right, // TODO: it should actually do nothing in this case!
        };

        Bullet.Spawn(bulletPrefab, firePoint.position, bulletDirection);

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
        clip = ClipSize;
    }

    private void StartCooldown()
    {
        cooldownEndTime = Time.time + cooldownDuration;
    }
}
