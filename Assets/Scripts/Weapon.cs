using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public int damage;

    public float fireRate;

    public float reloadSpeed;

    public int magsLeft;

    public int magSize;

    public float spread;

    public int pellets;

    public float bulletSpeed;

    [Space]
    public GameObject bulletPrefab;

    #region Private Variables

    private float nextFire = 0f;

    private int ammoLeft;

    private bool shooting;

    private bool reloading;

    #endregion

    private void Start()
    {
        ammoLeft = magSize;

        spread /= 100f;
    }

    private void Update()
    {
        shooting = Input.GetMouseButton(0);
        reloading = Input.GetKeyDown(KeyCode.R);

        if (shooting && Time.time >= nextFire && ammoLeft > 0 && !reloading)
        {
            nextFire = Time.time + 1f / fireRate;
            Fire();
        }

        if (reloading)
        {
            Reload();
        }
    }

    private void Fire()
    {
        ammoLeft--;
        Debug.Log("Ammo Left: " + ammoLeft);

        if (ammoLeft >= 0)
        {
            for (int i = 0; i < pellets; i++)
            {
                // Instantiate bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();

                // Randomize spread
                float spreadFactorX = Random.Range(-spread, spread);
                float spreadFactorY = Random.Range(-spread, spread);

                Vector2 direction = new Vector2(transform.up.x + spreadFactorX, transform.up.y + spreadFactorY);
                // Set bullet's properties
                bulletScript.speed = bulletSpeed;
                bulletScript.damage = damage;

                bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
            }
        }
    }

    private void Reload()
    {
        if (magsLeft > 0)
        {
            ammoLeft = magSize;
            magSize--;
        }
    }
}
