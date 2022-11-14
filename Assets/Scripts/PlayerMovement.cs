using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    public float jumpPower;
    private float Move;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask wallLayer; 
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float walljumpCooldown; 
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {   
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");                           

        //flip player when moving left and right
        if(Move > 0.01f)
            transform.localScale = Vector3.one;
        else if(Move < -0.01f)
            transform.localScale =new Vector3(-1,1,1);    

        //Set animator parameters
        anim.SetBool("run", Move != 0);
        anim.SetBool("grounded", isGrounded()); 



        // wall jump logic
         if(walljumpCooldown > 0.2f)
         {
             body.velocity = new Vector2(speed * Move, body.velocity.y);

             if(onWall() && !isGrounded())
             {
                 body.gravityScale = 0;
                 body.velocity = Vector2.zero;
             }
             else 
                 body.gravityScale = 2;

             if(Input.GetKey(KeyCode.Space))       
                 Jump();
         }
         else 
             walljumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if(isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            if(Move == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10,0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z); 
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 4,6);

            walljumpCooldown = 0;
             
        }

      
    }
    
    private void OnCollisionEnter2D(Collision2D other)              //Checks if the player is touching the ground
    {
      //  if(other.gameObject.CompareTag("Ground"))
      //  {
      //      grounded = true;
      //      //isJumping = false;
      //  }
    } 

   /* private void OnCollisionExit2D(Collision2D other)               //Checks if the player is touching the ground
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }
*/
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer); 
        return raycastHit.collider != null;
    }

     private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer); 
        return raycastHit.collider != null;
    }


}
