using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidbody2D;
    public float maxSpeed = 10f;
    public float speed = 5f;
    public float jumpForce = 30f;
    
    private bool canDoubleJump = true;

    private bool grounded = true;



    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update() {

        if(Input.GetButtonDown("Jump")) {
            if(grounded) {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
                canDoubleJump = true;
            }else {
                if (canDoubleJump) {
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                    rigidbody2D.AddForce(new Vector2(0, jumpForce));
                    canDoubleJump = false;
                }
            }
        }


        if(Mathf.Approximately(rigidbody2D.velocity.y,0)) {
            canDoubleJump = true;
        }

        if(Input.GetButtonDown("Jump") && (canDoubleJump || Mathf.Approximately(rigidbody2D.velocity.y,0))){
            
            rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");


        rigidbody2D.velocity = new Vector2(horizontal * speed, rigidbody2D.velocity.y);
        
        
    }
}
