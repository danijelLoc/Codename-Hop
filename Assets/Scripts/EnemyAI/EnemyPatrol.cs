using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft = true;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    [SerializeField] private float guardDuration;
    [SerializeField] private float reactionTime;
    private float idleTimer = 0;
    private float guardTimer = 0;
    private float reactionTimer = 0;

    private bool _takeGuard = false;
    private bool _reactOnAttack = false;

    private Animator animator;
    private Senses senses;

    private bool AnimatorGroundedState
    {
        get { return animator.GetBool("isGrounded"); }
        set { animator.SetBool("isGrounded", value); }
    }

    private bool AnimatorOnGuardState
    {
        get { return animator.GetBool("onGuard"); }
        set { animator.SetBool("onGuard", value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        senses = GetComponent<Senses>();
        initScale = transform.localScale;
    }

    private void OnDisable()
    {
        animator.SetBool("moving", false);
    }

    private void Update()
    {
        AnimatorGroundedState = senses.IsGrounded();
        if (_reactOnAttack)
        {

        }
        if (_takeGuard)
        {
            guardTimer += Time.deltaTime;
            if (guardTimer >= guardDuration)
            {
                _takeGuard = false;
                AnimatorOnGuardState = false;
                guardTimer = 0;

            }

            else
                return;
        }
        if (!senses.ThreatInCloseRange())
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

    public void OnAttack()
    {
        _reactOnAttack = true;
        if (!senses.ThreatInSight())
        {
            movingLeft = !movingLeft;
        }
        else
        {
            guardTimer = 0;
            animator.SetBool("moving", false);
            if (!AnimatorOnGuardState)
                animator.SetTrigger("takeGuard");
            _takeGuard = true;
        }
    }

    private void LowerGuard()
    {
        AnimatorOnGuardState = false;
    }

    public void SetOnGuard()
    {
        AnimatorOnGuardState = true;
    }
}