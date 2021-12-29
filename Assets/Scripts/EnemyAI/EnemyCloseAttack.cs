using UnityEngine;

public class EnemyCloseAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private Collider2D collider2d;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    //References
    private Animator anim;
    private Health playerHealth;
    private Health enemyHealth;
    private EnemyPatrol enemyPatrol;

    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        enemyHealth = GetComponent<Health>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (enemyHealth.isDead())
        {
            enemyPatrol.enabled = false;
            this.enabled = false;
            return;
        }
        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("closeAttack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider2d.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(collider2d.bounds.size.x * range, collider2d.bounds.size.y, collider2d.bounds.size.z));
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(collider2d.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(collider2d.bounds.size.x * range, collider2d.bounds.size.y, collider2d.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}