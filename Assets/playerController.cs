using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Normal speed of the player
    public float sprintMultiplier = 2f; // Multiplier when shifting
    private Rigidbody2D rb;      // Reference to the Rigidbody2D component
    private Vector2 movement;    // Store movement direction
    private bool canMove = true;
    public bool has_key = false;
    public DialogueManager dialogueManager; // Reference to the DialogueManager
    private bool hasTriggeredDialogue = false; // Flag to track if dialogue has been triggered
    private SwitchController switchController;  // Reference to SwitchController

    void Start()
    {
        switchController = FindObjectOfType<SwitchController>();  // Find the SwitchController in the scene

        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Start the AutoMoveLeft coroutine
        StartCoroutine(AutoMoveLeft());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Switch player when Q is pressed
            if (switchController != null)
            {
                if (switchController.players[switchController.currentPlayerIndex] && !IsPlayerInVoidLayer() && !IsPlayerInWaterLayer())
                    switchController.SwitchPlayers();
            }
            else
            {
                Debug.LogWarning("SwitchController not found in the scene.");
            }
        }

        if (canMove)
        {
            // Capture input for movement
            movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
            movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrow

            // Speed boost when Shift is held down
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = 5f * sprintMultiplier; // Sprint speed with multiplier
            }
            else
            {
                moveSpeed = 5f; // Default walking speed
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // Move the player using Rigidbody2D
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Function to check if the player is in the Void layer
    private bool IsPlayerInVoidLayer()
    {
        // Ensure the player exists and has a Collider2D component
        Collider2D playerCollider = switchController.players[switchController.currentPlayerIndex].GetComponent<Collider2D>();
        if (playerCollider != null && switchController.players[switchController.currentPlayerIndex].layer == LayerMask.NameToLayer("MothmanLayer"))
        {
            // Offset the ray slightly below the player's position to avoid hitting the player's collider
            Vector2 rayOrigin = new Vector2(playerCollider.transform.position.x, playerCollider.transform.position.y - 1f);  // Small offset to avoid hitting the player's own collider

            // Cast a ray downwards from the offset position to check for Void layer beneath them
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

            // Debug: Draw the ray in the scene view
            //Debug.DrawRay(rayOrigin, Vector2.down * 10, Color.green, 2f); // Change the multiplier if needed

            // Check if the raycast hits something in the Void layer
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("void"))  // Check against "Void" layer
                {
                    return true;  // Player is in the Void layer
                }
                else
                {
                    Debug.Log("Ray hit an object but it is not in the 'void' layer. Hit: " + hit.collider.gameObject.name);
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

    private bool IsPlayerInWaterLayer()
    {
        // Ensure the player exists and has a Collider2D component
        Collider2D playerCollider = switchController.players[switchController.currentPlayerIndex].GetComponent<Collider2D>();
        if (playerCollider != null && switchController.players[switchController.currentPlayerIndex].layer == LayerMask.NameToLayer("NessieLayer"))
        {
            // Dynamic offset based on the player's collider bounds
            Vector2 rayOrigin = new Vector2(
                playerCollider.bounds.center.x,
                playerCollider.bounds.min.y - 0.5f);  // Small offset below the player

            // Cast a ray downwards from the offset position to check for the Water layer beneath them
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

            // Debug: Draw the ray in the scene view (Adjust length if needed)
            //Debug.DrawRay(rayOrigin, Vector2.down * 5, Color.blue, 2f);

            // Check if the raycast hits something in the Water layer
            if (hit.collider != null)
            {
                // Debug the hit object layer

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

    public void SetCanMove(bool state)
    {
        canMove = state;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Void" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("void"))
        {
            if (!hasTriggeredDialogue)
            {
                // Display dialogue for Void trigger
                if (dialogueManager != null)
                {
                    string[] voidDialogue = {
                        "Woah, careful! There's no telling how far down that hole goes...",
                        "If only you could fly!"
                    };
                    dialogueManager.StartDialogue(voidDialogue);
                }
                hasTriggeredDialogue = true;
            }
        }
    }

    IEnumerator AutoMoveLeft()
    {
        Debug.Log("AutoMoveLeft Coroutine Started"); // This should be printed when the coroutine starts

        float moveDuration = 0.05f; // Duration of movement to the left
        float elapsedTime = 0f;

        canMove = false;

        // Move to the left automatically
        while (elapsedTime < moveDuration)
        {
            rb.MovePosition(rb.position + Vector2.left * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // After the move is complete, stop movement and reset to default state if needed
        movement = Vector2.zero; // Optionally stop movement after auto-move
        Debug.Log("AutoMoveLeft Coroutine Finished"); // Confirm coroutine finishes
        canMove = true;
    }
}
