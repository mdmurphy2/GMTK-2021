using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    public LayerMask groundLayer;
    public float maxSpeed = 10f;
    public float speed = 5f;
    public float jumpForce = 30f;

    private bool canDoubleJump = true;

    private bool grounded = true;



    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
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

    private void CheckJump() {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
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
    }

    private bool IsGrounded()
    {
        float extraHeight = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.down, capsuleCollider2D.bounds.extents.y + extraHeight, groundLayer);

        return raycastHit.collider != null;
    }
}
