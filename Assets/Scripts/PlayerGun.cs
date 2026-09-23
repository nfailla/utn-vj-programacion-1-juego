using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    // TODO: attach these fields programmatically during Awake
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private Transform firePointRight;

    void Update()
    {
        // TODO: cap bullets somehow. Reload mechanics if I happen to have time

        bool isFiringRight = Input.GetButtonDown("FireRight"); // TODO: unhardcode
        if (isFiringRight)
        {
            ShootRight();
        }
        // TODO: the rest of the directions
    }

    void ShootRight()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePointRight.position, Quaternion.identity);
    }
}
