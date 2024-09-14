using System;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ReadMeanTemperatureFiles))]
public class ReadMeanTemperatureFilesEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ReadMeanTemperatureFiles myTarget = (ReadMeanTemperatureFiles)target;
        if (GUILayout.Button("Read Files"))
        {
            myTarget.ReadFiles();
        }
        if (GUILayout.Button("Create Gradients"))
        {
            myTarget.CreateGradients();
        }
    }
}

public class ReadMeanTemperatureFiles : MonoBehaviour
{
    public enum DataTypes
    {
        Time,
        Latitude,
        Longitude,
        TwoMetersTemperature,
        TotalPrecipitation,
        TotalCloudCover,
        LakeIceDepth,
        ConvectivePrecipitation,
        LeafAreaIndexHigh,
        LeafAreaIndexLow,
        Snowfall,
        SnowDepth,
        VolumetricSoilWaterLayer1,
        VolumetricSoilWaterLayer2,

        Count
    }

    [System.Serializable]
    public struct DataParams
    {
        public double minValue;
        public double maxValue;
    }
    [System.Serializable]
    public struct TextureArraysArray
    {
        public Texture2DArray oldDataTextureArray;
        public Texture2DArray newDataTextureArray;
    }

    public string[] filePaths = new string[(int)DataTypes.Count];
    public string outputPath;
    public Renderer earthRenderer;
    public TextureArraysArray[] textureArraysArray;
    public Gradient[] gradients;
    public Gradient[] comparisonGradients;
    public Texture2D[] gradientTextures;
    public Texture2D[] comparisonGradientTextures;
    public DataParams[] dataParams;

    private double[][] fileData;
    private Texture2D texture;
    private int longitudeCount;
    private int latitudeCount;
    private int timeCount;
    private float[] data;

    private float t;
    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
        earthRenderer.material.SetTexture("_OldDataTextureArray", textureArraysArray[currentIndex].oldDataTextureArray);
        earthRenderer.material.SetTexture("_NewDataTextureArray", textureArraysArray[currentIndex].newDataTextureArray);
        earthRenderer.material.SetTexture("_DataGradientTexture", gradientTextures[currentIndex]);
        earthRenderer.material.SetTexture("_ComparisonDataGradientTexture", comparisonGradientTextures[currentIndex]);
        t = Time.time;
    }

    private void Update()
    {
        t += Time.deltaTime;
        if(t > 12.0f)
        {
            t -= 12.0f;
            currentIndex = (currentIndex + 1) % textureArraysArray.Length;
        }

    }
    public void SetIndex(int index)
    {
        currentIndex = index;

        earthRenderer.material.SetTexture("_OldDataTextureArray", textureArraysArray[currentIndex].oldDataTextureArray);
        earthRenderer.material.SetTexture("_NewDataTextureArray", textureArraysArray[currentIndex].newDataTextureArray);
        earthRenderer.material.SetTexture("_DataGradientTexture", gradientTextures[currentIndex]);
        earthRenderer.material.SetTexture("_ComparisonDataGradientTexture", comparisonGradientTextures[currentIndex]);
    }

    public void CreateGradients()
    {
        Color[] textureBuffer = new Color[256];
        float div = 1.0f / 255.0f;

        for (int i = 0; i < gradients.Length; ++i)
        {
            float currentT = 0.0f;
            Texture2D gradientTexture = new Texture2D(256, 1, TextureFormat.RGBA32, false);
            for(int j = 0; j < 256; ++j)
            {
                Color newColor = gradients[i].Evaluate(currentT);
                textureBuffer[j] = newColor;
                currentT += div;
            }
            gradientTexture.SetPixels(textureBuffer);
            byte[] bytesJPG = gradientTexture.EncodeToPNG();
            string filePath = (Application.dataPath + outputPath + i.ToString() + ".png");
            File.WriteAllBytes(filePath, bytesJPG);
        }
        for (int i = 0; i < comparisonGradients.Length; ++i)
        {
            float currentT = 0.0f;
            Texture2D gradientTexture = new Texture2D(256, 1, TextureFormat.RGBA32, false);
            for (int j = 0; j < 256; ++j)
            {
                Color newColor = comparisonGradients[i].Evaluate(currentT);
                textureBuffer[j] = newColor;
                currentT += div;
            }
            gradientTexture.SetPixels(textureBuffer);
            byte[] bytesJPG = gradientTexture.EncodeToPNG();
            string filePath = (Application.dataPath + outputPath + "comp" + i.ToString() + ".png");
            File.WriteAllBytes(filePath, bytesJPG);
        }
    }

    public void ReadFiles()
    {
        fileData = new double[(int)DataTypes.TwoMetersTemperature][];
        // Read the binary files and store their data
        ReadBinaryFiles();

        int heightResolution = 180 * 4 + 1;
        int widthResolution = 360 * 4;

        timeCount = fileData[(int)DataTypes.Time].Length;
        latitudeCount = fileData[(int)DataTypes.Latitude].Length;
        longitudeCount = fileData[(int)DataTypes.Longitude].Length;

        texture = new Texture2D(widthResolution, 12 * heightResolution, TextureFormat.RFloat, false);
        data = new float[12*(widthResolution) * (heightResolution)];

        int streamCount = (int)DataTypes.Count - (int)DataTypes.TwoMetersTemperature;
        FileStream[] fileStreams = new FileStream[streamCount]; 

        for(int i = 0; i < streamCount; ++i)
        {
            fileStreams[i] = new FileStream(Application.dataPath + filePaths[(int)DataTypes.TwoMetersTemperature + i] + ".dat", FileMode.Open, FileAccess.Read);
        }


        byte[] bytes = new byte[12*latitudeCount*longitudeCount*sizeof(double)];
        double[] doubleData = new double[12 * latitudeCount * longitudeCount];

        int[] textureIndices = new int[latitudeCount * longitudeCount];
        int[] valueIndices = new int[latitudeCount * longitudeCount];

        int indicesIndex = 0;
        for (int iy = 0; iy < latitudeCount; ++iy)
        {
            float latitude = (float)fileData[(int)DataTypes.Latitude][iy];
            latitude = Mathf.Round(4.0f * (latitude + 90.0f));
            for (int ix = 0; ix < longitudeCount; ++ix)
            {
                float longitude = (float)fileData[(int)DataTypes.Longitude][ix];
                longitude *= 4.0f;
                textureIndices[indicesIndex] = (int)(latitude * widthResolution) + (int)longitude;
                int x = ix + longitudeCount / 2;
                if (x >= longitudeCount)
                {
                    x -= longitudeCount;
                }
                int y = iy;
                valueIndices[indicesIndex++] = (y) * longitudeCount + x;
            }
        }

        for (int dataType = 0; dataType < 11; ++dataType)
        {
            double minVal = dataParams[dataType].minValue;
            double maxVal = dataParams[dataType].maxValue;
            double lowestValue = minVal;
            double highestValue = maxVal;
            for (int yearIndex = 0; yearIndex < 1; ++yearIndex)
            {

                int bytesReadTemperature = fileStreams[dataType].Read(bytes, 0, bytes.Length);

                int valueIndex = 0;
                for (int k = 0; k < bytes.Length; k += sizeof(double))
                {
                    double value = (double)BitConverter.ToDouble(bytes, k);
                    if (double.IsNaN(value) == true)
                    {
                        value = double.MinValue;
                    }
                    doubleData[valueIndex] = value;
                    ++valueIndex;
                }

                for (int monthIndex = 0; monthIndex < 12; ++monthIndex)
                {
                    for (int i = 0; i < (widthResolution) * (heightResolution); ++i)
                    {
                        data[monthIndex * widthResolution * heightResolution + i] = 0;
                    }

                    for (int i = 0; i < (latitudeCount) * (longitudeCount); ++i)
                    {
                        int currentTextureIndex = textureIndices[i];
                        int currentValueIndex = valueIndices[i];
                        double value = doubleData[monthIndex * latitudeCount * longitudeCount + currentValueIndex];

                        float t;
                        t = (float)((value - minVal) / (maxVal - minVal));
                        if (value < lowestValue)
                        {
                            lowestValue = value;
                        }
                        if (value > highestValue)
                        {
                            highestValue = value;
                        }
                        t = Mathf.Clamp01(t);
                        data[(monthIndex * widthResolution * heightResolution) + currentTextureIndex] = t;
                    }

                }

                texture.SetPixelData<float>(data, 0);
                byte[] bytesJPG = texture.EncodeToJPG(100);
                Directory.CreateDirectory(Application.dataPath + outputPath + ((DataTypes)((int)DataTypes.TwoMetersTemperature + dataType)).ToString());
                string filePath = (Application.dataPath + outputPath + ((DataTypes)((int)DataTypes.TwoMetersTemperature + dataType)).ToString() + "/norm.jpg");
                File.WriteAllBytes(filePath, bytesJPG);
            }
            Debug.Log("Lowest value - " + ((DataTypes)((int)DataTypes.TwoMetersTemperature + dataType)).ToString() + " - " + lowestValue.ToString());
            Debug.Log("Highest value - " + ((DataTypes)((int)DataTypes.TwoMetersTemperature + dataType)).ToString() + " - " + highestValue.ToString());

        }


    }

    void ReadBinaryFiles()
    {
        for (int i = 0; i < (int)DataTypes.TwoMetersTemperature; i++)
        {
            if (File.Exists(Application.dataPath + filePaths[i] + ".dat"))
            {
                byte[] fileBytes = File.ReadAllBytes(Application.dataPath + filePaths[i] + ".dat");
                fileData[i] = new double[(int)(fileBytes.Length / sizeof(double))];

                // Process the byte array into doubles
                int valueIndex = 0;
                for (int k = 0; k < fileBytes.Length; k += sizeof(double))
                {
                    double value = (double)BitConverter.ToDouble(fileBytes, k);
                    if(double.IsNaN(value) == true)
                    {
                        value = 0.0;
                    }
                    fileData[i][valueIndex] = value;
                    ++valueIndex;
                }

            }
            else
            {
                Debug.LogError("File not found: " + Application.dataPath + filePaths[i]);
            }
        }

    }
}
