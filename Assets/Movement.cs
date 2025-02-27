using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Komponen CharacterController untuk mengatur pergerakan karakter
    public CharacterController controller;
    
    // Kecepatan gerak karakter
    public float speed = 12f;
    
    // Nilai gravitasi yang diterapkan ke karakter
    public float gravity = -20f;
    
    // Tinggi lompatan karakter
    public float jumpHeight = 5f;
    
    // Faktor percepatan gravitasi saat karakter jatuh
    public float fallMultiplier = 2.5f;
    
    // Jumlah maksimum lompatan yang dapat dilakukan (termasuk double jump)
    public int maxJumps = 2;
    
    // Objek transform yang digunakan untuk mendeteksi tanah
    public Transform groundCheck;
    
    // Jarak deteksi tanah untuk menentukan apakah karakter berada di atas tanah
    public float groundDistance = 0.1f;
    
    // Layer yang dianggap sebagai tanah
    public LayerMask groundMask;
    
    // Vektor kecepatan karakter, digunakan untuk mengatur pergerakan dan gravitasi
    private Vector3 velocity;
    
    // Status apakah karakter sedang menyentuh tanah atau tidak
    private bool isGrounded;
    
    // Menghitung jumlah lompatan yang telah dilakukan
    private int jumpCount = 0;
    
    // Komponen animator untuk mengontrol animasi karakter
    private Animator animasi;
    
    // Inisialisasi animator saat permainan dimulai
    void Start()
    {
        animasi = GetComponent<Animator>();
    }

    // Fungsi yang dipanggil setiap frame untuk memperbarui pergerakan karakter
    void Update()
    {
        // Menyimpan status sebelumnya apakah karakter menyentuh tanah
        bool wasGrounded = isGrounded;
        
        // Mengecek apakah karakter berada di atas tanah dengan menggunakan Physics.CheckSphere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Jika karakter baru saja mendarat, reset jumlah lompatan dan kecepatan vertikal
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
            velocity.y = -2f; // Mencegah karakter melayang setelah mendarat
        }

        // Mengambil input pergerakan dari pemain
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Menentukan arah pergerakan berdasarkan input pemain
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Menggerakkan karakter berdasarkan input dan kecepatan
        controller.Move(move * speed * Time.deltaTime);
        
        // Jika karakter bergerak, lakukan rotasi ke arah pergerakan
        if (move.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
        
        // Mengatur animasi berjalan berdasarkan status pergerakan
        bool isMoving = move.magnitude > 0.01f;
        animasi.SetBool("isRun", isMoving);
        
        // Memproses input untuk melompat, dengan batasan maksimal lompatan (double jump)
        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }
        
        // Mengaplikasikan gravitasi ke karakter
        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime; // Jatuh lebih cepat
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Jatuh normal
        }
        
        // Menggerakkan karakter berdasarkan kecepatan (termasuk efek gravitasi)
        controller.Move(velocity * Time.deltaTime);
    }
}
