using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravity;
    Vector3 gravityVector;
    public Transform Planet;
    public float speed;
    public float rotateSpeed;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded;
    private bool insidePlanet=false;
    public float jumpValue;
    public float gravityRotation;

    Vector3 CameramoveDirection;
    public float camerashakeDuration;
    public float camerashakeMagnitude;
    bool isCameraShaking;

    private void Update()
    {
       
        CameramoveDirection = (Camera.main.transform.forward * (-Input.GetAxis("Joystick Vertical")) +Camera.main.transform.right * (Input.GetAxis("Joystick Horizontal")*0.5f));

        if (Input.GetAxis("Joystick Vertical") > 0 && Input.GetAxis("Joystick Vertical") <= 1) 
        {
           // CameramoveDirection = Vector3.zero;
        }
        gravityVector = (Planet.position - transform.position).normalized * gravity * Time.deltaTime;
        if (insidePlanet)
        {
            StayOnPlanet();
        }
        Vector3 RightVector = Vector3.Cross(CameramoveDirection, gravityVector);
        Vector3 PlayerForwardVector = Vector3.Cross(gravityVector, RightVector).normalized;
        Debug.Log(PlayerForwardVector);
         transform.position +=  PlayerForwardVector* speed * Time.fixedDeltaTime;
        // transform.position += transform.up * -Input.GetAxis("Joystick Vertical") * 20f * Time.deltaTime;
        //transform.Rotate(new Vector3(0f, 0f, Input.GetAxis("Joystick Horizontal") * rotateSpeed * Time.deltaTime));

        if (Input.GetButton("Joystick Jump") && isGrounded)
        {
            isGrounded = false;
            Vector3 jumpDirection = (transform.position - Planet.position).normalized;
            velocity += jumpDirection * jumpValue;
        }
        velocity += gravityVector;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-gravityVector, transform.up), gravityRotation * Time.deltaTime);

       Quaternion rot;
        if (CameramoveDirection.sqrMagnitude > 0.01f)
        {
            rot = Quaternion.LookRotation(PlayerForwardVector, -gravityVector);
        }
        else
        {
            rot = Quaternion.LookRotation(transform.forward, -gravityVector);
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, gravityRotation * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;


    }
  

    private void LateUpdate()
    {
        if (isCameraShaking)
        {
           // Camera.main.transform.position += Random.insideUnitSphere * camerashakeMagnitude;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Planet")
        {
            insidePlanet = true;
            isCameraShaking = true;
            Invoke("CameraShake", camerashakeDuration);
        }
        if (other.tag == "FauxGravity")
        {
            Planet = other.transform.parent;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Planet")
        {
            insidePlanet = true;        }
    }


    void CameraShake()
    {
        isCameraShaking = false;
    }

    void StayOnPlanet()
    {
        isGrounded = true;
        velocity = Vector3.zero;
        transform.position = Planet.position - gravityVector.normalized * Planet.transform.localScale.y * 0.5f;
        insidePlanet = false;
    }
}
