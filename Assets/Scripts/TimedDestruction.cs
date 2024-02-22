using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestruction : MonoBehaviour
{
    public float delay;

    private void Awake()
    {
        Invoke(nameof(DestroySelf), delay);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
