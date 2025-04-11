using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion_Follow : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the companion
    public float stopDistance = 2.5f;  // Minimum distance to maintain from the player (adjustable)
    private Rigidbody2D rb;       // Reference to Rigidbody2D
    private Vector2 targetPosition; // Target position for the companion

    public GameObject targetPlayer; // The player the companion is following
    private bool isFollowing = true; // Whether the companion is currently following the player

    private LayerMask voidLayer; // Layer mask for the Void layer
    private LayerMask waterLayer; // Layer mask for the Void layer


    private bool isInVoid = false;  // Track if the player is in the Void layer
    private bool isInWater = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody2D
        if (targetPlayer != null)
        {
            // Set initial target position based on the first player
            targetPosition = new Vector2(targetPlayer.transform.position.x - 1, targetPlayer.transform.position.y);  // 1 unit behind
        }

        // Define the void layer (make sure the Void layer exists in your Unity project)
        voidLayer = LayerMask.GetMask("void");
        waterLayer = LayerMask.GetMask("water1");
    }

    void Update()
    {
        if (targetPlayer == null) return;

        // Check if the target player is in the Void layer
        bool playerInVoid = IsPlayerInVoidLayer();
        bool playerInWater = IsPlayerInWaterLayer();
        Debug.Log(isInVoid);

        // Update the current state of the player's Void status
        isInVoid = playerInVoid;
        isInWater = playerInWater;

        // Update the waypoint position to be behind the target player every frame if following
        if (!isInVoid && !isInWater)
        {
            UpdateWaypointPosition();
        }
    }

    void FixedUpdate()
    {
        if (targetPlayer == null || !isFollowing) return;

        // Move the companion smoothly towards the target position (waypoint)
        Vector2 currentPosition = rb.position;
        Vector2 newPosition = Vector2.Lerp(currentPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    // Update the waypoint position based on the target player's facing direction
    void UpdateWaypointPosition()
    {
        Vector2 facingDirection = GetPlayerFacingDirection();  // Get facing direction of the player

        if (facingDirection == Vector2.up)  // Facing upwards (towards the screen)
        {
            targetPosition = new Vector2(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 3);  // 1 unit behind
        }
        else if (facingDirection == Vector2.down)  // Facing downwards (away from the screen)
        {
            targetPosition = new Vector2(targetPlayer.transform.position.x, targetPlayer.transform.position.y - 3);  // 1 unit ahead
        }
        else if (facingDirection == Vector2.left)  // Facing left (sideways)
        {
            targetPosition = new Vector2(targetPlayer.transform.position.x - 3, targetPlayer.transform.position.y);  // 1 unit to the left
        }
        else if (facingDirection == Vector2.right)  // Facing right (sideways)
        {
            targetPosition = new Vector2(targetPlayer.transform.position.x + 3, targetPlayer.transform.position.y);  // 1 unit to the right
        }
    }

    // Get the facing direction based on the player's animation state
    Vector2 GetPlayerFacingDirection()
    {
        var activeAnimator = targetPlayer.GetComponent<AnimationDirection>().GetActiveAnimator();
        Debug.Log("Active Animator: " + activeAnimator.name);

        // Check if the active animator is front or side, and return the appropriate direction
        if (activeAnimator == targetPlayer.GetComponent<AnimationDirection>().frontAnimator)
        {
            return Vector2.up;  // Behind player (relative to front)
        }
        else if (activeAnimator == targetPlayer.GetComponent<AnimationDirection>().sideAnimator)
        {
            return isFacingRight() ? Vector2.right : Vector2.left; // Sideways movement
        }

        return Vector2.down; // Default fallback direction (downward)
    }

    // Helper function to check if the target player is facing right
    private bool isFacingRight()
    {
        return targetPlayer.transform.localScale.x > 0; // Check if facing right
    }

    // Set the target player for this companion to follow
    public void SetTarget(GameObject target)
    {
        targetPlayer = target;
    }

    // Function to check if the player is in the Void layer
    private bool IsPlayerInVoidLayer()
    {
        // Ensure the player exists and has a Collider2D component
        Collider2D playerCollider = targetPlayer.GetComponent<Collider2D>();
        if (playerCollider != null && targetPlayer.layer == LayerMask.NameToLayer("MothmanLayer"))
        {
            // Offset the ray slightly below the player's position to avoid hitting the player's own collider
            Vector2 rayOrigin = new Vector2(playerCollider.transform.position.x, playerCollider.transform.position.y - 0.5f);  // Small offset to avoid hitting the player's own collider

            // Cast a ray downwards frosm the offset position to check for Void layer beneath them
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f);

            // Debug: Draw the ray in the scene view (Adjust length if needed)
            Debug.DrawRay(rayOrigin, Vector2.down * 5, Color.green, 2f);

            // Check if the raycast hits something in the Void layer
            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
                if (((1 << hit.collider.gameObject.layer) & voidLayer) != 0)
                {
                    return true;  // Player is in the Void layer
                }
                else
                {
                    Debug.Log("Ray hit an object but it is not in the 'Void' layer. Hit: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
        else
        {
            Debug.LogWarning("Player is either not assigned or not in MothmanLayer.");
        }
        return false;  // Default: player is not in the Void layer
    }

    // Function to check if the player is in the Water layer
    private bool IsPlayerInWaterLayer()
    {
        // Ensure the player exists and has a Collider2D component
        Collider2D playerCollider = targetPlayer.GetComponent<Collider2D>();
        if (playerCollider != null && targetPlayer.layer == LayerMask.NameToLayer("NessieLayer"))
        {
            // Dynamic offset based on the player's collider bounds
            Vector2 rayOrigin = new Vector2(
                playerCollider.bounds.center.x,
                playerCollider.bounds.min.y - 0.5f);  // Small offset below the player

            // Cast a ray downwards from the offset position to check for the Water layer beneath them
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

            // Debug: Draw the ray in the scene view (Adjust length if needed)
            Debug.DrawRay(rayOrigin, Vector2.down * 5, Color.blue, 2f);

            // Check if the raycast hits something in the Water layer
            if (hit.collider != null)
            {
                // Debug the hit object layer
                Debug.Log("Ray hit: " + hit.collider.name + " | Layer: " + hit.collider.gameObject.layer);
                
                // Direct layer name comparison
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("water1"))
                {
                    return true;  // Player is in the Water layer
                }
                else
                {
                    Debug.Log("Ray hit an object but it is not in the 'Water' layer. Hit: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
        else
        {
            Debug.LogWarning("Player is either not assigned or not in NessieLayer.");
        }
        return false;  // Default: player is not in the Water layer
    }



    // Disable the companion follow behavior
    private void DisableCompanionFollow()
    {
        if (isFollowing)
        {
            isFollowing = false;
            this.enabled = false;  // Disable the Companion_Follow script
        }
    }

    // Enable the companion follow behavior
    private void EnableCompanionFollow()
    {
        if (!isFollowing)
        {
            isFollowing = true;
            this.enabled = true;  // Re-enable the Companion_Follow script
        }
    }
}
