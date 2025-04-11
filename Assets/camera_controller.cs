using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform target;            // Reference to the target (e.g., player)
    public float snapThreshold = 0.1f;  // Threshold distance for snapping
    public float transitionSpeed = 5f;  // Speed at which the camera transitions between targets

    public Tilemap tilemap;             // Reference to your Tilemap
    public Camera mainCamera;           // Reference to the main camera

    private bool isTransitioning = false; // Flag to track transition state

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found. Please assign a camera to CameraController.");
                return;
            }
        }

        if (tilemap == null)
        {
            Debug.LogError("Tilemap not assigned in CameraController.");
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target not assigned in CameraController.");
            return;
        }

        // Ensure the camera starts correctly positioned
        transform.position = ClampToTilemapBounds(target.position);
    }

    private void LateUpdate()
    {
        if (target == null || isTransitioning) return;

        // Smoothly move the camera towards the target position
        Vector3 targetPos = target.position;
        targetPos = ClampToTilemapBounds(targetPos);

        if (Vector3.Distance(transform.position, targetPos) > snapThreshold)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * transitionSpeed);
        }
        else
        {
            transform.position = targetPos; // Snap directly if very close
        }
    }

    private Vector3 ClampToTilemapBounds(Vector3 targetPos)
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3 bottomLeft = tilemap.GetCellCenterWorld(new Vector3Int(bounds.xMin, bounds.yMin, 0));
        Vector3 topRight = tilemap.GetCellCenterWorld(new Vector3Int(bounds.xMax - 1, bounds.yMax - 1, 0));

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        targetPos.x = Mathf.Clamp(targetPos.x, bottomLeft.x + cameraWidth, topRight.x - cameraWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, bottomLeft.y + cameraHeight, topRight.y - cameraHeight);

        // Ensure the camera stays at the correct Z-level
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public void SetTarget(Transform newTarget)
    {
        if (isTransitioning)
        {
            Debug.Log("Already transitioning camera. Cannot set new target yet.");
            return; // Prevent setting new target if already transitioning
        }

        if (newTarget == null)
        {
            Debug.LogWarning("New target is null in CameraController.SetTarget.");
            return;
        }

        StartCoroutine(SmoothTransition(newTarget));
    }

    private IEnumerator SmoothTransition(Transform newTarget)
    {
        isTransitioning = true;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = ClampToTilemapBounds(newTarget.position);

        float journeyLength = Vector3.Distance(initialPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * transitionSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(initialPosition, targetPosition, fractionOfJourney);

            // Clamp the position during the smooth transition
            transform.position = ClampToTilemapBounds(transform.position);

            yield return null;
        }

        // Snap to final position and update target
        transform.position = targetPosition;
        target = newTarget;

        isTransitioning = false; // End the transition
    }
}
