using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float defaultMoveSpeed = 6f;
    private const float defaultJumpSpeed = 10f;
    private const float defaultGravity = 2f;

    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalConstraints;

    private BoxCollider2D boxCollider;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private static Vector2 moveValue;
    private static bool isGrounded;
    private float moveSpeed;
    private float jumpSpeed;
    private bool dashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalConstraints = rb.constraints;
        boxCollider = GetComponent<BoxCollider2D>();

        moveSpeed = defaultMoveSpeed;
        jumpSpeed = defaultJumpSpeed;
        rb.gravityScale = defaultGravity;
    }

    public void move(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
        FlipPlayer();
    }

    public void jump(InputAction.CallbackContext context)
    {
        //Allow player to jump when grounded
        moveValue.y = 1;
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        //Lower jump height if jump not held down fully
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void dash(InputAction.CallbackContext context)
    {
        if (context.performed && !dashing)
        {
            dashing = true;
            moveValue.x = 3 * transform.localScale.x;
            rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            StartCoroutine(EndDash());
        }
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(0.3f);
        rb.constraints = originalConstraints;
        moveValue.x = 0;
        yield return new WaitForSeconds(0.5f);
        dashing = false;
    }

    private void FlipPlayer()
    {
        //Flip player according to direction of movement
        if (moveValue.x > 0)
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                1
            );
        if (moveValue.x < 0)
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * -1,
                transform.localScale.y,
                1
            );
    }

    private bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.35f, groundLayer);
        return isGrounded;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveValue.x * moveSpeed, rb.velocity.y);
    }
}
