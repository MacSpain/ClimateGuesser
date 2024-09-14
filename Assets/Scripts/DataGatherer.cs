using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGatherer : MonoBehaviour
{
    //[SerializeField]
    //private Texture2D countryTexture;
    //[SerializeField]
    //private Texture2DArray[] oldNormArrays;
    //[SerializeField]
    //private Texture2DArray[] newNormArrays;
    //[SerializeField]
    //private Texture2DArray[] comparisonArrays;
    //[SerializeField]
    //private Country[] countries;

    //[System.Serializable]
    //public class CountryValues
    //{
    //    public float[] oldValues;
    //    public float[] newValues;
    //    public float[] comparisonValues;
    //}

    //private Color[] texturePixels;
    //private List<int>[] countryTextureValueIndices;
    //private int width;
    //private int height;
    //private int valueWidth;
    //private float countryToValueWidth;
    //private float countryToValueHeight;
    //private Color[][][] oldNormTextures;
    //private Color[][][] newNormTextures;
    //private Color[][][] comparisonTextures;

    //public CountryValues[] countryValues;

    //void Start()
    //{

    //    texturePixels = countryTexture.GetPixels();
    //    countryValues = new CountryValues[11];
    //    for(int i = 0; i < 11; ++i)
    //    {
    //        countryValues[i] = new CountryValues();
    //        countryValues[i].oldValues = new float[12];
    //        countryValues[i].newValues = new float[12];
    //        countryValues[i].comparisonValues = new float[12];
    //    }
    //    countryTextureValueIndices = new List<int>[countries.Length];
    //    for(int i = 0; i < countries.Length; ++i)
    //    {
    //        countryTextureValueIndices[i] = new List<int>();
    //    }
    //    width = countryTexture.width;
    //    height = countryTexture.height;
    //    oldNormTextures = new Color[11][][];
    //    newNormTextures = new Color[11][][];
    //    comparisonTextures = new Color[11][][];
    //    for (int i = 0; i < 11; ++i)
    //    {
    //        oldNormTextures[i] = new Color[12][];
    //        newNormTextures[i] = new Color[12][];
    //        comparisonTextures[i] = new Color[12][];
    //        for (int j = 0; j < 12; ++j)
    //        {
    //            oldNormTextures[i][j] = oldNormArrays[i].GetPixels(j);
    //            newNormTextures[i][j] = newNormArrays[i].GetPixels(j);
    //            comparisonTextures[i][j] = comparisonArrays[i].GetPixels(j);
    //        }
    //    }
    //    valueWidth = oldNormArrays[0].width;
    //    countryToValueWidth = (float)oldNormArrays[0].width/(float)width;
    //    countryToValueHeight = (float)oldNormArrays[0].height/(float)height;
    //    float yValue = 0.0f;
    //    int index = 0;
    //    for(int y = 0; y < height; ++y)
    //    {
    //        float xValue = 0.0f;
    //        for (int x = 0; x < width; ++x)
    //        {
    //            float redValue = texturePixels[index].r;
    //            if (redValue > 0.0f)
    //            {
    //                float val = 255.0f * redValue - 1.0f;

    //                int countryIndex = (int)val;

    //                int valueIndex = (int)yValue * valueWidth + (int)xValue;

    //                List<int> currentList = countryTextureValueIndices[countryIndex];
    //                if(currentList.Contains(valueIndex) == false)
    //                {
    //                    currentList.Add(valueIndex);
    //                }

    //            }
    //            xValue += countryToValueWidth;
    //            ++index;
    //        }
    //        yValue += countryToValueHeight;
    //    }

    //    for(int i = 0; i < cou)
    //}
}
