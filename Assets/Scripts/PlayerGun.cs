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
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private Transform firePointRight;

    private int ClipSize = 8; // TODO: unhardcode
    private int clip;

    private float reloadingDuration = 2f; // TODO: unhardcode
    private float cooldownDuration = .2f; // TODO: unhardcode


    private float reloadingEndTime;
    private float cooldownEndTime;

    private void Start()
    {
        clip = ClipSize;
        //isReloading = false;
        //isCoolingDown = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReloading();
            return;
        }

        bool isFiringRight = Input.GetButton("FireRight"); // TODO: unhardcode
        // TODO: cooldown between bullets
        if (isFiringRight)
        {
            Shoot(ShootDirection.Right);
        }
        // TODO: the rest of the directions
    }

    void Shoot(ShootDirection direction)
    {
        if (IsReloading() || IsCoolingDown()) return;

        // TODO: add the other directions
        Transform firePoint = direction switch {
            ShootDirection.Right => firePointRight,
            _ => firePointRight
        };

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
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
