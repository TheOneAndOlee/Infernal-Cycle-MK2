using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class ShooterScript : MonoBehaviour
{
    #region Getters and Setters

    [Tooltip("Damage per shot")]
    [SerializeField]
    private float _damage = 10f;

    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }

    [Tooltip("Time between shots in seconds")]
    [SerializeField]
    private float _fireRate = 0.5f;

    public float FireRate
    {
        get => _fireRate;
        set => _fireRate = value;
    }

    [Tooltip("Projectile Prefab to be fired")]
    [SerializeField]
    private GameObject _bulletPrefab;

    [Tooltip("Speed of the projectile")]
    [SerializeField]
    private float _bulletSpeed = 30f;

    public float BulletSpeed
    {
        get => _bulletSpeed;
        set => _bulletSpeed = value;
    }

    private bool canShoot = true;

    #endregion

    public void FireBullet()
    {
        if (canShoot)
        {
            Debug.Log("Shooting Bullet");

            var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);

            bullet.GetComponent<PlayerBullet>().EnemyDamage = Damage;

            var bulletRbody = bullet.GetComponent<Rigidbody2D>();

            // Get the mouse position in the world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0f; // Ensure it's on the 2D plane

            // Calculate the direction vector from the player to the mouse
            Vector2 direction = (mousePos - transform.position).normalized;

            // Apply the velocity in that direction
            bulletRbody.linearVelocity = direction * BulletSpeed;

            // Optional: Rotate the bullet to face the mouse
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            canShoot = false;
            StartCoroutine(FireCooldown());
        }
        else
        {
            Debug.Log("Shooting on cooldown");
        }

    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(FireRate);
        canShoot = true;
    }
}
