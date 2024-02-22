using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isPlayer1s;
    [HideInInspector] public int damage;
    [HideInInspector] public float speed;

    virtual public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet"))
        {
            if(collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().TakeDamage(damage, isPlayer1s);
            }
            Destroy(gameObject);
        }
    }
}
