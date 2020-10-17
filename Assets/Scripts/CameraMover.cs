using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    //TODO: incorporate time.deltatime
    [SerializeField] float movementFactor = 60f;
    [SerializeField] float rotationFactor = 60f;
    [SerializeField] float zoomFactor = 60f;

    void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = FindDirection(90);
            transform.position += direction * movementFactor * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 direction = FindDirection(180);
            transform.position += direction * movementFactor * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 direction = FindDirection(270);
            transform.position += direction * movementFactor * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 direction = FindDirection(0);
            transform.position += direction * movementFactor * Time.deltaTime;
        }
    }

    Vector3 FindDirection(float angle)
    {
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
        if (Input.GetAxis("ScrollWheel") > 0)
        {
            transform.position += transform.forward * zoomFactor * Time.deltaTime;
        }
        if (Input.GetAxis("ScrollWheel") < 0)
        {
            transform.position -= transform.forward * zoomFactor * Time.deltaTime;
        }
    }
}
