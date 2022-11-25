using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    [SerializeField] private float speed;
    private float wallJumpCoolDown;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //sets the input.getaxis to horzontal input
        horizontalInput = Input.GetAxis("Horizontal");
        //moves the player using a designer chosen speed
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        //flips the direction player is looking
        if(horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(.5f,.5f,.5f);
        }
        else if(horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-.5f,.5f,.5f);
        }

        //Set animator parameters
        anim.SetBool("isWalking", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //jump code
        if (wallJumpCoolDown > .2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 1;
            
            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCoolDown += Time.deltaTime;

    }
    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 6);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 6);

            wallJumpCoolDown = 0;
        }
    }
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
    public bool CanAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
