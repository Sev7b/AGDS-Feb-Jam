using UnityEngine;

public class MoveToMidpointOrPlayer1 : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float maxDistance = 10f; 

    // Update is called once per frame
    void Update()
    {
        if (player1 != null && player2 != null)
        {
            float distance = Vector2.Distance(player1.position, player2.position);

            if (distance > maxDistance)
            {
                transform.position = player1.position;
            }
            else
            {
                Vector2 midpoint = Vector2.Lerp(player1.position, player2.position, 0.5f);
                transform.position = midpoint;
            }
        }
    }
}
