using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] CameraManager cameraManager;
    public LayerMask groundLayer;
    public float maxSpeed = 10f;
    public float speed = 5f;
    public float jumpForce = 30f;
    public float dashSpeed = 30f;
    private Vector2 dashVelocity;

    private bool canDoubleJump = true;

    private bool canJump = true;

    private bool dashing = false;
    private bool canDash = true;
    public float dashTiming = 0.1f;
    public float dashStartup = 0.02f;
    private bool dashStarting = false;
    private float timeSinceLastDash = 0f;

    private Character currentCharacter = Character.Ninja;
    public float characterSwapDelay = 5f;
    private bool canSwapCharacters = true;
    private float timeSinceLastSwap = 0f;

    public float coyoteTime = .2f;
    private bool canCoyoteJump = true;
    public float coyoteTimer = 0f;
    private float gravity;
    private float drag;

    private readonly float RAD45 = Mathf.Sqrt(2) / 2;
    private enum Character
    {
        Ninja,
        Samurai,
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = rigidbody2D.gravityScale;
        drag = rigidbody2D.drag;
    }

    private void Update()
    {
        SwitchCharacters();
        CheckJump();
        CheckDash();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveHorizontal();
        //ApplyGravity();
        ApplyDash();
    }

    private void MoveHorizontal()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody2D.velocity = new Vector2(horizontal * speed, rigidbody2D.velocity.y);
        
    }

    /*private void ApplyGravity() // previous rigidbody gravity was 
    {
        if (!IsGrounded() && !IsDashing()) {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y - CalculateGravity());
        } else if (IsGrounded() && !IsDashing()) {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        }
    }*/

    /**
    models gravity artificially to allow for non-quadratic jump curves
    currently uses linear gravity (v = vt + v0) for a quadratic curve
    */
    /*private float CalculateGravity() {
        return gravity * Time.fixedDeltaTime;
    }*/

    private void SwitchCharacters()
    {
        if (!canSwapCharacters)
        {
            timeSinceLastSwap += Time.deltaTime;
            if (timeSinceLastSwap >= characterSwapDelay)
            {
                canSwapCharacters = true;
            }
        }
        if (Input.GetButtonDown("Fire3") && canSwapCharacters)
        {
            if (currentCharacter == Character.Ninja)
            {
                currentCharacter = Character.Samurai;
                //spriteRenderer.color = Color.red;
            }
            else
            {
                currentCharacter = Character.Ninja;
                //spriteRenderer.color = Color.black;
            }
        }


    }

    private void CheckDash() {
        if (currentCharacter == Character.Ninja) {
            if(dashing) {
                timeSinceLastDash += Time.deltaTime;
                if (dashStarting && timeSinceLastDash >= dashStartup) {
                    dashStarting = false;
                    cameraManager.DoScreenShake(0.25f, 0.2f);
                }
                    
                if(timeSinceLastDash >= dashTiming) {
                    dashing = false;
                    rigidbody2D.velocity *= maxSpeed / dashSpeed;
                    rigidbody2D.gravityScale = gravity;
                    rigidbody2D.drag = drag;
                }
            } else if (IsGrounded()){
                canDash = true;
            }

            if(Input.GetButton("Fire2") && canDash) {
                Debug.Log("DASH");
                dashing = true;
                canDash = false;
                timeSinceLastDash = 0;
                rigidbody2D.gravityScale = 0;
                rigidbody2D.drag = 0;

                dashStarting = true;
            
                dashVelocity = dashSpeed * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
                    dashVelocity *= RAD45;
            }
        }
    }

    private void ApplyDash() {
        if (dashStarting)
            rigidbody2D.velocity = Vector2.zero;
        else if (dashing)
            rigidbody2D.velocity = dashVelocity;
    }

    private void CheckJump()
    {
        if (currentCharacter == Character.Ninja)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded() && Mathf.Approximately(rigidbody2D.velocity.y, 0f))
                {
                    Debug.Log("JUMP1");
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                    rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    canDoubleJump = true;
                }
                else
                {
                    if (canDoubleJump)
                    {
                        Debug.Log("JUMP2");
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                        rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        canDoubleJump = false;
                    }
                }
            }
            if (IsGrounded())
            {
                canDoubleJump = true;
            }
        }

        else
        {
            if(!IsGrounded()) {
                coyoteTimer += Time.deltaTime;
                if(coyoteTimer >= coyoteTime) {
                    canCoyoteJump = false;
                }
            } else {
                canCoyoteJump = true;
                coyoteTimer = 0f;
            }
            if (Input.GetButtonDown("Jump"))
            {
                if ((IsGrounded() && Mathf.Approximately(rigidbody2D.velocity.y, 0f)) || canCoyoteJump)
                {
                    Debug.Log("JUMP1");
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                    rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    canJump = false;
                }
            }
        }
        



    }

    private bool IsGrounded()
    {
        float extraHeight = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.down, capsuleCollider2D.bounds.extents.y + extraHeight, groundLayer);

        return raycastHit.collider != null;
    }

    private bool IsDashing()
    {
        return false;
    }
}
