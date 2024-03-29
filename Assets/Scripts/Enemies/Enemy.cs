using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Stats")]
    public float minSpeed;
    public float maxSpeed;
    [HideInInspector] public float speed;

    public int health;

    public int damage;

    public float scoreOnKill;
    public float mergeChargeOnKill; // Merge cooldown decreased in seconds when killing this enemy

    [Header("GUI")]
    public GameObject deathEffect;

    #region Protected Variables

    protected GameObject[] players;

    protected Transform target;

    protected Transform currentTarget;

    protected PlayerMerge mergeManager;
    protected GameManager gameManager;

    protected bool turnedAround = false;

    #endregion

    // Start is called before the first frame update
    virtual public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mergeManager = GameObject.Find("MergeManager").GetComponent<PlayerMerge>();

        ResetPlayerArray();

        target = players[Random.Range(0, players.Length)].transform;

        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (target != null)
        {
            // Check if the target is active
            if (!target.gameObject.activeSelf)
            {
                ResetPlayerArray();
                target = players.Length > 0 ? players[Random.Range(0, players.Length)].transform : null;
            }

            FindClosestPlayer();

            if (target != null)
            {
                // Move the enemy towards the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                // Find the direction towards the target
                Vector2 direction = (target.position - transform.position).normalized;

                // Calculate the angle to the target
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Apply the rotation, -90 to assure the enemy is facing the right way
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            }
        }
        else
        {
            if (!turnedAround)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180);
                turnedAround = true;
            }

            transform.position += transform.up * speed * Time.deltaTime;
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


    public void TakeDamage(int damage, int playerSource)
    {
        health -= damage;
        if(health <= 0)
        {
            if (playerSource == 1)
                GameManager.player1Score += scoreOnKill;
            else if (playerSource == 2)
                GameManager.player2Score += scoreOnKill;
            else
            {
                GameManager.player1Score += scoreOnKill;
                GameManager.player2Score += scoreOnKill;
            }

            mergeManager.DecreaseTimer(mergeChargeOnKill);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    virtual public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player") || collision.transform.CompareTag("MergedPlayer"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void ResetPlayerArray()
    {
        List<GameObject> playerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject mergedPlayer = GameObject.FindGameObjectWithTag("MergedPlayer");

        if (mergedPlayer != null && mergedPlayer.activeSelf)
        {
            playerList.Add(mergedPlayer);
        }

        players = playerList.ToArray();

        if (players.Length > 0)
        {
            target = players[Random.Range(0, players.Length)].transform;
        }
        else
        {
            target = null;
        }
    }
}