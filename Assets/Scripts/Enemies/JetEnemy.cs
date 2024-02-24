using UnityEngine;

public class JetEnemy : Enemy
{
    [Header("Jet Enemy Stats")]
    public float turnSpeed;
    public float fireRate;
    public float spread;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    private Transform bulletParent;
    private float lastShotTime;

    public override void Awake()
    {
        base.Awake();
        bulletParent = GameObject.Find("Bullets").transform;

        spread /= 100f;

        lastShotTime = Time.time;
    }

    public override void Update()
    {
        if (target != null)
        {
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

            // Rotate towards the target over time
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Move forwards in the current facing direction
            transform.position += transform.up * speed * Time.deltaTime;

            FindClosestPlayer();

            Shooting();
        }
    }

    void FindClosestPlayer()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        target = closestPlayer;
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

    public override void OnCollisionEnter2D(Collision2D collision) {}
}
