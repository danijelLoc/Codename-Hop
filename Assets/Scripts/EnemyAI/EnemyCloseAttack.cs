using UnityEngine;

public class EnemyCloseAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeReference] private Health playerHealth;

    //References
    private Animator anim;
    private Health health;
    private EnemyPatrol patrol;

    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrol = GetComponentInParent<EnemyPatrol>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (patrol.PlayerInCloseRange())
        {
            Attack();
        }
        if (health.isDead())
        {
            patrol.enabled = false;
            this.enabled = false;
        }
    }

    public void Attack()
    {
        if (cooldownTimer >= attackCooldown)
        {
            anim.SetTrigger("closeAttack");
            cooldownTimer = 0;
        }
    }

    private void DamagePlayer()
    {
        if (patrol.PlayerInCloseRange())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}