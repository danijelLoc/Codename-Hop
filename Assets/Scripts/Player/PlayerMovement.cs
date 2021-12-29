using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpVerticalSpeed;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float airborneControlFactor;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private Collider2D collider2d;
    private Health health;
    private Shooting shooting;
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

    private bool AnimatorOnGuardState
    {
        get { return animator.GetBool("onGuard"); }
        set { animator.SetBool("onGuard", value); }
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
        collider2d = GetComponent<Collider2D>();
        health = GetComponent<Health>();
        shooting = GetComponent<Shooting>();
    }

    private void Update()
    {
        AnimatorGroundedState = isGrounded();        
        checkWallDistance();
        checkShootInput();
        checkOnGuardInput();
        checkJumpInput();
        checkHorizontalShiftInput();
        AnimatorRunningState = HorizontalInput != 0 && AnimatorGroundedState && !AnimatorOnGuardState;
        if (health && health.isDead()) this.enabled = false;
    }

    private void checkHorizontalShiftInput()
    {
        if (AnimatorGroundedState && !AnimatorOnGuardState && !momentJustAfterJump)
        {
            playerRigidbody.velocity = new Vector2(HorizontalInput * runningSpeed, playerRigidbody.velocity.y);
            setHorizontalSpriteOrientation();
        }
        else if(!AnimatorWallState && !AnimatorOnGuardState)
        {
            playerRigidbody.velocity = new Vector2(getHorizontalAirborneVelocity(), playerRigidbody.velocity.y);
            setHorizontalSpriteOrientation();
        }
    }

    private void checkOnGuardInput()
    {
        if (AnimatorGroundedState && !momentJustAfterJump)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.SetTrigger("takeGuard");
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return;
            }
        }
        AnimatorOnGuardState = false;
    }

    private void checkShootInput()
    {
        if (canShoot())
        {
            if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.W))
            {
                shooting.ShootUp();
            }
            else if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.S))
            {
                shooting.ShootDown();
            }
            else if (Input.GetKey(KeyCode.F) )
            {
                shooting.Shoot();
            }
        }
    }

    private void checkJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
            playerRigidbody.gravityScale = 1;
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
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D raycastHitOnEnemy = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.1f, enemyLayer);
        return raycastHitGround.collider != null || raycastHitOnEnemy.collider != null;
    }

    private bool isOnTheWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.01f, wallLayer);
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
        return !isOnTheWall();
    }

    public bool canAttackWithSword()
    {
        return !isOnTheWall();
    }

    public void SetOnGourd()
    {
        AnimatorOnGuardState = true;
    }
}
