using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    Vector3 cameraOffset;
    public float cameraSpeed;
    public float cameraRotation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraOffset = player.position - transform.position;
    }

    private void Update()
    {

        Vector3 goalPosition;
        Quaternion goalRotation;
        goalPosition = player.position;
        // goalRotation = Quaternion.LookRotation(player.up, player.forward);
        goalRotation = Quaternion.LookRotation(player.forward, player.up);
        goalPosition -= (transform.rotation * cameraOffset);

        /*transform.position = Vector3.Lerp(transform.position,goalPosition,cameraSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, cameraRotation);*/

        Vector3 goalDirection = (goalPosition - transform.position);
        Vector3 goalOffset = goalDirection * cameraSpeed * Time.deltaTime;

        if(Vector3.Dot((goalPosition-transform.position), goalPosition - (transform.position + goalOffset)) > 0.0f)
        {
            transform.position += goalOffset;
        }
        else
        {
            transform.position = goalOffset;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, cameraRotation * Time.deltaTime);


    }
}
