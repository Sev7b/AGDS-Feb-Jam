using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    public GameObject deathEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }    
}
