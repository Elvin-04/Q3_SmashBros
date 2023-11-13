using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{

    public float speed = 5f;
    
    public LayerMask groundLayer;
    public Vector2 gravityWallForce;

    Rigidbody2D rb;
    Transform myTransform;

    private Vector2 leftJoystickValue;

    [Header("Jump")]
    public float jumpForce = 15f;
    private int jumpCount = 2;
    private bool isJumping = false;
    public float maxTimeJump = 0.8f;
    private float jumpTimer = 0.0f;

    [Header("Dodge")]
    public float dodgeTime = 0.2f;
    private bool canDodge = true;
    public bool canMove = true;
    public float reloadDodgeTime = 1.2f;
    public Color dodgingColor;
    public Color normaColor;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();

        jumpCount = 2;
        canDodge = true;
    }


    private void FixedUpdate()
    {
        if(isJumping && jumpTimer < maxTimeJump)
        {
            rb.velocity = Vector2.up * jumpForce + new Vector2(rb.velocity.x, 0);
            jumpTimer += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if(jumpCount > 0)
        {
            rb.velocity = Vector2.zero + new Vector2(rb.velocity.x, 0);
            rb.velocity = Vector2.up * jumpForce + new Vector2(rb.velocity.x, 0);
            isJumping = true;
            jumpCount--;
        }

    }

    public void GrabWall()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.AddForce(gravityWallForce);
    }

    public void UnGrabWall()
    {
        rb.gravityScale = 1;
    }

    private void Dodge()
    {
        if(canDodge)
        {
            canMove = false;
            canDodge = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;

            GetComponent<SpriteRenderer>().color = dodgingColor;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(DodgeTime());
        }
        
    }

    IEnumerator DodgeTime()
    {
        yield return new WaitForSeconds(dodgeTime);
        rb.gravityScale = 1;
        canMove = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normaColor;
        StartCoroutine(DodgeReloadTime());
    }

    IEnumerator DodgeReloadTime()
    {
        yield return new WaitForSeconds(reloadDodgeTime);
        canDodge = true;
    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("test");
        leftJoystickValue = context.ReadValue<Vector2>();

        if (context.started)
        {
            GetComponent<PlayerAttack>()._joystickTuched = true;
        }

        if (context.canceled)
        {
            GetComponent<PlayerAttack>()._joystickTuched = false;
        }

        if (canMove)
        {
            if (leftJoystickValue.x < -0.5f)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new(0, 180, 0));
            }
            else if (leftJoystickValue.x > 0.5f)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new(0, 0, 0));
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }

        if(context.canceled)
        {
            isJumping = false;
            jumpTimer = 0.0f;
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Dodge();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ground")
        {
            jumpCount = 2;
        }
    }
}
