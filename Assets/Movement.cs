using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement Settings")]
    [SerializeField] private float jumpSpeed = 12f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxJumpCount = 2;

    private float horizontal;
    private int jumpCount = 0;
    private bool hasJumped = false;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movement
        rb.linearVelocity = new Vector2(horizontal * runningSpeed, rb.linearVelocity.y);

        // Ground check
        if (IsGrounded())
        {
            jumpCount = 0;
            hasJumped = false;
        }

        // Flip sprite
        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();
    }

    public void MoveHorizontally(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded() || jumpCount < maxJumpCount)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
                if (!IsGrounded()) jumpCount++;
                else jumpCount = 1;
                hasJumped = true;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
