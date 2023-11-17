using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{

    public GameData gameData;

    public float speed = 5f;
    
    public LayerMask groundLayer;
    public Vector2 gravityWallForce;

    public bool _ejection;

    public Animator _animatorLowHalf;

    public SpriteRenderer spritePlayer;

    Rigidbody2D rb;
    [SerializeField] private List<Collider2D> colliders;
    private GameObject currentPlatform;

    private Vector2 leftJoystickValue;

    [Header("Jump")]
    public float jumpForce = 15f;
    public int maxJump = 2;
    private int jumpCount = 2;
    private bool isJumping = false;
    public float maxTimeJump = 0.8f;
    private float jumpTimer = 0.0f;

    [Header("Dodge")]
    public float dodgeTime = 0.2f;
    private bool canDodge = true;
    private bool isDodge = false;
    public bool canMove = true;
    public float reloadDodgeTime = 1.2f;
    public Color dodgingColor;
    public Color normaColor;
    public GameObject floorCollider;
    public float dashForce = 2.5f;
    private Vector2 joystickValue;
    float actTime = 0.0f;

    public bool onTheWall = false;

    private AudioSource source;
    public AudioClip dashSound;

    public ParticleSystem particles;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        jumpCount = maxJump;
        canDodge = true;

        source = GetComponent<AudioSource>();

        speed = gameData.playerSpeed;
        maxJump = gameData.jump;
        jumpForce = gameData.jumpForce;
        dodgeTime = gameData.dodgeTime;
        dashForce = gameData.dodgeForce;
        
    }

    private void Start()
    {
        dodgingColor = normaColor;
        dodgingColor.a = 0.35f;

        _ejection = false;

        _animatorLowHalf.SetBool("Grouded", true);

        spritePlayer.color = normaColor;
    }


    private void FixedUpdate()
    {
        if(isJumping && jumpTimer < maxTimeJump)
        {
            rb.velocity = Vector2.up * jumpForce + new Vector2(rb.velocity.x, 0);
            jumpTimer += Time.deltaTime;
        }

        if(isDodge && actTime <= dodgeTime)
        {
            actTime += Time.deltaTime;
            rb.velocity = joystickValue * dashForce;
        }
    }

    private void Jump()
    {
        if(jumpCount > 0)
        {
            rb.gravityScale = 1;
            rb.velocity = Vector2.zero + new Vector2(rb.velocity.x, 0);
            rb.velocity = Vector2.up * jumpForce + new Vector2(rb.velocity.x, 0);
            isJumping = true;
            jumpCount--;
        }

    }

    public void GrabWall()
    {
        rb.gravityScale = 0;
        jumpCount = maxJump;
        rb.velocity = Vector2.zero;
        rb.AddForce(gravityWallForce);
        GetComponent<Animator>().SetBool("OnWall", true);
        _animatorLowHalf.SetBool("OnWall", true);
        onTheWall = true;
    }

    public void UnGrabWall()
    {
        rb.gravityScale = 1;
        GetComponent<Animator>().SetBool("OnWall", false);
        _animatorLowHalf.SetBool("OnWall", false);
        onTheWall = false;
    }

    private void Dodge()
    {
        if(canDodge && !GetComponent<PlayerAttack>()._isPause)
        {
            canMove = false;
            canDodge = false;
            isDodge = true;

            rb.velocity = Vector2.zero;
            
            particles.Play();

            joystickValue = leftJoystickValue;
            actTime = 0;

            source.clip = dashSound;
            source.Play();

            if (currentPlatform == null)
                rb.gravityScale = 0;

            floorCollider.SetActive(true);

            spritePlayer.color = dodgingColor;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(DodgeTime());
        }
        
    }

    IEnumerator DodgeTime()
    {
        yield return new WaitForSeconds(dodgeTime);
        rb.gravityScale = 1;
        floorCollider.SetActive(false);
        canMove = true;
        particles.Stop();
        isDodge = false;
        GetComponent<Collider2D>().enabled = true;
        spritePlayer.color = normaColor;
        StartCoroutine(DodgeReloadTime());
    }

    IEnumerator DodgeReloadTime()
    {
        yield return new WaitForSeconds(reloadDodgeTime);
        canDodge = true;
    }

    IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        foreach(Collider2D col in colliders)
        {
            Physics2D.IgnoreCollision(col, platformCollider);
        }
        yield return new WaitForSeconds(0.25f);
        foreach (Collider2D col in colliders)
        {
            Physics2D.IgnoreCollision(col, platformCollider, false);
        }
    }
   
    public void OnMove(InputAction.CallbackContext context)
    {
        leftJoystickValue = context.ReadValue<Vector2>();

        if (context.started)
        {
            GetComponent<PlayerAttack>()._joystickTuched = true;
        }

        if (context.canceled)
        {
            GetComponent<PlayerAttack>()._joystickTuched = false;
            _animatorLowHalf.SetBool("Moove", false);
        }

        if (canMove && !GetComponent<PlayerAttack>()._isPause)
        {
            if (leftJoystickValue.x < -0.5f)
            {
                _animatorLowHalf.SetBool("Moove", true);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new(0, 180, 0));
            }
            else if (leftJoystickValue.x > 0.5f)
            {
                _animatorLowHalf.SetBool("Moove", true);
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new(0, 0, 0));
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if(leftJoystickValue.y < -0.5f && currentPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }

            if(leftJoystickValue.y < -0.5f && !canDodge)
            {
                rb.velocity += new Vector2(0, -1.5f);
            }
        }
        else if (GetComponent<PlayerAttack>()._isPause)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && canMove && !GetComponent<PlayerAttack>()._isPause) 
        {
            Jump();
            _animatorLowHalf.Play("Jump");
            _animatorLowHalf.SetBool("Grouded", false);
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
            transform.parent = null;
            jumpCount = maxJump;
            if (GetComponent<PlayerAttack>()._joystickTuched == false)
            {
                rb.velocity = Vector2.zero;
            }
            _animatorLowHalf.SetBool("Grouded", true);
        }

        if(_ejection)
        {
            rb.velocity = Vector2.zero;
            _ejection = false;
        }

        if(collision.transform.tag == "Platform")
        {
            jumpCount = maxJump;
            currentPlatform = collision.gameObject;
            _animatorLowHalf.SetBool("Grouded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            currentPlatform = null;
        }
    }
}
