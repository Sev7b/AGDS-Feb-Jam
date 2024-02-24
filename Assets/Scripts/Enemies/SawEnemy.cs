using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawEnemy : Enemy
{
    public override void Update()
    {
        if (target != null)
        {
            // Check if the target is active
            if (!target.gameObject.activeSelf)
            {
                target = players.Length > 0 ? players[Random.Range(0, players.Length)].transform : null;
            }

            if (target != null)
            {
                // Move the enemy towards the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
}
