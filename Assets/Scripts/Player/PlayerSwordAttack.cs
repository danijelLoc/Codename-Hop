using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private int damage;
    [SerializeField] private float swordAttackCooldown;
    [SerializeField] private AudioClip swordAirSound;

    private Health enemyHealth;
    private Animator animator;
    private Senses senses;
    private PlayerMovement playerMovement;

    private float cooldownTimer = Mathf.Infinity;
    private Direction attackDirection = Direction.straight;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        senses = GetComponent<Senses>();
    }

    // Update is called once per frame
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G) && cooldownTimer > swordAttackCooldown && playerMovement.CanAttackWithSword())
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.S))
        {
            attackDirection = Direction.down;
            animator.SetTrigger("attackDownWithSword");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            attackDirection = Direction.up;
            animator.SetTrigger("attackUpWithSword");
        }
        else
        {
            attackDirection = Direction.straight;
            animator.SetTrigger("attackWithSword");
        }

        cooldownTimer = 0;

        //TODO Ako je zamah u prazno onda ovo, ako pogodi nesto ili nekog onda drugi zvuk
        SoundManager.instance.PlaySound(swordAirSound);
    }

    private void DamageEnemy()
    {
        var hit = senses.GetHitInCloseRange(attackDirection);
        if (hit.collider != null)
        {
            enemyHealth = hit.transform.GetComponent<Health>();
            enemyHealth.TakeDamage(damage);
        }

    }
}
