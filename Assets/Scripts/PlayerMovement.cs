using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private Rigidbody2D body;
    public float jumpPower;
    private float Move; 
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float walljumpCooldown; 
    private bool isFacingRight = true;

    private bool isWallJumping;
    private float wallJumpingDirection;
    public float wallJumpingTime;
    private float wallJumpingCounter;
    public float wallJumpingDuration; 
    private Vector2 wallJumpingPower = new Vector2(8f,12f);
 
    public bool isWallSliding;
    public float wallSlidingSpeed;

    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;


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
        
        if(!isWallJumping)
        {
            body.velocity = new Vector2(Move * speed, body.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && isGrounded())
        {
            anim.SetTrigger("jump");
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        if(Input.GetKeyUp(KeyCode.UpArrow) && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
        }
        //Set animator parameters
        anim.SetBool("run", Move != 0);
        anim.SetBool("grounded", isGrounded()); 

       
        wallSlide();
        wallJump();
        
        if(!isWallJumping)
        {    
            flip();
            stopWallJumping();
        }   
    }
    
    //flip
    private void flip()
    {
        if(isFacingRight && Move < 0f || !isFacingRight && Move > 0f)
        {
            isFacingRight =!isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

     private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void wallSlide()
    {
        if(isWalled() && !isGrounded() && Move != 0f)
            {
                isWallSliding  = true;
                body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = false;
            }
    }

    private void wallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(stopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            body.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f; 

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
                Invoke(nameof(stopWallJumping), wallJumpingDuration);
        }
    }

    private void stopWallJumping()
    {
        isWallJumping = false;
    }

}
