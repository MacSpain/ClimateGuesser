using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryClicker : MonoBehaviour
{
    public Texture2D countryTexture;
    public Color[] texturePixels;
    public int width;
    public int height;
    public Country[] countries;
    public Renderer earthRenderer;


    private LightingSettings lightingSettings;
    private bool dataMode;

    void Start()
    {
        lightingSettings = FindObjectOfType<LightingSettings>();
        dataMode = false;
        texturePixels = countryTexture.GetPixels();
        width = countryTexture.width;
        height = countryTexture.height;
        earthRenderer.material.SetFloat("_DataStrength", 0.0f);
        earthRenderer.material.SetFloat("_CloudsOpacity", 0.5f);
        earthRenderer.material.SetFloat("_LightsStrength", 1.0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            dataMode = !dataMode;
            if(dataMode)
            {
                lightingSettings.SetLighting(false);
                earthRenderer.material.SetFloat("_DataStrength", 1.0f);
                earthRenderer.material.SetFloat("_CloudsOpacity", 0.0f);
                earthRenderer.material.SetFloat("_LightsStrength", 0.0f);
            }
            else
            {
                lightingSettings.SetLighting(true);
                earthRenderer.material.SetFloat("_DataStrength", 0.0f);
                earthRenderer.material.SetFloat("_CloudsOpacity", 0.5f);
                earthRenderer.material.SetFloat("_LightsStrength", 1.0f);

            }
        }
        if (dataMode == true)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100000.0f, -1) == true)
                {

                    Vector3 hitPoint = hit.point.normalized;
                    float longitude = Mathf.Atan2(hitPoint.z, hitPoint.x);
                    if (longitude < 0)
                    {
                        longitude = 2.0f * Mathf.PI + longitude;
                    }
                    float latitude = Mathf.PI - Mathf.Acos(hitPoint.y);

                    int pointX = (int)((longitude / (2.0f * Mathf.PI)) * width);
                    int pointY = (int)((latitude / (Mathf.PI)) * height);
                    float redValue = texturePixels[pointY * width + pointX].r;
                    if (redValue > 0.0f)
                    {
                        float val = 255.0f * redValue - 1.0f;

                        int countryIndex = (int)val;
                    }

                }
            }
        }
    }
}
