using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables
    float speed;        // determines move speed
    float horizontalInput; // determines direction of movement

    float baseJumpForce;  // base jump force
    float jumpForce;      // determines how high the jump is
    bool isJumping;       // tracks if the object is jumping or not
    bool isJumpButtonHeld;  // tracks if the jump button is being held

    bool isFacingRight = false; // track is the object is facing right or not 


    Rigidbody2D rb;     // place to store the rigidbody of the object

    void Start()
    {
        speed = 10f;
        baseJumpForce = 300f;               // set base jump value to 400f
        jumpForce = baseJumpForce;

        rb = GetComponent<Rigidbody2D>();   // store the rb of the object
    }

    void Update()
    {
        // move the player
        horizontalInput = Input.GetAxis("Horizontal");                     // set move to read any of the Unity Horizontal keybinds

        rb.velocity = new Vector2(speed * horizontalInput, rb.velocity.y); // move on the x-axis (left or right)

        // single jump limit
        if (Input.GetButtonDown("Jump") && !isJumping)          // when the Unity Jump keybind is pressed and if the object is not already jumping
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce)); // jump
            isJumping = true;                                   // set jumping to true
        }

        // Check if the jump button is being held for higher jump
        if (Input.GetButton("Jump") && isJumping)
        {
            isJumpButtonHeld = true;
        }

        // Release the jump button to reset the jump force
        if (Input.GetButtonUp("Jump"))
        {
            isJumpButtonHeld = false;
        }

        // Dynamically adjust jump force if the jump button is held
        if (isJumping && isJumpButtonHeld)
        {
            // control how much holding the jump button affects the jump height
            jumpForce = baseJumpForce * 1.5f;
        }
        else
        {
            jumpForce = baseJumpForce;
        }

        FlipSprite();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (horizontalInput * speed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))  // if the other object is tagged as ground
        {
            isJumping = false;                     // set jumping to false
        }

    }

    void FlipSprite()   // face the player right from what it is currently not 
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
}
