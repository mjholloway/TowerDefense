﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] float movementFactor = 60f;
    [SerializeField] float rotationFactor = 60f;
    [SerializeField] float zoomFactor = 60f;

    void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    //This will call the FindDirection function along with the adjusted angle to perform calculations. It should be noted that the directions have to be adjusted 
    //by 90 degrees since the 0 degree rotation is forward, while polar coordinates generally consider the 0 degree rotation to be to the "right". After the
    //direction vector is calculated it is multipled by some factor (which can be changed in the editor) and added to the current position to move the camera.
    private void MoveCamera()
    {
        Vector3 direction = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            direction += FindDirection(90);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += FindDirection(180);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += FindDirection(270);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += FindDirection(0);
        }
        //This clamps the camera and prevents it from moving too far in any direction.
        Vector3 newPos = new Vector3(
                Mathf.Clamp(transform.position.x + direction.x * movementFactor * Time.deltaTime, -100f, 140f),
                transform.position.y,
                Mathf.Clamp(transform.position.z + direction.z * movementFactor * Time.deltaTime, -140f, 25f));
        transform.position = newPos;
    }

    Vector3 FindDirection(float angle)
    {
        //This takes the negative rotation of the camera (the direction of rotation is the opposite of what normal polar coordinates would consider positive)
        //and adjusts it based on the key pressed, then converts it to radians. It then uses basic polar equations to determine how much the x and z coordinates
        //should be incremented by each frame. This assumes a unit circle. It is also worth noting that the typical cartesian y axis is actually the z axis since 
        //we are rotating around the y axis.
        float radAngle = (-transform.rotation.eulerAngles.y + angle) * Mathf.PI / 180;
        float z = Mathf.Sin(radAngle);
        float x = Mathf.Cos(radAngle);
        return new Vector3(x, 0f, z);
    }

    //Raycast from the camera to determine the point that the camera should rotate around, then rotate around that point. This means the camera will rotate
    //correctly even if it has been moved from its starting position.
    private void RotateCamera()
    {   
        if (Input.GetKey(KeyCode.Q))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 300f, 1 << 10);
            transform.RotateAround(hit.point, Vector3.up, rotationFactor * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 300f, 1 << 10);
            transform.RotateAround(hit.point, Vector3.up, -rotationFactor * Time.deltaTime);
        }
    }

    private void ZoomCamera()
    {
        if (Input.GetAxis("ScrollWheel") > 0 && transform.position.y > 15)
        {
            transform.position += transform.forward * zoomFactor * Time.deltaTime;
        }
        if (Input.GetAxis("ScrollWheel") < 0 && transform.position.y < 125)
        {
            transform.position -= transform.forward * zoomFactor * Time.deltaTime;
        }
    }
}
