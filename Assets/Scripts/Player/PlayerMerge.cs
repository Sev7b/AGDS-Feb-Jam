using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMerge : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject player1;
    public GameObject player2;
    public GameObject mergedPlayer;

    [Header("Stats")]
    public float mergeDistance; // Distance within which players can merge
    public float cooldown;
    public float mergeLength; // Cooldown duration in seconds

    [Header("UI Elements")]
    public Slider mergeSlider; // Assign this in the inspector
    public Color cooldownColor; // Color when in cooldown
    public Color mergeColor; // Color during merge
    public Color readyColor; // Color when merge is ready

    [HideInInspector] public bool isMerged = false;

    #region Private Variables

    private float lastMergeTime = 0f;

    #endregion

    private void Start()
    {
        lastMergeTime = Time.time;

        if (mergeSlider != null)
        {
            mergeSlider.maxValue = cooldown;
            mergeSlider.value = 0;

            mergeSlider.fillRect.GetComponentInChildren<Image>().color = cooldownColor;
        }
    }


    void Update()
    {
        // Check if Q is pressed and if the cooldown has elapsed
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= lastMergeTime + cooldown && !isMerged)
        {
            TryMergePlayers();
        }

        UpdateSlider();
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

            StartCoroutine(UnmergeAfterDelay());
        }
    }

    IEnumerator UnmergeAfterDelay()
    {
        // Wait for mergeLength seconds
        yield return new WaitForSeconds(mergeLength);

        // Call UnmergePlayers if still merged
        if (isMerged)
        {
            UnmergePlayers();
        }
    }

    void UnmergePlayers()
    {
        // Ensure there's a merged player instance to unmerge
        if (mergedPlayer.activeSelf)
        {
            player1.SetActive(true);
            player2.SetActive(true);

            player1.transform.position = mergedPlayer.transform.position + Vector3.left;
            player2.transform.position = mergedPlayer.transform.position + Vector3.right; 

            mergedPlayer.SetActive(false);

            isMerged = false;
            lastMergeTime = Time.time;
        }
    }

    void UpdateSlider()
    {
        if (mergeSlider != null)
        {
            float timeSinceLastMerge = Time.time - lastMergeTime;
            if (isMerged)
            {
                // During merge, show the remaining merge duration
                mergeSlider.maxValue = mergeLength;
                mergeSlider.value = mergeLength - timeSinceLastMerge;
                mergeSlider.fillRect.GetComponentInChildren<Image>().color = mergeColor;
            }
            else if (timeSinceLastMerge < cooldown)
            {
                // Show the cooldown progress
                mergeSlider.maxValue = cooldown;
                mergeSlider.value = timeSinceLastMerge;
                mergeSlider.fillRect.GetComponentInChildren<Image>().color = cooldownColor;
            }
            else
            {
                // When ready to merge again
                mergeSlider.value = cooldown; // Keep the slider full to indicate readiness
                mergeSlider.fillRect.GetComponentInChildren<Image>().color = readyColor;
            }
        }
    }
}