using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GrappleState
{
  ready,
  isFiring
}

public class GrapplingGun : MonoBehaviour
{
  [Header("General Refernces:")]
  [SerializeField] private Camera mainCamera;

  [SerializeField] private Transform holder;
  [SerializeField] public Transform firePoint;
  [SerializeField] private Rope grappleRope;

  [Header("Targeting settings:")]
  [SerializeField] private float autoTargetRadius = 4;
  [SerializeField] private float maxRange = 4;
  [SerializeField] private LayerMask grapplePointLayer;

  [Header("Motion settings:")]
  [SerializeField] private float pullSpeed = 3;

  private GrappleState grappleState = GrappleState.ready;

  [HideInInspector]
  public GameObject targetGrapplePoint;
  [HideInInspector]
  public GameObject currentGrapplePoint;
  [HideInInspector]
  public Vector2 grappleDistanceVector;

  private Rigidbody2D holderRigidBody2D;

  // Start is called before the first frame update
  void Start()
  {
    holderRigidBody2D = holder.GetComponent<Rigidbody2D>();
    grappleDistanceVector = Vector2.zero;
    grappleRope.enabled = false;
  }

  // Update is called once per frame
  void Update()
  {
    targetGrapplePoint = getNearestGrapplePoint();

    // Fire Grappling Gun
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      if (targetGrapplePoint != null && grappleState == GrappleState.ready)
      {
        // grappleState = GrappleState.isFiring;
        currentGrapplePoint = targetGrapplePoint;
        grappleDistanceVector = currentGrapplePoint.transform.position - holder.transform.position;
        grappleRope.enabled = true;
        Debug.Log("Fire!");
      }
      else
      {
        Debug.Log("No Target!");
      }
    }

    // Get pulled by Grappling Gun
    if (Input.GetKey(KeyCode.Mouse0) && grappleState == GrappleState.isFiring)
    {
      grappleDistanceVector = currentGrapplePoint.transform.position - holder.transform.position;
      holderRigidBody2D.velocity = grappleDistanceVector.normalized * pullSpeed;
    }

    // Release Grappling Gun
    if (Input.GetKeyUp(KeyCode.Mouse0))
    {
      grappleState = GrappleState.ready;
      grappleRope.enabled = false;
      // TODO let go
      Debug.Log("Let go");
      currentGrapplePoint = null;
      holderRigidBody2D.velocity = new Vector2(holderRigidBody2D.velocity.x, 0);
    }
  }

  private GameObject getNearestGrapplePoint()
  {
    var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    var mouseDistance = Vector2.Distance(holder.transform.position, mousePosition);
    var clampedTargetPosition = new Vector2(holder.transform.position.x, holder.transform.position.y) + Vector2.ClampMagnitude(mousePosition - holder.transform.position, maxRange);
    var objects = Physics2D.OverlapCircleAll(clampedTargetPosition, autoTargetRadius);

    GameObject result = null;
    foreach (var target in objects)
    {
      if (!target.GetComponent<GrapplePoint>()) continue;

      var targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);

      var losDirection = targetPosition - new Vector2(holder.transform.position.x, holder.transform.position.y);
      RaycastHit2D losTest = Physics2D.Raycast(holder.transform.position, losDirection, maxRange, grapplePointLayer);
      if (!losTest.collider || !losTest.collider.transform.position.Equals(targetPosition)) continue;

      if (result is null
          || Vector2.Distance(clampedTargetPosition, result.transform.position) > Vector2.Distance(clampedTargetPosition, targetPosition))
      {
        result = target.gameObject;
      }
    }
    return result;
  }

  public void Grapple()
  {
    grappleState = GrappleState.isFiring;
  }

  private void OnDrawGizmosSelected()
  {
    // grappling hook range
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(holder.transform.position, maxRange);

    // auto targeting radius
    var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    var mouseDistance = Vector2.Distance(holder.transform.position, mousePosition);
    var clampedTargetPosition = new Vector2(holder.transform.position.x, holder.transform.position.y) + Vector2.ClampMagnitude(mousePosition - holder.transform.position, maxRange);
    Gizmos.DrawWireSphere(clampedTargetPosition, autoTargetRadius);
  }
}
