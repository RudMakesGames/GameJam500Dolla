using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float jumpSpeed = 16f;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumpCount = 2;

    [Header("Jump Improvements")]
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 groundCheckOffset = Vector2.zero;

    private float horizontal;
    private bool isFacingRight = true;
    private int jumpCount;
    private bool hasJumped;

    // Jump improvement variables
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Movement
        rb.linearVelocity = new Vector2(horizontal * runningSpeed, rb.linearVelocity.y);

       if(Math.Abs(horizontal) > 0)
        {
            anim.SetBool("IsWalking", true);
        }
       else
        {
            anim.SetBool("IsWalking", false);
        }

        // Coyote time logic
        if (IsGrounded())
        {
            jumpCount = 0;
            hasJumped = false;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer countdown
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;

            // Execute buffered jump when possible
            if ((coyoteTimeCounter > 0f || jumpCount < maxJumpCount) && jumpBufferCounter > 0)
            {
                PerformJump();
                jumpBufferCounter = 0f; // Reset buffer
            }
        }

        // Flip sprite
        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime;

            // Immediate jump if conditions are met
            if (coyoteTimeCounter > 0f || jumpCount < maxJumpCount)
            {
                PerformJump();
                jumpBufferCounter = 0f; // Reset buffer since we jumped immediately
            }
        }

        // Jump cut - reduce upward velocity when button is released
        if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);

        if (coyoteTimeCounter > 0f)
        {
            jumpCount = 1; // First jump used coyote time
            coyoteTimeCounter = 0f; // Reset coyote time
        }
        else
        {
            jumpCount++; // Multi-jump
        }

        hasJumped = true;
    }

    private bool IsGrounded()
    {
        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;
        return Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null || Application.isPlaying)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;
            Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
        }
    }
}
