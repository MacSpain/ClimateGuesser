using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(DataGatherer))]
public class DataGathererEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DataGatherer myTarget = (DataGatherer)target;
        if (GUILayout.Button("Gather Data"))
        {
            myTarget.GatherData();
            EditorUtility.SetDirty(myTarget);
        }
    }
}


public class DataGatherer : MonoBehaviour
{
    [SerializeField]
    private Texture2D countryTexture;
    [SerializeField]
    private Texture2DArray[] oldNormArrays;
    [SerializeField]
    private Texture2DArray[] newNormArrays;
    [SerializeField]
    private Texture2DArray[] comparisonArrays;
    [SerializeField]
    private Country[] countries;

    public float[][] currentOldValues; 
    public float[][] currentNewValues; 
    public float[][] currentComparisonValues;

    public float[][] clickedOldValues;
    public float[][] clickedNewValues;
    public float[][] clickedComparisonValues;

    private Color[] texturePixels;
    private int width;
    private int height;
    private int valueWidth;
    private int valueHeight;
    private float countryToValueWidth;
    private float countryToValueHeight;
    [SerializeField]
    public List<int>[] countryTextureValueIndices;
    [SerializeField]
    private Color[][][] oldNormTextures;
    [SerializeField]
    private Color[][][] newNormTextures;
    [SerializeField]
    private Color[][][] comparisonTextures;

    private void Start()
    {
        width = countryTexture.width;
        height = countryTexture.height;
        valueWidth = oldNormArrays[0].width;
        countryToValueWidth = (float)oldNormArrays[0].width / (float)width;
        countryToValueHeight = (float)oldNormArrays[0].height / (float)height;
    }


    public float[] GetClickedValues(int dataType, DataUIManager.DataMode dataMode)
    {
        switch(dataMode)
        {
            case DataUIManager.DataMode.OldNorm:
                {
                    return clickedOldValues[dataType];
                }
                break;
            case DataUIManager.DataMode.NewNorm:
                {
                    return clickedNewValues[dataType];
                }
                break;
            case DataUIManager.DataMode.NormDifference:
                {
                    return clickedComparisonValues[dataType];
                }
                break;
        }
        return null;
    }

    public void FillDataAtPoint(int valueIndex)
    {
        currentOldValues = new float[11][];
        currentNewValues = new float[11][];
        currentComparisonValues = new float[11][];

        for(int i = 0; i < 11; ++i)
        {
            currentOldValues[i] = new float[12];
            currentNewValues[i] = new float[12];
            currentComparisonValues[i] = new float[12];
            for(int j = 0; j < 12; ++j)
            {
                currentOldValues[i][j] = oldNormTextures[i][j][valueIndex].r;
                currentNewValues[i][j] = newNormTextures[i][j][valueIndex].r;
                currentComparisonValues[i][j] = comparisonTextures[i][j][valueIndex].r;
            }
        }
    }
    public void FillDataAtClickedPoint(int valueIndex)
    {
        clickedOldValues = new float[11][];
        clickedNewValues = new float[11][];
        clickedComparisonValues = new float[11][];

        for (int i = 0; i < 11; ++i)
        {
            clickedOldValues[i] = new float[12];
            clickedNewValues[i] = new float[12];
            clickedComparisonValues[i] = new float[12];
            for (int j = 0; j < 12; ++j)
            {
                clickedOldValues[i][j] = oldNormTextures[i][j][valueIndex].r;
                clickedNewValues[i][j] = newNormTextures[i][j][valueIndex].r;
                clickedComparisonValues[i][j] = comparisonTextures[i][j][valueIndex].r;
            }
        }
    }

    public void GatherData()
    {

        texturePixels = countryTexture.GetPixels();
        countryTextureValueIndices = new List<int>[countries.Length];
        for(int i = 0; i < countries.Length; ++i)
        {
            countryTextureValueIndices[i] = new List<int>();
        }
        width = countryTexture.width;
        height = countryTexture.height;
        oldNormTextures = new Color[11][][];
        newNormTextures = new Color[11][][];
        comparisonTextures = new Color[11][][];
        for (int i = 0; i < 11; ++i)
        {
            oldNormTextures[i] = new Color[12][];
            newNormTextures[i] = new Color[12][];
            comparisonTextures[i] = new Color[12][];
            for (int j = 0; j < 12; ++j)
            {
                oldNormTextures[i][j] = oldNormArrays[i].GetPixels(j);
                newNormTextures[i][j] = newNormArrays[i].GetPixels(j);
                comparisonTextures[i][j] = comparisonArrays[i].GetPixels(j);
            }
        }
        valueWidth = oldNormArrays[0].width;
        valueHeight = oldNormArrays[0].height;
        countryToValueWidth = (float)oldNormArrays[0].width/(float)width;
        countryToValueHeight = (float)oldNormArrays[0].height/(float)height;
        List<int>[] ownedByCountries = new List<int>[valueWidth*valueHeight];
        for(int i = 0; i < valueWidth*valueHeight; ++i)
        {
            ownedByCountries[i] = new List<int>();
        }
        float yValue = 0.0f;
        int index = 0;
        for(int y = 0; y < height; ++y)
        {
            float xValue = 0.0f;
            for (int x = 0; x < width; ++x)
            {
                float redValue = texturePixels[index].r;
                if (redValue > 0.0f)
                {
                    float val = 255.0f * redValue - 1.0f;

                    int countryIndex = (int)val;

                    int valueIndex = (int)yValue * valueWidth + (int)xValue;

                    List<int> currentOwned = ownedByCountries[valueIndex];
                    if (currentOwned.Contains(countryIndex) == false)
                    {
                        List<int> currentList = countryTextureValueIndices[countryIndex];
                        currentList.Add(valueIndex);
                    }

                }
                xValue += countryToValueWidth;
                ++index;
            }
            yValue += countryToValueHeight;
        }
    }
}
