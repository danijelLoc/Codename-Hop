using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootingCooldown;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject[] bullets;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private AudioClip gunShotSound;

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
        if (Input.GetKeyDown(KeyCode.F) && cooldownTimer > shootingCooldown && playerMovement.canShoot())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("shoot");
        cooldownTimer = 0;
        SoundManager.instance.PlaySound(gunShotSound);
    }

    private void PullTheTrigger()
    {
        bullets[FindBullet()].transform.position = shootingPoint.position;
        bullets[FindBullet()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
