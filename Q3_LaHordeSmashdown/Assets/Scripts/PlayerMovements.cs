using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{

    public float speed = 5f;
    public float jumpForce = 15f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    Transform myTransform;

    private Vector2 leftJoystickValue;
    private int jumpCount = 2;

    public Vector2 gravityWallForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();

        jumpCount = 2;
    }




    private void Update()
    {
        if(leftJoystickValue.x < -0.5f) {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            //myTransform.Translate(new Vector2(-speed * Time.deltaTime, 0));
        }
        else if(leftJoystickValue.x > 0.5f) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //myTransform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
    }

    private void Jump()
    {
        if(jumpCount > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
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

    
    public void OnMove(InputAction.CallbackContext context)
    {
        leftJoystickValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
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
