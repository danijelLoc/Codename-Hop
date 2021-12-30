using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [Header("Platform Points")]
    [SerializeField] private Transform upperPoint;
    [SerializeField] private Transform lowerPoint;

    [Header("Movement parameters")]
    [SerializeField] private float speed;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;

    private bool goingUp = true;
    private float idleTimer;

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            if (transform.position.y <= upperPoint.position.y)
                MoveInDirection(1);
            else
                DirectionChange();
        }
        else
        {
            if (transform.position.y >= lowerPoint.position.y)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            goingUp = !goingUp;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        transform.position = new Vector3(transform.position.x,
            transform.position.y + Time.deltaTime * _direction * speed, transform.position.z);
    }
}
