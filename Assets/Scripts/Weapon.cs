using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public int damage;

    public float fireRate;

    public float reloadSpeed;

    public int magSize;

    public float spread;

    public int pellets;

    public float bulletSpeed;

    [Space]
    public GameObject bulletPrefab;

    public TextMeshProUGUI ammoLeftText;

    public Slider reloadSlider;

    #region Private Variables

    private Transform bulletParent;

    private float nextFire = 0f;

    private int ammoLeft;

    private bool shooting;

    private bool reloading;

    #endregion

    private void Awake()
    {
        bulletParent = GameObject.Find("Bullets").transform;

        ammoLeft = magSize;

        spread /= 100f;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFire && ammoLeft > 0 && !reloading)
        {
            nextFire = Time.time + 1f / fireRate;
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        ammoLeftText.text = ammoLeft.ToString();

        reloadSlider.maxValue = magSize;
        reloadSlider.value = ammoLeft;
    }

    private void Fire()
    {
        ammoLeft--;

        if (ammoLeft >= 0)
        {
            for (int i = 0; i < pellets; i++)
            {
                PlayerController player = GetComponentInParent<PlayerController>();
                // Instantiate bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletParent);
                Bullet bulletScript = bullet.GetComponent<Bullet>();

                // Randomize spread
                float spreadFactorX = Random.Range(-spread, spread);
                float spreadFactorY = Random.Range(-spread, spread);

                Vector2 direction = new Vector2(transform.up.x + spreadFactorX, transform.up.y + spreadFactorY);
                // Set bullet's properties
                if (player != null)
                {
                    if (player.isPlayer1)
                        bulletScript.playerSource = 1;
                    else if (!player.isPlayer1)
                        bulletScript.playerSource = 2;
                }
                else
                    bulletScript.playerSource = 3;

                bulletScript.speed = bulletSpeed;
                bulletScript.damage = damage;

                bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
            }
        }
    }

    private void Reload()
    {
        if (!reloading)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        reloading = true;

        while (ammoLeft < magSize)
        {
            ammoLeft++;
            yield return new WaitForSeconds(reloadSpeed);
        }

        ammoLeft = magSize;

        yield return new WaitForSeconds(1f);

        reloading = false;
    }

}
