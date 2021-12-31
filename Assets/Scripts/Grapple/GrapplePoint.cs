using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
  [SerializeField] private GrapplingGun gun;
  private bool isTargeted = false;
  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    isTargeted = gun.targetGrapplePoint != null && gun.targetGrapplePoint.Equals(this.gameObject);

    this.GetComponent<SpriteRenderer>().color = isTargeted ? Color.blue : Color.white;
  }
}
