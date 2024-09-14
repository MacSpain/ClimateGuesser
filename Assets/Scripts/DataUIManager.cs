using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataUIManager : MonoBehaviour
{
    public enum DataMode
    {
        OldNorm,
        NewNorm,
        NormDifference,
    }

    [SerializeField]
    private Button oldClimateNormButton;
    [SerializeField]
    private Button newClimateNormButton;
    [SerializeField]
    private Button normDifferenceButton;
    [SerializeField]
    private TMP_Text currentRegionSelectionText;
    [SerializeField]
    private TMP_Text currentDataSelectionText;
    [SerializeField]
    private RawImage legendImage;
    [SerializeField]
    private TMP_Text legendMinValueText;
    [SerializeField]
    private TMP_Text legendMaxValueText;
    [SerializeField]
    private string[] dataTypesNames;
    [SerializeField]
    private Renderer earthRenderer;
    [SerializeField]
    private GameObject playGuessButtonObject;
    [SerializeField]
    private GameObject guessObject;
    [SerializeField]
    private TMP_Text guessCountry;
    [SerializeField]
    private RawImage guessLegendImage;
    [SerializeField]
    private TMP_Text guessDistance;

    private DataMode currentDataMode = DataMode.OldNorm;
    private int currentDataType;
    private int currentRegionSelected;
    private CountryClicker countryClicker;
    private ReadMeanTemperatureFiles reader;
    private DataGatherer gatherer;
    private MainUIManager mainUIManager;

    public void PrepareGuess()
    {
        guessObject.SetActive(true);
        playGuessButtonObject.SetActive(false);
    }
    public void CloseGuess()
    {
        guessObject.SetActive(false);
        playGuessButtonObject.SetActive(true);
    }

    public void RegionSelected(int index, string region)
    {
        currentRegionSelected = index;
        if (index == -1)
        {
            currentRegionSelectionText.text = "Earth";
        }
        else
        {
            currentRegionSelectionText.text = region;
        }
        ChooseLegend(currentDataMode, currentDataType);
    }


    public void SetLegend(Texture2D legendTexture, double legendMinValue, double legendMaxValue, float[] values)
    {
        legendMinValueText.text = legendMinValue.ToString("F2");
        legendMaxValueText.text = legendMaxValue.ToString("F2");
        legendImage.material.SetTexture("_LegendTexture", legendTexture);
        if (values != null)
        {
            legendImage.material.SetFloat("_Value1", values[0]);
            legendImage.material.SetFloat("_Value2", values[1]);
            legendImage.material.SetFloat("_Value3", values[2]);
            legendImage.material.SetFloat("_Value4", values[3]);
            legendImage.material.SetFloat("_Value5", values[4]);
            legendImage.material.SetFloat("_Value6", values[5]);
            legendImage.material.SetFloat("_Value7", values[6]);
            legendImage.material.SetFloat("_Value8", values[7]);
            legendImage.material.SetFloat("_Value9", values[8]);
            legendImage.material.SetFloat("_Value10", values[9]);
            legendImage.material.SetFloat("_Value11", values[10]);
            legendImage.material.SetFloat("_Value12", values[11]);
            legendImage.material.SetFloat("_Value12", values[11]);
        }
        else
        {

            legendImage.material.SetFloat("_Value1", 0.0f);
            legendImage.material.SetFloat("_Value2", 0.0f);
            legendImage.material.SetFloat("_Value3", 0.0f);
            legendImage.material.SetFloat("_Value4", 0.0f);
            legendImage.material.SetFloat("_Value5", 0.0f);
            legendImage.material.SetFloat("_Value6", 0.0f);
            legendImage.material.SetFloat("_Value7", 0.0f);
            legendImage.material.SetFloat("_Value8", 0.0f);
            legendImage.material.SetFloat("_Value9", 0.0f);
            legendImage.material.SetFloat("_Value10", 0.0f);
            legendImage.material.SetFloat("_Value11", 0.0f);
            legendImage.material.SetFloat("_Value12", 0.0f);
        }
    }

    public void ChooseLegend(DataMode dataMode, int dataType)
    {
        Texture2D texture = null;
        double minVal = 0.0;
        double maxVal = 0.0;
        float[] values = gatherer.GetClickedValues(dataType, dataMode);
        switch(dataMode)
        {
            case DataMode.OldNorm:
                {
                    texture = reader.gradientTextures[dataType];
                    minVal = reader.dataParams[dataType].minValue;
                    maxVal = reader.dataParams[dataType].maxValue;
                }
                break;
            case DataMode.NewNorm:
                {

                    texture = reader.gradientTextures[dataType];
                    minVal = reader.dataParams[dataType].minValue;
                    maxVal = reader.dataParams[dataType].maxValue;
                }
                break;
            case DataMode.NormDifference:
                {

                    texture = reader.comparisonGradientTextures[dataType];
                    minVal = reader.normDataParams[dataType].minValue;
                    maxVal = reader.normDataParams[dataType].maxValue;
                }
                break;
        }
        SetLegend(texture, minVal, maxVal, values);
    }

    public void PreviousType()
    {
        currentDataType--;
        if (currentDataType == 0)
        {
            currentDataType = dataTypesNames.Length - 1;
        }
        currentDataSelectionText.text = dataTypesNames[currentDataType];
        ChooseLegend(currentDataMode, currentDataType);
    }
    public void NextType()
    {
        currentDataType = (currentDataType + 1) % dataTypesNames.Length;
        currentDataSelectionText.text = dataTypesNames[currentDataType];
        ChooseLegend(currentDataMode, currentDataType);
    }
    public void SetDataType(int index)
    {
        currentDataType = index;
        currentDataSelectionText.text = dataTypesNames[currentDataType];
        ChooseLegend(currentDataMode, currentDataType);
    }
    public void SetDataMode(int index)
    {
        currentDataMode = (DataMode)index;
        earthRenderer.material.SetFloat("_Choice", (float)index);
        switch(index)
        {
            case 0:
                {

                    mainUIManager.SetCurrentDataNormButton(oldClimateNormButton);
                }
                break;
            case 1:
                {

                    mainUIManager.SetCurrentDataNormButton(newClimateNormButton);
                }
                break;
            case 2:
                {

                    mainUIManager.SetCurrentDataNormButton(normDifferenceButton);
                }
                break;
        }
        ChooseLegend(currentDataMode, currentDataType);
    }

    private void OnEnable()
    {
        if (countryClicker == null)
        {
            countryClicker = FindObjectOfType<CountryClicker>(true);
            reader = FindObjectOfType<ReadMeanTemperatureFiles>(true);
            mainUIManager = FindObjectOfType<MainUIManager>(true);
            gatherer = FindObjectOfType<DataGatherer>(true);
        }
        SetDataType(currentDataType);
        countryClicker.ChooseCountry(0);
        RegionSelected(-1, "");
        earthRenderer.material.SetFloat("_Choice", (float)(int)currentDataMode);
        ChooseLegend(currentDataMode, currentDataType);
        oldClimateNormButton.onClick.Invoke();
        mainUIManager.SetCurrentDataNormButton(oldClimateNormButton);
    }


}
