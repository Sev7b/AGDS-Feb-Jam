using System.Collections;
using UnityEngine;

public class PlayerMerge : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject player1;
    public GameObject player2;
    public GameObject mergedPlayer;
    [Header("Stats")]
    public float mergeDistance = 2f; // Distance within which players can merge
    public float cooldown = 5f; // Cooldown duration in seconds

    #region Private Variables

    private bool isMerged = false;
    private float lastMergeTime = -5f;

    #endregion

    void Update()
    {
        // Check if Q is pressed and if the cooldown has elapsed
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= lastMergeTime + cooldown)
        {
            if (!isMerged)
            {
                // Attempt to merge
                TryMergePlayers();
            }
            else
            {
                // Attempt to unmerge
                UnmergePlayers();
            }
        }
    }

    void TryMergePlayers()
    {
        float distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        if (distance <= mergeDistance)
        {
            mergedPlayer.SetActive(true);
            mergedPlayer.transform.position = (player1.transform.position + player2.transform.position) / 2f;

            player1.SetActive(false);
            player2.SetActive(false);

            isMerged = true;
            lastMergeTime = Time.time;
        }
    }

    void UnmergePlayers()
    {
        // Ensure there's a merged player instance to unmerge
        if (mergedPlayer.activeSelf)
        {
            // Instantiate the player1 and player2 prefabs at desired positions
            player1.SetActive(true);
            player2.SetActive(true);
            player1.transform.position = mergedPlayer.transform.position + Vector3.left;
            player2.transform.position = mergedPlayer.transform.position + Vector3.right;

            mergedPlayer.SetActive(false);

            isMerged = false;
            lastMergeTime = Time.time;
        }
    }
}
