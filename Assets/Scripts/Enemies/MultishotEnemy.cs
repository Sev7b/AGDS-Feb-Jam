using UnityEngine;

public class MultishotEnemy : Enemy
{
    [Header("Multishot Enemy Stats")]
    public float shootingDistance;
    public float fireRate;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    private Transform bulletParent;
    private float lastShotTime = -Mathf.Infinity;

    public override void Awake()
    {
        base.Awake();
        bulletParent = GameObject.Find("Bullets").transform;

        lastShotTime = Time.time;
    }

    public override void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!target.gameObject.activeSelf)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                if (players.Length > 0)
                    target = players[Random.Range(0, players.Length - 1)].transform;
                else
                    target = null;
            }

            Shooting();

            // Check if the target exists
            if (distanceToTarget > shootingDistance && target != null)
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
        int numberOfBullets = 6;
        float angleStep = 360f / numberOfBullets; // Divide 360 by the number of bullets to get equal spacing
        float startingAngle = 0f; // Start shooting directly upwards

        for (int i = 0; i < numberOfBullets; i++)
        {
            // Calculate the bullet's direction with the offset and additional rotation of the game object
            float currentAngle = startingAngle - (i * angleStep) + 25; 
            float currentAngleInRad = currentAngle * Mathf.Deg2Rad;

            // Adjust angle based on the game object's rotation
            float adjustedAngleInRad = currentAngleInRad + transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

            // Calculate the spawn position, adjusted for the game object's rotation
            Vector3 spawnDirection = new Vector3(Mathf.Cos(adjustedAngleInRad), Mathf.Sin(adjustedAngleInRad), 0);
            Vector3 spawnPosition = transform.position + spawnDirection;

            // Calculate the final direction for the bullet, adjusted for rotation
            Vector2 direction = new Vector2(spawnDirection.x, spawnDirection.y);

            // Instantiate the bullet at this position, facing the direction it will travel
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0f, 0f, currentAngle + transform.eulerAngles.z), bulletParent);

            // Set bullet's properties and direction
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.speed = bulletSpeed;
            bulletScript.damage = damage;

            bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
        }
    }
}
