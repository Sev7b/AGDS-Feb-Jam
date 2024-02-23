using UnityEngine;
using Cinemachine; // Make sure to include the Cinemachine namespace

public class CameraCenterer : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Transform mergedPlayer;
    public CinemachineVirtualCamera cinemachineCamera; // Assign in the inspector

    public float maxDistance = 15f;
    public float maxZoom = 10f; // Maximum orthographic size
    public float minZoom = 5f;  // Minimum orthographic size (optional, based on preference)
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
    }

    void AdjustCameraPosition()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 targetPosition = Vector3.zero;

            if (mergedPlayer != null && mergedPlayer.gameObject.activeSelf)            targetPosition = mergedPlayer.position;

            else if (player1.gameObject.activeSelf && player2.gameObject.activeSelf)   targetPosition = Vector3.Lerp(player1.position, player2.position, 0.5f);

            else if (!player1.gameObject.activeSelf)                                   targetPosition = player2.position;

            else if (!player2.gameObject.activeSelf)                                   targetPosition = player1.position;
             
            else                                                                       targetPosition = Vector3.Lerp(player1.position, player2.position, 0.5f);

            rb.position = targetPosition;
        }
    }

    void AdjustCameraZoom()
    {
        if (player1 != null && player2 != null && player1.gameObject.activeSelf && player2.gameObject.activeSelf)
        {
            float distance = Vector2.Distance(player1.position, player2.position);
            // Map the distance to the orthographic size within the specified min and max bounds
            if (distance > maxDistance)
            {
                float targetOrthoSize = Mathf.Clamp(distance, minZoom, maxZoom);

                // Smoothly transition to the target orthographic size
                cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, targetOrthoSize, zoomSpeed * Time.deltaTime);
            }
            else
            {
                cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, minZoom, zoomSpeed * Time.deltaTime);
            }
        }
        else
        {
            cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, minZoom, zoomSpeed * Time.deltaTime);
        }
    }
}
