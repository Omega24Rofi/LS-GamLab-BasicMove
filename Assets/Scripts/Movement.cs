using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Inisiasi variabel
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -20f;
    public float jumpHeight = 5f;

    public float fallMultiplier = 2.5f; // Faktor percepatan gravitasi saat jatuh
    public int maxJumps = 2; // Maksimal double jump

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0; // Menghitung jumlah lompatan
    private Animator animasi;

    // Start is called before the first frame update
    void Start()
    {
        animasi = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset jump jika menyentuh tanah
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }

        // Get player input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the character
        controller.Move(move * speed * Time.deltaTime);

        // Rotasi karakter mengikuti arah gerakan
        if (move.magnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }

        // Animasi lari
        if (isGrounded)
        {
            bool isMoving = Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0;
            animasi.SetBool("isRun", isMoving);
        }

        // Handle jumping (maksimal double jump)
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
            
        }

        if(jumpCount == 2){
                jumpCount = 0;
            }

        // Apply gravity
        if (velocity.y < 0) 
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime; // Lebih cepat jatuh
        } 
        else 
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Apply movement
        controller.Move(velocity * Time.deltaTime);
    }
}
