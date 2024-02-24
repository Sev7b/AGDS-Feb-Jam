using UnityEngine;
using Cinemachine; // Make sure to include the Cinemachine namespace

public class CameraCenterer : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Transform mergedPlayer;
    public CinemachineVirtualCamera cinemachineCamera;

    public float maxZoom = 10f; // Maximum orthographic size
    public float minZoom = 5f;  // Minimum orthographic size
    public float zoomSpeed = 1f; // Speed at which the camera zooms in/out

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        AdjustCameraPosition();
        AdjustCameraZoom();
        EnsurePlayerDistance();
    }

    void AdjustCameraPosition()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 targetPosition;

            if (mergedPlayer.gameObject.activeSelf)                                    targetPosition = mergedPlayer.position;

            else if (player1.gameObject.activeSelf && player2.gameObject.activeSelf)   targetPosition = Vector3.Lerp(player1.position, player2.position, 0.5f);

            else if (!player1.gameObject.activeSelf)                                   targetPosition = player2.position;

            else if (!player2.gameObject.activeSelf)                                   targetPosition = player1.position;
             
            else                                                                       targetPosition = Vector3.Lerp(player1.position, player2.position, 0.5f);

            rb.position = targetPosition;
        }
    }

    void AdjustCameraZoom()
    {
        if (player1 != null && player2 != null)
        {
            float targetOrthoSize = minZoom;
            if (player1.gameObject.activeSelf && player2.gameObject.activeSelf)
            {
                // Calculate the midpoint between the players
                Vector3 midpoint = (player1.position + player2.position) / 2;
                // Calculate the distance between the players
                float distance = Vector3.Distance(player1.position, player2.position);
                // Adjust distance based on the camera's aspect ratio to ensure both players are visible
                float cameraWidth = distance * Screen.width / Screen.height;
                // Determine target orthographic size based on player separation and camera aspect ratio
                targetOrthoSize = Mathf.Max(cameraWidth / 4, distance / 4, minZoom);
            }
            // Ensure the target orthographic size is within the specified bounds
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, minZoom, maxZoom);
            // Smoothly transition to the target orthographic size
            cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, targetOrthoSize, zoomSpeed * Time.deltaTime);
        }
        else
        {
            // Default to minimum zoom if one or both players are null
            cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, minZoom, zoomSpeed * Time.deltaTime);
        }
    }
    void EnsurePlayerDistance()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 midpoint = (player1.position + player2.position) / 2;
            Vector3 separationVector = player2.position - player1.position;
            float currentDistance = separationVector.magnitude;

            if (currentDistance > 40f)
            {
                Vector3 direction = separationVector.normalized;
                // Clamp positions based on midpoint and max distance
                player1.position = midpoint - direction * 40f / 2;
                player2.position = midpoint + direction * 40f / 2;
            }
        }
    }
}
