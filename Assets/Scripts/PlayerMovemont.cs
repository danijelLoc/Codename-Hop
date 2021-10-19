using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerMovemont : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        checkHorizontalShift();
        checkJump();
    }

    private void checkHorizontalShift() {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        checkHorizontalSpriteFlip(horizontalInput);
    }

    private void checkHorizontalSpriteFlip(float horizontalInput) {
        if (horizontalInput >= 0.1f)
            transform.localScale = Vector3.one;
        else if (horizontalInput <= -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void checkJump() {
        if (Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, speed / 2);
        }
    }

}
