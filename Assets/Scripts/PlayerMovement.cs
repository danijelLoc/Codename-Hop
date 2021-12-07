using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpVerticalSpeed;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float airborneControlFactor;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool momentJustAfterJump = false; //Moment after jump, player is still close to ground. Horizontal input at running speed must be avoided.
    private float wallJumpCoolDown;

    private bool AnimatorGroundedState
    {
        get { return animator.GetBool("isGrounded"); }
        set { animator.SetBool("isGrounded", value); }
    }

    private bool AnimatorRunningState
    {
        get { return animator.GetBool("isRunning"); }
        set { animator.SetBool("isRunning", value); }
    }

    private bool AnimatorWallState
    {
        get { return animator.GetBool("isOnTheWall"); }
        set { animator.SetBool("isOnTheWall", value); }
    }

    private float HorizontalInput
    {
        get { return Input.GetAxis("Horizontal"); }
    }

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        AnimatorGroundedState = isGrounded();
        checkWallDistance();
        checkJumpInput();
        checkHorizontalShiftInput();
        
    }

    private void checkHorizontalShiftInput()
    {
        if (AnimatorGroundedState && !momentJustAfterJump)
        {
            playerRigidbody.velocity = new Vector2(HorizontalInput * runningSpeed, playerRigidbody.velocity.y);
            AnimatorRunningState = HorizontalInput != 0;
            setHorizontalSpriteOrientation();
        }
        else if(!AnimatorWallState)
        {
            playerRigidbody.velocity = new Vector2(getHorizontalAirborneVelocity(), playerRigidbody.velocity.y);
            setHorizontalSpriteOrientation();
        }
        
    }

    private void checkJumpInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if(AnimatorGroundedState)
                jump();
            if (AnimatorWallState)
                wallJump();
            momentJustAfterJump = true;
        }
        else
        {
            momentJustAfterJump = false;
        }
    }

    private void checkWallDistance()
    {
        bool playerOnTheWall = isOnTheWall();
        if (playerOnTheWall && !AnimatorGroundedState)
        {
            playerRigidbody.gravityScale = 0;
            playerRigidbody.velocity = Vector2.zero;
        }
        else
        {
            playerRigidbody.gravityScale = 2;
        }
        AnimatorWallState = playerOnTheWall;
    }

    private void setHorizontalSpriteOrientation()
    {
        if (HorizontalInput > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (HorizontalInput < 0)
        {
            // Face left
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void jump()
    {
        playerRigidbody.velocity = new Vector2(HorizontalInput * jumpHorizontalSpeed, jumpVerticalSpeed);
    }

    private void wallJump()
    {
        playerRigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, jumpVerticalSpeed * 0.7f);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isOnTheWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.01f, wallLayer);
        return raycastHit.collider != null;
    }

    private float getHorizontalAirborneVelocity()
    {
        float tempVelocity = playerRigidbody.velocity.x + HorizontalInput * airborneControlFactor;
        if (playerRigidbody.velocity.x < 0 && tempVelocity < -jumpHorizontalSpeed)
            return -jumpHorizontalSpeed;
        else if (playerRigidbody.velocity.x > 0 && tempVelocity > jumpHorizontalSpeed)
            return jumpHorizontalSpeed;
        else
            return tempVelocity;
    }

    public bool canShoot() 
    {
        return isGrounded() && ! isOnTheWall();
    }
}
