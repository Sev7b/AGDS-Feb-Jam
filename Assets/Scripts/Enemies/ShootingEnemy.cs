using UnityEngine;

public class ShootingEnemy : Enemy
{
    [Header("Shooting Enemy Stats")]
    public float shootingDistance;
    public float fireRate;
    public float spread;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    private Transform bulletParent;
    private bool shooting;
    private float lastShotTime;

    public override void Awake()
    {
        base.Awake();
        bulletParent = GameObject.Find("Bullets").transform;

        spread /= 100f;

        lastShotTime = Time.time;
        shooting = false;
    }

    public override void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Find the direction towards the target
            Vector2 direction = (target.position - transform.position).normalized;

            // Calculate the angle to the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation, -90 to assure the enemy is facing the right way
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

            if (!target.gameObject.activeSelf)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                target = players[Random.Range(0, players.Length)].transform;
            }

            if (distanceToTarget < shootingDistance || shooting)
            {
                shooting = true;
                Shooting();
            }

            // Check if the target exists
            if (!shooting)
            {
                // Move the enemy towards the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
    }

    void Shooting()
    {
        
        if (Time.time - lastShotTime >= 1 / fireRate)
        {
            Fire();
            lastShotTime = Time.time;
        }
    }

    void Fire()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletParent);
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
