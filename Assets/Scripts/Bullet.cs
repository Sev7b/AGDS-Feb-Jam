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

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
