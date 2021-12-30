using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft = true;

    [Header("Close range Parameters")]
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeReference] private Collider2D collider2d;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private Animator animator;

    private bool AnimatorGroundedState
    {
        get { return animator.GetBool("isGrounded"); }
        set { animator.SetBool("isGrounded", value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initScale = transform.localScale;
    }
    private void OnDisable()
    {
        animator.SetBool("moving", false);
    }

    private void Update()
    {
        AnimatorGroundedState = isGrounded();
        if (!PlayerInCloseRange())
        {
            if (movingLeft)
            {
                if (transform.position.x >= leftEdge.position.x)
                    MoveInDirection(-1);
                else
                    DirectionChange();
            }
            else
            {
                if (transform.position.x <= rightEdge.position.x)
                    MoveInDirection(1);
                else
                    DirectionChange();
            }
        }
        else 
        {
            animator.SetBool("moving", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider2d.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(collider2d.bounds.size.x * range, collider2d.bounds.size.y, collider2d.bounds.size.z));
    }


    private void DirectionChange()
    {
        animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        animator.SetBool("moving", true);

        //Make enemy face direction
        transform.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        transform.position = new Vector3(transform.position.x + Time.deltaTime * _direction * speed,
            transform.position.y, transform.position.z);
    }


    public bool PlayerInCloseRange()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(collider2d.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(collider2d.bounds.size.x * range, collider2d.bounds.size.y, collider2d.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHitGround.collider != null;
    }
}