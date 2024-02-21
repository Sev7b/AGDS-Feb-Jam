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
    public float cooldown; // Cooldown duration in seconds
    public float mergeLength; // Duration of the merge state

    [Header("UI Elements")]
    public Slider mergeSlider; // Assign this in the inspector
    public Color cooldownColor; // Color when in cooldown
    public Color mergeColor; // Color during merge
    public Color readyColor; // Color when merge is ready

    [HideInInspector] public bool isMerged = false;

    private float lastMergeTime = -Mathf.Infinity; // Initialize to a large negative to start on cooldown
    private float timeSinceLastMerge;

    private void Start()
    {
        if (mergeSlider != null)
        {
            mergeSlider.maxValue = cooldown;
            mergeSlider.fillRect.GetComponentInChildren<Image>().color = cooldownColor;
        }

        // Set the initial slider state to reflect the cooldown status at game start
        ResetCooldown();
    }

    private void Update()
    {
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
        yield return new WaitForSeconds(mergeLength);

        if (isMerged)
        {
            UnmergePlayers();
        }
    }

    void UnmergePlayers()
    {
        if (mergedPlayer.activeSelf)
        {
            player1.SetActive(true);
            player2.SetActive(true);

            player1.transform.position = mergedPlayer.transform.position + Vector3.left;
            player2.transform.position = mergedPlayer.transform.position + Vector3.right;

            mergedPlayer.SetActive(false);

            isMerged = false;
            ResetCooldown();
        }
    }

    void UpdateSlider()
    {
        if (mergeSlider != null)
        {
            timeSinceLastMerge = Time.time - lastMergeTime;
            if (isMerged)
            {
                mergeSlider.maxValue = mergeLength;
                mergeSlider.value = mergeLength - timeSinceLastMerge;
                mergeSlider.fillRect.GetComponentInChildren<Image>().color = mergeColor;
            }
            else if (timeSinceLastMerge < cooldown)
            {
                mergeSlider.maxValue = cooldown;
                mergeSlider.value = timeSinceLastMerge;
                mergeSlider.fillRect.GetComponentInChildren<Image>().color = cooldownColor;
            }
            else
            {
                mergeSlider.value = cooldown;
                mergeSlider.fillRect.GetComponentInChildren<Image>().color = readyColor;
            }
        }
    }

    public void DecreaseTimer(float amount)
    {
        if (!isMerged)
        {
            // Decrease the last merge time to reduce the cooldown
            lastMergeTime -= amount;

            // Ensure we don't go beyond the current time, making it immediately available
            lastMergeTime = Mathf.Max(lastMergeTime, Time.time - cooldown);

            // Update the UI elements if necessary
            UpdateSlider();
        }
    }

    private void ResetCooldown()
    {
        // To start on cooldown, simulate that the last merge just happened
        lastMergeTime = Time.time;
        if (mergeSlider != null)
        {
            mergeSlider.value = 0; // Start with the slider empty or at the beginning of the cooldown
        }
    }
}