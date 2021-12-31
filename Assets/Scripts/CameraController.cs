using UnityEngine;

public class CameraController : MonoBehaviour
{
  private float currentPosX;
  [SerializeField] private Transform player;
  [SerializeField] private float aheadDistance;
  [SerializeField] private float yOffset = 0;
  [SerializeField] private float cameraSpeed;
  private float lookAhead;

  private void Update()
  {
    transform.position = new Vector3(player.position.x + lookAhead, player.position.y + yOffset, transform.position.z);
    lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
  }

  public void MoveToNewRoom(Transform _newRoom)
  {
    currentPosX = _newRoom.position.x;
  }


}
