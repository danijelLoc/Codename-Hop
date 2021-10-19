using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovemont : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D playerRigidbody;
    private Animator animator; 
    private BoxCollider2D boxCollider;

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


    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        checkIsGrounded();
        checkHorizontalShiftInput();
        checkJumpInput();
    }

    private void checkHorizontalShiftInput() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRigidbody.velocity = new Vector2(horizontalInput * speed, playerRigidbody.velocity.y);

        #region animation and sprites
        checkHorizontalSpriteFlip(horizontalInput);
        AnimatorRunningState = horizontalInput != 0;
        #endregion
    }

    private void checkHorizontalSpriteFlip(float horizontalInput) 
    {
        if (horizontalInput >= 0.1f)
            transform.localScale = Vector3.one;
        else if (horizontalInput <= -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void checkJumpInput() 
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            jump();
        }
    }

    private void checkIsGrounded() 
    {
        AnimatorGroundedState = isGrounded();
    }

    private void jump() 
    {
        AnimatorGroundedState = false;
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, speed / 2);
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            AnimatorGroundedState = true;
    }
*/


    private bool isGrounded() 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null ;
    }

}
