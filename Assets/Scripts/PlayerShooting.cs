using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject[] bullets;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

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
        if (Input.GetKeyDown(KeyCode.F) && cooldownTimer > attackCooldown && playerMovement.canShoot())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("shoot");
        cooldownTimer = 0;
    }

    private void PullTheTrigger() 
    {
        bullets[0].transform.position = shootingPoint.position;
        bullets[0].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

    }
}
