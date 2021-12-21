using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    [SerializeField] private float swordAttackCooldown;
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
        else animator.SetTrigger("attackWithSword");
        cooldownTimer = 0;

        //TODO Ako je zamah u prazno onda ovo, ako pogodi nešto ili nekog onda drugi zvuk
        SoundManager.instance.PlaySound(swordAirSound);
    }
}
