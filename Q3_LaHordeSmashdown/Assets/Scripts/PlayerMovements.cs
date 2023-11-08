using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovements : MonoBehaviour
{

    public float speed = 5f;
    public float jumpForce = 15f;

    Rigidbody2D rb;
    Transform myTransform;

    private Vector2 leftJoystickValue;
    private int jumpCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    

    private void Update()
    {
        if(leftJoystickValue.x < -0.5f)
        {
            myTransform.Translate(new Vector2(-speed * Time.deltaTime, 0));
        }
        else if(leftJoystickValue.x > 0.5f)
        {
            myTransform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumpCount++;
    }

    private bool IsGrounded()
    {


        return false;
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        leftJoystickValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && jumpCount <= 1)
        {
            Jump();
        }
    }
}
