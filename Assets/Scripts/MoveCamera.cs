using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // serialized variables for this script to be set in the Unity UI
    [SerializeField] Transform centerObject;
    [SerializeField] float speed = 2;
    [SerializeField] float turnSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        // move our center position
        float xPos = (Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        float yPos = (Input.GetAxis("Vertical") * speed * Time.deltaTime);

        transform.Translate(xPos, yPos, 0);

        // roatate around the object in the center
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(centerObject.position, Vector3.up, Input.GetAxis("Mouse X") * turnSpeed);
            transform.RotateAround(centerObject.position, Vector3.left, Input.GetAxis("Mouse Y") * turnSpeed);
        }

    } // end Update
}
