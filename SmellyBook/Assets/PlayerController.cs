using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    public LayerMask groundLayer;
    public float maxSpeed = 10f;
    public float speed = 5f;
    public float jumpForce = 30f;
    public float dashForce = 30f;

    private bool canDoubleJump = true;

    private bool canJump = true;

    private bool dashing = false;
    public float dashTiming = 1f;
    private float timeSinceLastDash = 0f;

    private Character currentCharacter = Character.Ninja;
    public float characterSwapDelay = 5f;
    private bool canSwapCharacters = true;
    private float timeSinceLastSwap = 0f;

    public float coyoteTime = .2f;
    private bool canCoyoteJump = true;
    public float coyoteTimer = 0f;

    private enum Character
    {
        Ninja,
        Samurai,
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        SwitchCharacters();
        CheckJump();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveHorizontal();
    }

    private void MoveHorizontal()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody2D.velocity = new Vector2(horizontal * speed, rigidbody2D.velocity.y);
        
    }

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

    // private void CheckDash() {
    //     if(dashing) {
    //         timeSinceLastDash += Time.deltaTime;
    //         if(timeSinceLastDash >= dashTiming && IsGrounded()) {
    //             dashing = false;
    //         }
    //     }

    //     if(Input.GetButton("Fire2") && !dashing) {
    //         Debug.Log("DASH");
    //         dashing = true;
    //         timeSinceLastDash = 0;

    //        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //        Vector2 direction = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
    //        Debug.Log(direction);


    //         rigidbody2D.AddForce(direction * dashForce, ForceMode2D.Impulse);
    //     }
    // }

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
}
