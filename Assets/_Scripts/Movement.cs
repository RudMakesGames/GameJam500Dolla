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

    [Header("Idle Animation")]
    [SerializeField] private float idleAnimationSwitchTime = 5f;
    [SerializeField] private AnimationClip IdleClip;
    [SerializeField] private AnimationClip Idle2Clip;

    private float horizontal;
    private bool isFacingRight = true;
    private int jumpCount;
    private bool hasJumped;

    // Jump improvement variables
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // Idle animation variables
    [SerializeField]
    private float idleTimer = 0f;
    private bool isUsingIdle2 = false;
    private bool wasMovingLastFrame = false;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Animator anim;
    private AnimatorOverrideController animatorOverrideController;

    public ParticleSystem DustParticleFx;
    private bool wasGroundedLastFrame = false;
    private bool dustParticlesPlaying = false;
    [Header("Footstep Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip grassStep;
    public AudioClip woodStep;
    public AudioClip stoneStep;
    public AudioClip defaultStep;

    public LayerMask groundRaycastLayer;
    public float groundRaycastDistance = 0.1f;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up animator override controller for runtime animation switching
        SetupAnimatorOverride();
    }

    private void Update()
    {

        if(IsGrounded())
        {
            anim.SetBool("IsFalling",false);
        }
        else
        {
            anim.SetBool("IsFalling", true);
        }
        // Movement
        rb.linearVelocity = new Vector2(horizontal * runningSpeed, rb.linearVelocity.y);

        bool isMoving = Math.Abs(horizontal) > 0;

        if (isMoving)
        {
            anim.SetBool("IsWalking", true);

            
            if (!wasMovingLastFrame)
            {
                ResetIdleAnimation();
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);

           
            HandleIdleAnimations();
        }

        wasMovingLastFrame = isMoving;

        // Coyote time logic
        bool isCurrentlyGrounded = IsGrounded();
        if (isCurrentlyGrounded)
        {
            jumpCount = 0;
            hasJumped = false;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        
        
        if (!wasGroundedLastFrame && isCurrentlyGrounded)
        {
            // Player just landed - start dust particles
            if (DustParticleFx != null)
            {
                DustParticleFx.Play();
                dustParticlesPlaying = true;
            }
        }

        // Stop dust particles if grounded and moving
        

        
        wasGroundedLastFrame = isCurrentlyGrounded;

        
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;

            
            if ((coyoteTimeCounter > 0f || jumpCount < maxJumpCount) && jumpBufferCounter > 0)
            {
                PerformJump();
                jumpBufferCounter = 0f; 
            }
        }

        // Flip sprite
        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();
    }
    private void HandleIdleAnimations()
    {
        // Increment idle timer
        idleTimer += Time.deltaTime;

        // Check if it's time to switch idle animations
        if (idleTimer >= idleAnimationSwitchTime)
        {
            SwitchIdleAnimation();
            idleTimer = 0f; // Reset timer
        }
    }

    private void SwitchIdleAnimation()
    {
        if (animatorOverrideController == null) return;

        // Switch between idle animations
        isUsingIdle2 = !isUsingIdle2;

        if (isUsingIdle2)
        {
            // Switch to Idle2
            animatorOverrideController["Idle"] = Idle2Clip;
        }
        else
        {
            // Switch back to Idle
            animatorOverrideController["Idle"] = IdleClip;
        }
    }

    private void ResetIdleAnimation()
    {
        idleTimer = 0f;
        isUsingIdle2 = false;

        // Make sure we're using the default idle animation
        if (animatorOverrideController != null)
        {
            animatorOverrideController["Idle"] = IdleClip;
        }
    }

    private void SetupAnimatorOverride()
    {
       
        RuntimeAnimatorController runtimeController = anim.runtimeAnimatorController;
        animatorOverrideController = new AnimatorOverrideController(runtimeController);
        anim.runtimeAnimatorController = animatorOverrideController;

        // Set default idle animation
        if (IdleClip != null)
        {
            animatorOverrideController["Idle"] = IdleClip;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(CutsceneManager.instance?.isCutsceneActive == false)
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && CutsceneManager.instance?.isCutsceneActive == false)
        {
            jumpBufferCounter = jumpBufferTime;

            // Immediate jump if conditions are met
            if (coyoteTimeCounter > 0f || jumpCount < maxJumpCount)
            {
                PerformJump();
                jumpBufferCounter = 0f; // Reset buffer since we jumped immediately
                anim.SetTrigger("Jump");
            }
        }

        // Jump cut - reduce upward velocity when button is released
        if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            anim.SetTrigger("Jump");
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
        if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            
        }

    }

    public void PlayFootstepSound()
    {
        // Ensure player is grounded and moving
        if (!IsGrounded() || Mathf.Abs(horizontal) < 0.1f)
            return;

        // Raycast downward to detect ground surface
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundRaycastLayer);
        if (hit.collider == null) return;

        // Get the physics material name
        var material = hit.collider.sharedMaterial;

        if (material != null)
        {
            switch (material.name)
            {
                case "Grass":
                    footstepAudioSource.PlayOneShot(grassStep);
                    break;
                case "Wood":
                    footstepAudioSource.PlayOneShot(woodStep);
                    break;
                case "Stone":
                    footstepAudioSource.PlayOneShot(stoneStep);
                    break;
                default:
                    footstepAudioSource.PlayOneShot(defaultStep);
                    break;
            }
        }
        else
        {
            footstepAudioSource.PlayOneShot(defaultStep);
        }
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
