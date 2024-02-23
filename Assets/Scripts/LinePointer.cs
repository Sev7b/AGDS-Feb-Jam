using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LinePointer : MonoBehaviour
{
    public Transform target;
    public float distance;
    public Vector2 cornerOffsetPercentage; // Use percentage instead of fixed offset

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || lineRenderer == null) return;

        Camera cam = Camera.main;

        // Calculate dynamic corner offset based on screen size and percentage
        Vector2 dynamicCornerOffset = new Vector2(Screen.width * cornerOffsetPercentage.x / 100, Screen.height * cornerOffsetPercentage.y / 100);

        // Adjust the screen's top right corner by the dynamic offset
        Vector3 screenTopRight = new Vector3(Screen.width - dynamicCornerOffset.x, Screen.height - dynamicCornerOffset.y, 0);

        // Convert screen position to world position with a z value that represents distance from the camera
        float distanceToCamera = 10.0f; // Adjust this value based on your scene and camera setup
        Vector3 worldTopRight = cam.ScreenToWorldPoint(new Vector3(screenTopRight.x, screenTopRight.y, cam.nearClipPlane + distanceToCamera));

        Vector3 targetPosition = target.position;
        Vector3 direction = (targetPosition - worldTopRight).normalized;

        // Calculate the end position that is distance units away from the target towards the worldTopRight
        Vector3 endPosition = targetPosition - direction * distance;

        // Set the positions for the line renderer
        lineRenderer.SetPosition(0, worldTopRight); // First point with offset from the top right of the screen
        lineRenderer.SetPosition(1, endPosition); // Last point distance units away from the target
    }
}
