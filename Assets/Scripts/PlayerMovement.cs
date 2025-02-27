using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Public variables for character control
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    // Movement variables
    float speed = 12f;
    float gravity = -20f;
    float groundDistance = 0.4f;
    float jumpHeight = 3f;

    // Animation variables
    Animator animator;

    // Private variables
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset vertical velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input from the player
        float x = -Input.GetAxis("Horizontal");
        float z = -Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = new Vector3(x, 0, z);
        move = transform.TransformDirection(move);

        // Move the character
        controller.Move(move * speed * Time.deltaTime);

        // Handle running animation
        bool isRunning = move.magnitude > 0.1f; // Check if the character is moving
        animator.SetBool("IsRunning", isRunning);

        // Handle jumping
        if (Input.GetKeyDown("space") && isGrounded)
        {
            velocity.y = MathF.Sqrt(jumpHeight * -2 * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Update IsGrounded parameter
        animator.SetBool("IsGrounded", isGrounded);
    }
}