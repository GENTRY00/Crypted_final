using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the character
    private Animator animator; // Reference to the Animator component
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store the movement direction

    void Start()
    {
        // Get the Animator and Rigidbody2D components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from WASD keys
        float horizontal = Input.GetAxis("Horizontal"); // A/D keys or Left/Right Arrow keys
        float vertical = Input.GetAxis("Vertical"); // W/S keys or Up/Down Arrow keys

        // Create a movement vector
        movement = new Vector2(horizontal, vertical).normalized;

        // Update animation parameters
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Move the character using Rigidbody2D
        MoveCharacter();
    }

    void MoveCharacter()
    {
        // Apply movement to the Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //// Rotate the character to face the movement direction (optional)
        //if (movement != Vector2.zero)
        //{
        //    float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        //    rb.rotation = angle;
        //}
    }

    void UpdateAnimations()
    {
        bool isMoving = movement.magnitude > 0;
        animator.SetBool("isWalking", isMoving);
    }
}
