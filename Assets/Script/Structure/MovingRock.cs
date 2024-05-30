using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingRock : MonoBehaviour
{
    public Transform transformRock;
    public Vector3 destination;
    public Vector3 nowPosition;
    public float speed = 1.0f;
    private bool movingToDestination = true;
    public Rigidbody _rigidBody;
    void Awake()
    {
        transformRock = GetComponent<Transform>();
        nowPosition = transformRock.position;
        destination = nowPosition + new Vector3(10, 0, 10); 
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = movingToDestination ? destination : nowPosition;
        _rigidBody.MovePosition(Vector3.MoveTowards(transformRock.position, targetPosition, speed * Time.deltaTime));
        //if Object's transfrom moves by position, cant't give velocity to player
        //transform.position = new Vector3(transform.position.x,2, transform.position.z);
        //transformRock.position = Vector3.MoveTowards(transformRock.position, targetPosition, speed * Time.deltaTime);

        //if Object's transfrom moves by rigidbody's velocity, can't active isKinematic and Object will be disturbed by player,enemy
        //_rigidBody.velocity = targetPosition.normalized*speed;
        if (Vector3.Distance(transformRock.position, targetPosition) < 0.1f)
        {
            movingToDestination = !movingToDestination;
        }
    }
}
