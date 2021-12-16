using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "piratePRO") {

            if (collision.transform.position.x < transform.position.x) {
                camera.MoveToNewRoom(nextRoom);
            }
            else
            {
                camera.MoveToNewRoom(previousRoom);
            }
        }
    }
}
