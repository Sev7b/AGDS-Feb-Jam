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
                // Wait for all enemies to be destroyed before continuing to the next wave, can change how this works
                yield return null;
            }

            waveNumber++; // Increment the wave number after all enemies are destroyed
        }

        OnAllWavesCompleted();
    }

    void SpawnEnemy(Wave wave)
    {
        if (wave.enemyTypes.Length == 0) return;

        // Select a random enemy type to spawn
        GameObject enemyToSpawn = wave.enemyTypes[UnityEngine.Random.Range(0, wave.enemyTypes.Length)];

        // Generate a random position that's 20 to 30 units away from this GameObject
        Vector2 spawnPosition = CalculateSpawnPosition();

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }

    Vector2 CalculateSpawnPosition()
    {
        // Generate a random direction
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad; 

        // Generate a random distance within the specified range
        float distance = UnityEngine.Random.Range(20f, 25f);

        // Calculate offset from the reference point
        Vector2 offset = new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);

        // Calculate and return the spawn position
        return (Vector2)transform.position + offset;
    }

    void OnAllWavesCompleted()
    {
        Debug.Log("All waves completed!");
    }
}
