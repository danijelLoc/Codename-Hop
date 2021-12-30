using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private LayerMask groundLayer;
    [SerializeReference] private Transform pointOfNoReturn;
    public float currentHealth { get; private set; }
    private Animator anim;
    private Collider2D collider2d;
    private EnemyPatrol patrol;
    private Senses senses;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
        patrol = GetComponent<EnemyPatrol>();
        senses = GetComponent<Senses>();
    }

    private void Update()
    {
        if (transform.localPosition.y < pointOfNoReturn.localPosition.y)
            HasFallenFromPlatform();
        if (dead)
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
            bool onTheGround = raycastHit.collider != null;
            if (onTheGround)
                Invoke(nameof(DisableRigidBody), 1f);
        }
    }

    public void TakeDamage(float damage)
    {
        // No demage if subject is on guard
        if (anim.GetBool("onGuard") && senses.ThreatInSight())
        {
            if (patrol != null)
                patrol.OnAttack();
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("getHurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                dead = true;
            }
        }
    }

    public void HasFallenFromPlatform()
    {
        currentHealth = 0;
        dead = true;
        this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        this.gameObject.SetActive(false);
        if (this.gameObject.tag == "Player")
            PauseMenu.GameFailed = true;
    }

    public bool isDead()
    {
        return dead;
    }

    public void AddHealth(float healthValue)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthValue, 0, startingHealth);
    }

    public float getStartingHealth()
    {
        return startingHealth;
    }

    private void DisableRigidBody()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        if (this.gameObject.tag == "Player")
            PauseMenu.GameFailed = true;
    }
}