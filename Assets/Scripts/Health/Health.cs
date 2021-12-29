using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private LayerMask groundLayer;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private bool rigidBodyDisabled;
    private Collider2D collider2d;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            TakeDamage(1);
        if (dead)
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
            bool onTheGround = raycastHit.collider != null;
            if (onTheGround)
                Invoke(nameof(DisableRigidBody), 1f);
        }
    }

    public void TakeDamage(float _damage)
    {
        if (anim.GetBool("onGuard"))
            return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

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

    public bool isDead() 
    {
        return dead;
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public float getStartingHealth() 
    {
        return startingHealth;
    }

    private void DisableRigidBody()
    {
        GetComponent<Rigidbody2D>().simulated = false; 
        rigidBodyDisabled = true;
    }
}