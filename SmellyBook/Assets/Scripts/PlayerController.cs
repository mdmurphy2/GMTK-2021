using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

 public enum Character
    {
        Ninja,
        Samurai,
    }

public static class Stats {
    private static Character currentCharacter = Character.Ninja;
    public static Character CurrentCharacter {
        get{
            return currentCharacter;
        } set {
            currentCharacter = value;
        }
    }
}

public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite ninjaSprite;
    [SerializeField] Sprite samuraiSprite;
    [SerializeField] Animator animator;


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
    public float characterSwapDelay = .2f;
    private bool canSwapCharacters = true;
    private float timeSinceLastSwap = 0f;

    public float coyoteTime = .2f;
    private bool canCoyoteJump = true;
    public float coyoteTimer = 0f;
    private float gravity;
    private float drag;

    private readonly float RAD45 = Mathf.Sqrt(2) / 2;
   


    public Transform rightAttackPoint;
    public Transform downAttackPoint;

    public float attackRange = .5f;
    public LayerMask enemyLayer;
    private bool attack;
    private bool canAttack = true;
    public float attackDelay = .2f;
    private float attackTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gravity = rigidbody2D.gravityScale;
        drag = rigidbody2D.drag;

        SetCharacter(Stats.CurrentCharacter);
    }

    private void Update()
    {
        SwitchCharacters();
        CheckJump();
        CheckDash();
        CheckAttack();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Find Direction of Attack
        MoveHorizontal();
        //ApplyGravity();
        ApplyDash();
        ApplyAttack();
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
    
    private void CheckAttack() {
        if(!canAttack) {
            attackTimer += Time.deltaTime;
            if(attackTimer > attackDelay) {
                canAttack = true;
            }
        }

        if(currentCharacter == Character.Samurai) {
            if(Input.GetButtonDown("Fire1")) {
                if(canAttack) {
                    Debug.Log("attacking");
                    canAttack = false;
                    attack = true;
                    attackTimer = 0f;
                }
            }
        }
        
    }

    private void OnDrawGizmosSelected() {
        if(rightAttackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(rightAttackPoint.position, attackRange);    
        Gizmos.DrawWireSphere(downAttackPoint.position, attackRange);    

    }

    private void ApplyAttack() {
        if(attack) {
            attack = false;
            //Find Direction of Attack
            float horizontal = Input.GetAxisRaw("Horizontal");

            bool jumpFromAttack = false;
            
            Collider2D[] hits = new Collider2D[0];
           
             if(!IsGrounded()) { //Up Or Down
                    hits = Physics2D.OverlapCircleAll(downAttackPoint.position, attackRange, enemyLayer);
                    Debug.Log("Attack Down");
                    animator.SetTrigger("swingRight");
                    jumpFromAttack = true;
            }  else { //Left Or Right
                if(horizontal < 0) {
                    Debug.Log("Attack Left");
                } else { //If not moving attack right
                    hits = Physics2D.OverlapCircleAll(rightAttackPoint.position, attackRange, enemyLayer);
                    animator.SetTrigger("swingRight");
                    Debug.Log("Attack right");
                }
                
            }



            foreach(Collider2D enemy in hits) {
                enemy.GetComponent<Enemy>().TakeDamage(1);
                if(jumpFromAttack) {
                    jumpFromAttack = false;
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                    rigidbody2D.AddForce(new Vector2(0, jumpForce * 1.1f), ForceMode2D.Impulse);
                }
            }
            
        }
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
                SetCharacter(Character.Samurai);
                currentCharacter = Character.Samurai;
                spriteRenderer.sprite = samuraiSprite;
                Stats.CurrentCharacter = currentCharacter;
                timeSinceLastSwap = 0;
                canSwapCharacters = false;
            }
            else
            {
                SetCharacter(Character.Ninja);
                currentCharacter = Character.Ninja;
                spriteRenderer.sprite = ninjaSprite;
                Stats.CurrentCharacter = currentCharacter;
                timeSinceLastSwap = 0;
                canSwapCharacters = false;
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

            if(Input.GetButton("Fire1") && canDash) {
                Debug.Log("DASH");
                canSwapCharacters = false;
                timeSinceLastSwap = 0f;
                
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
        // if (currentCharacter == Character.Ninja)
        // {
        //     if (Input.GetButtonDown("Jump"))
        //     {
        //         if (IsGrounded() && Mathf.Approximately(rigidbody2D.velocity.y, 0f))
        //         {
        //             Debug.Log("JUMP1");
        //             rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
        //             rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        //             canDoubleJump = true;
        //         }
        //         else
        //         {
        //             // if (canDoubleJump)
        //             // {
        //             //     Debug.Log("JUMP2");
        //             //     rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
        //             //     rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        //             //     canDoubleJump = false;
        //             // }
        //         }
        //     }
        //     if (IsGrounded())
        //     {
        //         canDoubleJump = true;
        //     }
        // }
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

    public void Die() 
    {
        //Do some fadiing

        //Do Death Stuff
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public Character GetCharacter() {
        return currentCharacter;
    }
    public void SetCharacter(Character character) {
        currentCharacter = character;
        Stats.CurrentCharacter = character;
         if (character == Character.Ninja)
            {
                animator.SetBool("isNinja", true);
                animator.SetBool("isSamurai", false);
            }
            else
            {
                animator.SetBool("isNinja", false);
                animator.SetBool("isSamurai", true);
            }
    }

}
