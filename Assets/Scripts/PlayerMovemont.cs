using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovemont : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Animator animator;
    [SerializeField] private float speed;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        checkHorizontalShiftInput();
        checkJumpInput();
    }

    private void checkHorizontalShiftInput() {
        float horizontalInput = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(horizontalInput * speed, rigidbody.velocity.y);

        #region animation and sprites
        checkHorizontalSpriteFlip(horizontalInput);
        animator.SetBool("isRunning", horizontalInput != 0);
        #endregion
    }

    private void checkHorizontalSpriteFlip(float horizontalInput) {
        if (horizontalInput >= 0.1f)
            transform.localScale = Vector3.one;
        else if (horizontalInput <= -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void checkJumpInput() {
        if (Input.GetKey(KeyCode.Space) && animator.GetBool("isGrounded"))
        {
            jump();
        }
    }

    private void jump() {
        animator.SetBool("isGrounded", false);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed / 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            animator.SetBool("isGrounded", true);
    }

}
