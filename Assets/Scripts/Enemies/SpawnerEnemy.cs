using UnityEngine;

public class SpawnerEnemy : Enemy
{
    [Header("Shooting Enemy Stats")]
    public float spawningDistance;
    public float spawnCooldown;
    public int spawnAmountMin;
    public int spawnAmountMax;
    public GameObject enemyPrefab;

    #region Private Variables

    private Transform enemyParent;
    private bool spawning;
    private float lastSpawnTime;

    #endregion

    public override void Awake()
    {
        base.Awake();
        enemyParent = GameObject.Find("Enemies").transform;

        lastSpawnTime = Time.time;
        spawning = false;
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

            if (distanceToTarget < spawningDistance || spawning)
            {
                spawning = true;
                Spawning();
            }

            // Check if the target exists
            if (!spawning)
            {
                // Move the enemy towards the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
    }

    void Spawning()
    {

        if (Time.time - lastSpawnTime >= spawnCooldown)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < Random.Range(spawnAmountMin, spawnAmountMax); i++)
        {
            // Generate a random angle in radians
            float angle = Random.Range(0f, Mathf.PI * 2);

            // Calculate the spawning position based on the angle and the fixed distance
            Vector3 spawnDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 1.5f; 
            Vector3 spawnPosition = transform.position + spawnDirection;

            // Instantiate the enemy at the calculated position
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemyParent);
        }
    }

}
