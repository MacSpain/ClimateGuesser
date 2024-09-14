using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSettings : MonoBehaviour
{
    [SerializeField]
    private Light sun;
    [SerializeField]
    private float sunSpeed;
    [SerializeField, Range(-60.0f, 60.0f)]
    private float sunHeight;

    private bool naturalLighting = true;
    private float currentAngle;
    private Transform sunTransform;

    private void Start()
    {
        currentAngle = 0.0f;
        naturalLighting = true;
        sun.enabled = true;
        sunTransform = sun.gameObject.transform;
        RenderSettings.ambientLight = Color.black;
    }

    public void SetLighting(bool natural)
    {
        naturalLighting = natural;

        if (naturalLighting == true)
        {

            sun.enabled = true;
            RenderSettings.ambientLight = Color.black;
        }
        else
        {

            sun.enabled = false;
            RenderSettings.ambientLight = Color.white;
        }
    }

    void Update()
    {
        currentAngle += sunSpeed * Time.deltaTime;
        if(currentAngle > 360.0f)
        {
            currentAngle -= 360.0f;
        }
        Vector3 currentSunEulers = sunTransform.localRotation.eulerAngles;
        currentSunEulers.y = currentAngle;
        currentSunEulers.x = sunHeight;
        sunTransform.localRotation = Quaternion.Euler(currentSunEulers);
    }
}
