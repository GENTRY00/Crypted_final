using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of the current waypoint from which Enemy walks to the next one
    private int waypointIndex = 0;

    // Direction to move
    private Vector3 direction;

    // Reference to the visual cone
    public Transform coneView;

    // Grace period duration (in seconds)
    [SerializeField]
    private float gracePeriodDuration = 0.1f;

    // Timer to track grace period
    private float graceTimer = 0f;

    // Use this for initialization
    private void Start()
    {
        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // Decrease the grace timer if it's active
        if (graceTimer > 0)
        {
            graceTimer -= Time.deltaTime;
        }

        // Move Enemy
        Move();
    }

    // Method that actually makes Enemy walk
    private void Move()
    {
        if (waypointIndex < waypoints.Length)
        {
            // Target position and direction
            Vector3 targetPosition = waypoints[waypointIndex].transform.position;
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Move the Zookeeper
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Rotate ConeView
            if (coneView != null)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

                // Normalize the angle to be between 0 and 360
                angle = (angle + 360) % 360;

                coneView.localRotation = Quaternion.Euler(0, 0, angle);
            }

            // Move to the next waypoint
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Reset grace period when changing waypoints
                graceTimer = gracePeriodDuration;

                transform.position = targetPosition; // Snap to the exact position
                waypointIndex++;
            }
        }
        else
        {
            waypointIndex = 0; // Loop back to the first waypoint
        }
    }

    // Public method to check if the Zookeeper can detect the player
    public bool CanDetectPlayer()
    {
        // Return true only if the grace timer is inactive
        return graceTimer <= 0;
    }
}
