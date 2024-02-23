using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int spawnAmount;
    }

    [Serializable]
    public struct Wave
    {
        public float waveDelay; // Time in seconds until next wave
        public float enemyRate; // Time between enemy spawns
        public List<EnemySpawnInfo> enemySpawns; // New field replacing amountOfEnemies and enemyTypes
    }


    public Transform enemyParent;

    public List<Wave> waves;
    public Transform spawnPoint; // Assign a spawn point in the inspector

    public TextMeshProUGUI waveNumberText;
    private int waveNumber = 0;

    // Post game difficulty scaling variables
    public GameObject[] enemyPrefabs;
    public float difficultyMultiplier = 1.1f;
    public int baseEnemyCount = 5;
    public float baseEnemyRate = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        foreach (Wave wave in waves)
        {
            waveNumberText.text = "WAVE " + (waveNumber+1);
            waveNumberText.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            waveNumberText.transform.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(wave.waveDelay - 3f);

            foreach (EnemySpawnInfo enemySpawn in wave.enemySpawns)
            {
                for (int i = 0; i < enemySpawn.spawnAmount; i++)
                {
                    SpawnEnemy(enemySpawn.enemyPrefab);
                    yield return new WaitForSeconds(wave.enemyRate);
                }
            }

            while (GameObject.FindWithTag("Enemy") != null)
            {
                yield return null;
            }

            waveNumber++;
        }

        OnAllWavesCompleted();
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = CalculateSpawnPosition();

        // Check if the spawn position is too close to another enemy, retry if necessary
        while (IsPositionNearEnemy(spawnPosition))
        {
            spawnPosition = CalculateSpawnPosition();
        }

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemyParent);
    }


    Vector2 CalculateSpawnPosition()
    {
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(angle) * 25f, Mathf.Sin(angle) * 25f);
        return (Vector2)transform.position + offset;
    }

    // Check if the given position is within 10 units of any enemy
    bool IsPositionNearEnemy(Vector2 position)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    void OnAllWavesCompleted()
    {
        Debug.Log("All predefined waves completed! Starting random waves with increasing difficulty.");
        StartCoroutine(GenerateRandomWaves());
    }

    IEnumerator GenerateRandomWaves()
    {
        while (true) // Keep generating waves indefinitely
        {
            Wave generatedWave = GenerateRandomWave();
            yield return new WaitForSeconds(generatedWave.waveDelay);

            foreach (EnemySpawnInfo enemySpawn in generatedWave.enemySpawns)
            {
                for (int i = 0; i < enemySpawn.spawnAmount; i++)
                {
                    SpawnEnemy(enemySpawn.enemyPrefab);
                    yield return new WaitForSeconds(generatedWave.enemyRate);
                }
            }

            while (GameObject.FindWithTag("Enemy") != null)
            {
                yield return null;
            }

            // Increase difficulty for the next wave
            baseEnemyCount = Mathf.CeilToInt(baseEnemyCount * difficultyMultiplier);
            baseEnemyRate *= 0.95f; // Decrease spawn time to increase difficulty
            waveNumber++;
        }
    }

    Wave GenerateRandomWave()
    {
        // Generate a new wave based on the current difficulty level
        Wave newWave = new Wave
        {
            waveDelay = UnityEngine.Random.Range(3f, 5f), 
            enemyRate = Mathf.Max(baseEnemyRate, 0.5f), 
            enemySpawns = new List<EnemySpawnInfo>()
        };

        for (int i = 0; i < baseEnemyCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
            newWave.enemySpawns.Add(new EnemySpawnInfo { enemyPrefab = enemyPrefab, spawnAmount = 1 });
        }

        return newWave;
    }
}
