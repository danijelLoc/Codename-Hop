using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float swordAttackCooldown;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;


    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;

    private Health enemyHealth;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private AudioClip swordAirSound;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G) && cooldownTimer > swordAttackCooldown && playerMovement.canAttackWithSword())
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.S)) animator.SetTrigger("attackDownWithSword");
        else if (Input.GetKey(KeyCode.W)) animator.SetTrigger("attackUpWithSword");
        else
        {
            DamageEnemy();
            animator.SetTrigger("attackWithSword");
        }
            
        cooldownTimer = 0;

        //TODO Ako je zamah u prazno onda ovo, ako pogodi nesto ili nekog onda drugi zvuk
        SoundManager.instance.PlaySound(swordAirSound);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void DamageEnemy()
    {
        if (EnemyInSight()) 
        {
            enemyHealth.TakeDamage(damage);
        }
            
    }
}
