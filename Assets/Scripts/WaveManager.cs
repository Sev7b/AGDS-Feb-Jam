using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public float waveDelay; // Time in seconds until next wave
        public float enemyRate; // Time between enemy spawns
        public int amountOfEnemies;
        public GameObject[] enemyTypes;
    }

    public List<Wave> waves;
    public Transform spawnPoint; // Assign a spawn point in the inspector

    private int waveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        foreach (Wave wave in waves)
        {
            yield return new WaitForSeconds(wave.waveDelay);

            for (int i = 0; i < wave.amountOfEnemies; i++)
            {
                SpawnEnemy(wave);
                yield return new WaitForSeconds(wave.enemyRate);
            }

            while (GameObject.FindWithTag("Enemy") != null)
            {
                yield return null;
            }

            waveNumber++;
        }

        OnAllWavesCompleted();
    }

    void SpawnEnemy(Wave wave)
    {
        if (wave.enemyTypes.Length == 0) return;

        Vector2 spawnPosition = CalculateSpawnPosition();

        // Check if the spawn position is too close to another enemy, retry if necessary
        while (IsPositionNearEnemy(spawnPosition))
        {
            spawnPosition = CalculateSpawnPosition();
        }

        GameObject enemyToSpawn = wave.enemyTypes[UnityEngine.Random.Range(0, wave.enemyTypes.Length)];
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
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
        Debug.Log("All waves completed!");
    }
}
