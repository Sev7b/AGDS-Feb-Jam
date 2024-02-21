using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    [HideInInspector] public float speed;

    public int health;

    public int damage;

    #region Private Variables

    private GameObject[] players;

    private Transform target;

    private Transform currentTarget;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        target = players[Random.Range(0, players.Length)].transform;

        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    virtual public void Update()
    {
        // Check if the target exists
        if (target != null)
        {
            if(!target.gameObject.activeSelf)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                target = players[Random.Range(0, players.Length)].transform;
            }

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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}