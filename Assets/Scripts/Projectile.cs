using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float demage;
    private Vector2 direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private Collider2D collider2d;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (hit)
        {
            Deactivate();
            return;
        }
        Vector2 movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed.x, movementSpeed.y, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        Health casualty = collision.gameObject.GetComponent<Health>();
        if (casualty != null)
            casualty.TakeDamage(demage);
        collider2d.enabled = false;
    }
    public void SetDirection(Vector2 _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        collider2d.enabled = true;
        float rotationZ = direction.y > 0 ? 90 : direction.y < 0 ? -90 : 0;
        Debug.Log(rotationZ);
        float localScaleX = Mathf.Sign(transform.localScale.x) != _direction.x ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        // Rotate only sprite, cant figure it out, using lead ball to skip it.
        // transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, rotationZ, 0f);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}