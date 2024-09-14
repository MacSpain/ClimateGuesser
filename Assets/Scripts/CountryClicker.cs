using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryClicker : MonoBehaviour
{
    public enum Mode
    {
        Visual,
        Data,
        Game
    }

    public Texture2D countryTexture;
    public Color[] texturePixels;
    public int width;
    public int height;
    public Country[] countries;
    public Renderer earthRenderer;


    private LightingSettings lightingSettings;
    private ReadMeanTemperatureFiles reader;
    private Mode currentMode;
    private int dataChoice;

    void Start()
    {
        lightingSettings = FindObjectOfType<LightingSettings>();
        reader = FindObjectOfType<ReadMeanTemperatureFiles>();
        currentMode = Mode.Visual;
        texturePixels = countryTexture.GetPixels();
        width = countryTexture.width;
        height = countryTexture.height;
        earthRenderer.material.SetFloat("_DataStrength", 0.0f);
        earthRenderer.material.SetFloat("_DataSaturation", 1.0f);
        earthRenderer.material.SetFloat("_CloudsOpacity", 0.5f);
        earthRenderer.material.SetFloat("_LightsStrength", 1.0f);
        earthRenderer.material.SetFloat("_Choice", 0.0f);
    }

    public void SetMode(int index)
    {
        currentMode = (Mode)(index);
        switch(currentMode)
        {
            case Mode.Visual:
                {
                    lightingSettings.SetLighting(true);
                    earthRenderer.material.SetFloat("_DataStrength", 0.0f);
                    earthRenderer.material.SetFloat("_DataSaturation", 1.0f);
                    earthRenderer.material.SetFloat("_CloudsOpacity", 0.5f);
                    earthRenderer.material.SetFloat("_LightsStrength", 1.0f);
                }
                break;
            case Mode.Data:
                {
                    lightingSettings.SetLighting(false);
                    earthRenderer.material.SetFloat("_DataStrength", 1.0f);
                    earthRenderer.material.SetFloat("_DataSaturation", 0.0f);
                    earthRenderer.material.SetFloat("_CloudsOpacity", 0.0f);
                    earthRenderer.material.SetFloat("_LightsStrength", 0.0f);
                }
                break;
            case Mode.Game:
                {
                    lightingSettings.SetLighting(false);
                    earthRenderer.material.SetFloat("_DataStrength", 1.0f);
                    earthRenderer.material.SetFloat("_DataSaturation", 0.0f);
                    earthRenderer.material.SetFloat("_CloudsOpacity", 0.0f);
                    earthRenderer.material.SetFloat("_LightsStrength", 0.0f);
                }
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            if(currentMode == Mode.Visual)
            {
                currentMode = Mode.Data;
                lightingSettings.SetLighting(false);
                earthRenderer.material.SetFloat("_DataStrength", 1.0f);
                earthRenderer.material.SetFloat("_DataSaturation", 0.0f);
                earthRenderer.material.SetFloat("_CloudsOpacity", 0.0f);
                earthRenderer.material.SetFloat("_LightsStrength", 0.0f);
            }
            else if(currentMode == Mode.Data)
            {
                currentMode = Mode.Visual;
                lightingSettings.SetLighting(true);
                earthRenderer.material.SetFloat("_DataStrength", 0.0f);
                earthRenderer.material.SetFloat("_DataSaturation", 1.0f);
                earthRenderer.material.SetFloat("_CloudsOpacity", 0.5f);
                earthRenderer.material.SetFloat("_LightsStrength", 1.0f);

            }
        }
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            earthRenderer.material.SetFloat("_Choice", 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            earthRenderer.material.SetFloat("_Choice", 1.0f);
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            earthRenderer.material.SetFloat("_Choice", 2.0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
        {
            reader.SetIndexLower();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) == true)
        {
            reader.SetIndexHigher();
        }
        if (currentMode == Mode.Data)
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
                    earthRenderer.material.SetFloat("_CountryIndex", redValue);

                }
            }
        }
        else if(currentMode == Mode.Game)
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
