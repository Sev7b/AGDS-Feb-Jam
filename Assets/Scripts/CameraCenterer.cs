using UnityEngine;

public class CameraCenterer : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Transform mergedPlayer;
    public float maxDistance = 10f; 

    // Update is called once per frame
    void Update()
    {
        if (player1 != null && player2 != null)
        {
            if (player1.gameObject.activeSelf && player1.gameObject.activeSelf)
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
            else
            {
                transform.position = mergedPlayer.position;
            }
        }
    }
}
