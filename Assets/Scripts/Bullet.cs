using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;

    private void Awake()
    {
        Invoke(nameof(DestroySelf), 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet"))
        {
            DestroySelf();
            if(collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

}
