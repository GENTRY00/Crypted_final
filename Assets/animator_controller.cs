using UnityEngine;

public class BigfootAnimationController : MonoBehaviour
{
    private Animator animator;
    public RuntimeAnimatorController bigfootFrontController; // Assign in Inspector
    public RuntimeAnimatorController bigfootBackController;  // Assign in Inspector

    private bool isFacingFront = true; // Track which direction is facing

    // References to the sprite renderers
    public GameObject frontSprite; // Assign Bigfoot_Front GameObject
    public GameObject backSprite;  // Assign Bigfoot_Back GameObject

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = bigfootFrontController; // Set default to front
        SetVisibility(true); // Start with front visible
    }

    void Update()
    {
        // Movement input to switch animation based on direction
        if (Input.GetKeyDown(KeyCode.W)) // Move forward
        {
            Move();
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Move backward
        {
            MoveBack();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) // Toggle facing direction
        {
            ToggleFacingDirection();
        }

        // Check walking state
        animator.SetBool("isWalking", Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S));
    }

    void Move()
    {
        // Move the character forward
        if (!isFacingFront)
        {
            ToggleFacingDirection(); // Switch to front if currently back-facing
        }
    }

    void MoveBack()
    {
        // Move the character backward
        if (isFacingFront)
        {
            ToggleFacingDirection(); // Switch to back if currently front-facing
        }
    }

    void ToggleFacingDirection()
    {
        isFacingFront = !isFacingFront;

        if (isFacingFront)
        {
            animator.runtimeAnimatorController = bigfootFrontController;
            animator.Play("Idle_Front"); // Ensure Idle is set for front
            SetVisibility(true); // Show front sprite
        }
        else
        {
            animator.runtimeAnimatorController = bigfootBackController;
            animator.Play("Idle_Back"); // Ensure Idle is set for back
            SetVisibility(false); // Show back sprite
        }
    }

    void SetVisibility(bool isFrontVisible)
    {
        frontSprite.SetActive(isFrontVisible); // Enable or disable front sprite
        backSprite.SetActive(!isFrontVisible); // Enable or disable back sprite
    }
}
