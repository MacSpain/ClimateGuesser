using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public float minZoom = 1.0f;
    public float maxZoom = 2.0f;

    public float cameraSpeed = 1.0f;

    private float currentZoom = 1.0f;
    private float currentXAngle = 0.0f;
    private float currentYAngle = 0.0f;
    private Vector3 oldMousePosition = Vector3.zero;
    private bool rightPressed = false;

    void Start()
    {
        currentZoom = minZoom + 0.5f*(maxZoom - minZoom);
        oldMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if(Input.GetMouseButton(1) == true)
        {
            if (rightPressed == false)
            {
                rightPressed = true;
                oldMousePosition = Input.mousePosition;
            }
            else
            {

                Vector3 newMousePosition = Input.mousePosition;
                Vector3 diff = newMousePosition - oldMousePosition;
                currentYAngle += cameraSpeed*diff.x;
                if (currentYAngle > 360.0f)
                {
                    currentYAngle -= 360.0f;
                }
                currentXAngle += cameraSpeed * diff.y;
                if (currentXAngle > 89.0f)
                {
                    currentXAngle = 89.0f;
                }
                if (currentXAngle < -89.0f)
                {
                    currentXAngle = -89.0f;
                }
                oldMousePosition = newMousePosition;
            }
        }
        else
        {
            rightPressed = false;
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            float delta = Input.mouseScrollDelta.y;
            currentZoom -= delta * zoomSpeed;
            if(currentZoom < minZoom)
            {
                currentZoom = minZoom;
            }
            if(currentZoom > maxZoom)
            {
                currentZoom = maxZoom;
            }
        }

        Vector3 currentVector = new Vector3(0.0f, 
            -Mathf.Sin(Mathf.Deg2Rad * currentXAngle), 
            Mathf.Cos(Mathf.Deg2Rad * currentXAngle));
        currentVector = new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentYAngle) * currentVector.z,
            currentVector.y,
            Mathf.Cos(Mathf.Deg2Rad * currentYAngle)* currentVector.z);

        transform.position = currentZoom * currentVector;
        transform.rotation = Quaternion.LookRotation(-currentVector.normalized);
    }
}
