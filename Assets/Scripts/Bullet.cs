using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject newBulletPrefab;

    public int playerSource = 0;
    [HideInInspector] public int damage;
    [HideInInspector] public float speed;
    public Vector2 direction; 

    void Start()
    {
        direction = GetComponent<Rigidbody2D>().velocity;
    }

    virtual public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && playerSource != 0)
        {
            Bullet otherBullet = collision.gameObject.GetComponent<Bullet>();
            if (otherBullet != null && newBulletPrefab != null)
            {
                Vector2 averageDirection = (direction.normalized + otherBullet.direction.normalized).normalized;
                if(playerSource == 1)
                    CreateNewBullet(averageDirection, speed * 2);

                Destroy(gameObject);
                Destroy(otherBullet.gameObject);
            }
        }
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().TakeDamage(damage, playerSource);
            }
            Destroy(gameObject);
        }
    }

    void CreateNewBullet(Vector2 direction, float newSpeed)
    {
        GameObject newBullet = Instantiate(newBulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.direction = direction;
        bulletComponent.speed = newSpeed;
        bulletComponent.playerSource = playerSource;
        bulletComponent.damage = damage * 2;

        // Explicitly set the new bullet's velocity to ensure it moves as intended.
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * newSpeed;
    }
}