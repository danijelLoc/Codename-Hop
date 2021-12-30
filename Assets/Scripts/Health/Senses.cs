using System.Collections;
using UnityEngine;


public class Senses : MonoBehaviour
{
    [Header("Range Parameters")]
    [SerializeField] private float closeRange;
    [SerializeField] private float eyeRange;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistanceCloseRange;
    [SerializeField] private float colliderDistanceEyeRange;
    [SerializeReference] private Collider2D collider2d;

    [Header("Layers")]
    [SerializeField] private LayerMask threatLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    public bool ThreatInCloseRange()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(collider2d.bounds.center + transform.right * closeRange * transform.localScale.x * colliderDistanceCloseRange,
            new Vector3(collider2d.bounds.size.x * closeRange, collider2d.bounds.size.y, collider2d.bounds.size.z),
            0, Vector2.left, 0, threatLayer);

        return hit.collider != null;
    }

    public bool ThreatInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(collider2d.bounds.center + transform.right * eyeRange * transform.localScale.x * colliderDistanceEyeRange,
            new Vector3(collider2d.bounds.size.x * eyeRange, collider2d.bounds.size.y, collider2d.bounds.size.z),
            0, Vector2.left, 0, threatLayer);

        return hit.collider != null;
    }

    public RaycastHit2D GetHitInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(collider2d.bounds.center + transform.right * eyeRange * transform.localScale.x * colliderDistanceEyeRange,
            new Vector3(collider2d.bounds.size.x * eyeRange, collider2d.bounds.size.y, collider2d.bounds.size.z),
            0, Vector2.left, 0, threatLayer);

        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider2d.bounds.center + transform.right * closeRange * transform.localScale.x * colliderDistanceCloseRange,
            new Vector3(collider2d.bounds.size.x * closeRange, collider2d.bounds.size.y, collider2d.bounds.size.z));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(collider2d.bounds.center + transform.right * eyeRange * transform.localScale.x * colliderDistanceEyeRange,
           new Vector3(collider2d.bounds.size.x * eyeRange, collider2d.bounds.size.y, collider2d.bounds.size.z));
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D raycastHitOnThreat = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, Vector2.down, 0.1f, threatLayer);
        return raycastHitGround.collider != null || raycastHitOnThreat.collider != null;
    }

    public bool IsOnTheWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider2d.bounds.center, collider2d.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.01f, wallLayer);
        return raycastHit.collider != null;
    }
}