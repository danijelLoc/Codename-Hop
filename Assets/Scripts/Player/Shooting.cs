using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float shootingCooldown;
    [SerializeField] private int initialNumberOfBullets;
    [SerializeField] private Transform shootingPointStraight;
    [SerializeField] private Transform shootingPointUp;
    [SerializeField] private Transform shootingPointDown;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private AudioClip gunShotSound;
    private Animator animator;
    private float cooldownTimer = Mathf.Infinity;
    public int numberOfBullets { get; private set; }

    private Direction shootingDirection = Direction.straight;

    enum Direction
    {
        up,
        straight,
        down
    }


    // Start is called before the first frame update
    private void Awake()
    {
        numberOfBullets = initialNumberOfBullets;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public void Shoot()
    {
        if (cooldownTimer < shootingCooldown) return;
        shootingDirection = Direction.straight;
        animator.SetTrigger("shoot");
        cooldownTimer = 0f;
    }

    public void ShootUp()
    {
        if (cooldownTimer < shootingCooldown) return;
        shootingDirection = Direction.up;
        animator.SetTrigger("shootUp");
        cooldownTimer = 0f;
    }

    public void ShootDown()
    {
        if (cooldownTimer < shootingCooldown) return;
        shootingDirection = Direction.down;
        animator.SetTrigger("shootDown");
        cooldownTimer = 0f;
    }

    public void PullTheTrigger()
    {
        if (numberOfBullets == 0) return;
        SoundManager.instance.PlaySound(gunShotSound);
        numberOfBullets--;
        switch (shootingDirection)
        {
            case Direction.straight:
                {
                    bullets[FindBullet()].transform.position = shootingPointStraight.position;
                    var direction = new Vector2(Mathf.Sign(transform.localScale.x), 0);
                    bullets[FindBullet()].GetComponent<Projectile>().SetDirection(direction);
                    break;
                }
            case Direction.up:
                {
                    bullets[FindBullet()].transform.position = shootingPointUp.position;
                    var direction = new Vector2(0, 1);
                    bullets[FindBullet()].GetComponent<Projectile>().SetDirection(direction);
                    break;
                }
            case Direction.down:
                {
                    bullets[FindBullet()].transform.position = shootingPointDown.position;
                    var direction = new Vector2(0, -1);
                    bullets[FindBullet()].GetComponent<Projectile>().SetDirection(direction);
                    break;
                }
        }
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
