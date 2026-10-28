using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerGun : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    public int MaxClipSize { get; private set; } = 30;

    [SerializeField] private float knockbackMagnitude = 200f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePointStraight;
    [SerializeField] private Transform firePointStraightUp;
    [SerializeField] private Transform firePointStraightDown;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointProne;
    [SerializeField] private Transform firePointDown;
    [SerializeField] private ParticleSystem bulletShellParticles;

    [SerializeField] private float reloadingDuration = 2f; // duration of a full gun reload
    [SerializeField] private float cooldownDuration = .2f; // time between one bullet firing and the next one

    [SerializeField] private AudioClip shootSfx;
    [SerializeField] private float shootSfxPitchRangeFrom = 0.8f;
    [SerializeField] private float shootSfxPitchRangeTo = 1.2f;

    [SerializeField] private AudioClip[] reloadSFXs;

    public event Action<int, int> OnAmmoChanged;
    public event Action<bool> OnReloading;

    private PlayerMovement _movement;

    private int _clip;

    private bool _isReloading;
    private float _reloadingEndTime;
    private float _cooldownEndTime;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _clip = MaxClipSize;
    }

    void Update()
    {
        if (PauseManager.Instance.IsPaused) return;

        if (_isReloading && ReloadingTimeElapsed())
        {
            _isReloading = false;
            OnReloading?.Invoke(_isReloading);
            OnAmmoChanged?.Invoke(_clip, MaxClipSize);

            SFXManager.Instance.Play(reloadSFXs.Last());
        }

        // TODO: unhardcode the input keys
        if (Input.GetButton("Reload") && _clip != MaxClipSize)
        {
            StartReloading();
            return;
        }

        // TODO: unhardcode the input keys
        if (Input.GetButton("Shoot"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (_isReloading || IsCoolingDown()) return;
        Transform firePoint = _movement.AimingDirection switch {
            PlayerMovement.AimDirection.Straight =>     firePointStraight,
            PlayerMovement.AimDirection.StraightUp =>   firePointStraightUp,
            PlayerMovement.AimDirection.StraightDown => firePointStraightDown,
            PlayerMovement.AimDirection.Up =>           firePointUp,
            PlayerMovement.AimDirection.Down =>         firePointDown,
            PlayerMovement.AimDirection.Prone =>        firePointProne,
            _ => firePointStraight, // TODO: it should actually do nothing in this case!
        };

        Vector2 bulletDirection = firePoint.right; // always to the right relative to the firePoint. Each firePoint is rotated accordingly

        Bullet.Spawn(bulletPrefab, enemyTag, firePoint.position, bulletDirection, knockbackMagnitude);
        bulletShellParticles.Emit(1);

        _clip--;

        OnAmmoChanged?.Invoke(_clip, MaxClipSize);

        float sfxPitch = UnityEngine.Random.Range(shootSfxPitchRangeFrom, shootSfxPitchRangeTo);
        SFXManager.Instance.Play(shootSfx, sfxPitch);

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

    private bool ReloadingTimeElapsed() => Time.time >= _reloadingEndTime;
    private bool IsCoolingDown() => Time.time < _cooldownEndTime;

    private void StartReloading()
    {
        SFXManager.Instance.PlaySequentially(reloadSFXs.SkipLast(1).ToArray());

        _isReloading = true;
        OnReloading?.Invoke(_isReloading);

        _reloadingEndTime = Time.time + reloadingDuration;
        _clip = MaxClipSize;
    }

    private void StartCooldown()
    {
        _cooldownEndTime = Time.time + cooldownDuration;
    }

    public void OnLevelFinish()
    {
        enabled = false;
    }
}
