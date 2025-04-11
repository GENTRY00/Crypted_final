using UnityEngine;
using System.Collections;

public class AnimationDirection : MonoBehaviour
{
    public GameObject characterParent; // The parent GameObject containing all sprites

    public GameObject frontSprite;
    public GameObject backSprite;
    public GameObject sideSprite;

    public Animator frontAnimator;
    public Animator backAnimator;
    public Animator sideAnimator;

    private int currentDirection = 0;
    private Vector3 lastPosition;
    private bool wasMoving = false;  // Track the previous movement state
    private Coroutine stopMovementCoroutine;
    private bool isFacingRight = true; // Track the current facing direction
    private Vector2 lastMovementDirection; // Track the last movement direction

    void Start()
    {
        // Initialize the last position as the current position
        lastPosition = transform.position;
    }

    void Update()
    {
        UpdateDirectionAndWalkingState();
    }

    void UpdateDirectionAndWalkingState()
    {
        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Calculate the movement vector
        Vector3 movement = currentPosition - lastPosition;

        // Set a small threshold to detect movement
        bool isMoving = movement.magnitude > 0.001f;

        // Update the last position for the next frame
        lastPosition = currentPosition;

        // Check if the state of movement has changed (from moving to idle, or idle to moving)
        if (isMoving != wasMoving)
        {
            wasMoving = isMoving; // Update the wasMoving flag

            if (isMoving)
            {
                // If the character starts moving, stop any existing coroutine
                if (stopMovementCoroutine != null)
                {
                    StopCoroutine(stopMovementCoroutine);
                    stopMovementCoroutine = null;
                }

                // Set the "isWalking" parameter to true
                GetActiveAnimator()?.SetBool("isWalking", true);
            }
            else
            {
                // If the character stops moving, start the coroutine to delay setting "isWalking" to false
                stopMovementCoroutine = StartCoroutine(DelayStopWalking());
            }
        }

        // Only update the direction if the character is moving
        if (isMoving)
        {
            float threshold = 0.01f; // Adjust this value to fine-tune sensitivity

            // Check if vertical movement dominates or is close to horizontal movement
            if (Mathf.Abs(movement.y) - Mathf.Abs(movement.x) > threshold)
            {
                if (movement.y > 0)
                {
                    UpdateDirection(1); // Back view
                }
                else
                {
                    UpdateDirection(0); // Front view
                }
            }
            else if (Mathf.Abs(movement.x) - Mathf.Abs(movement.y) > threshold)
            {
                UpdateDirection(2); // Side view
                FlipCharacter(movement.x); // Flip character based on movement direction
            }

            // Update the last movement direction
            lastMovementDirection = new Vector2(movement.x, movement.y);
        }
    }



    // Coroutine to delay setting "isWalking" to false
    private IEnumerator DelayStopWalking()
    {
        // Wait for a short period to ensure the character is completely still
        yield return new WaitForSeconds(0.1f);

        // Set the "isWalking" parameter to false
        GetActiveAnimator()?.SetBool("isWalking", false);
    }

    // Method to flip the character when moving right or left
    private void FlipCharacter(float movementX)
    {
        // Only flip the character if there is movement along the x-axis
        if (movementX != 0)
        {
            bool shouldFaceRight = movementX > 0;

            // Only flip if the direction has changed
            if (shouldFaceRight != isFacingRight)
            {
                isFacingRight = shouldFaceRight;
                Vector3 currentScale = characterParent.transform.localScale;
                currentScale.x = Mathf.Abs(currentScale.x) * (shouldFaceRight ? -1 : 1); // Flip the character
                characterParent.transform.localScale = currentScale;
                Debug.Log("Flipping character");
            }
        }
    }

    public void UpdateDirection(int direction)
    {
        if (currentDirection != direction)
        {
            currentDirection = direction;

            switch (direction)
            {
                case 0: // Front
                    SetActiveSprite(frontSprite);
                    ActivateAnimator(frontAnimator);
                    break;
                case 1: // Back
                    SetActiveSprite(backSprite);
                    ActivateAnimator(backAnimator);
                    break;
                case 2: // Side
                    SetActiveSprite(sideSprite);
                    ActivateAnimator(sideAnimator);
                    break;
                default:
                    Debug.LogWarning("Invalid direction: " + direction);
                    break;
            }
        }
    }

    private void SetActiveSprite(GameObject activeSprite)
    {
        Debug.Log(activeSprite.name);
        // Deactivate all sprites
        frontSprite.SetActive(false);
        backSprite.SetActive(false);
        sideSprite.SetActive(false);

        // Activate the desired sprite
        activeSprite.SetActive(true);
    }

    private void ActivateAnimator(Animator activeAnimator)
    {
        // Deactivate all animators
        frontAnimator.gameObject.SetActive(false);
        backAnimator.gameObject.SetActive(false);
        sideAnimator.gameObject.SetActive(false);

        // Activate the desired animator
        activeAnimator.gameObject.SetActive(true);
        activeAnimator.enabled = true;
    }

    public Animator GetActiveAnimator()
    {
        switch (currentDirection)
        {
            case 0: return frontAnimator;
            case 1: return backAnimator;
            case 2: return sideAnimator;
            default:
                Debug.LogWarning("No active animator for the current direction: " + currentDirection);
                return null;
        }
    }
}