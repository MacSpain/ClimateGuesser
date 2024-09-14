using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    private Button visualButton;
    [SerializeField]
    private Button dataButton;
    [SerializeField]
    private Button gameButton;
    [SerializeField]
    private VisualUIManager visualUI;

    [SerializeField]
    private DataUIManager dataUI;

    [SerializeField]
    private GameUIManager gameUI;

    private CountryClicker countryClicker;

    private void Start()
    {
        countryClicker = FindObjectOfType<CountryClicker>(true);
        visualButton.onClick.Invoke();
    }

    public void SetVisual()
    {
        visualUI.gameObject.SetActive(true);
        dataUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        countryClicker.SetMode(0);
    }
    public void SetData()
    {
        visualUI.gameObject.SetActive(false);
        dataUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
        countryClicker.SetMode(1);
    }
    public void SetGame()
    {
        visualUI.gameObject.SetActive(false);
        dataUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        countryClicker.SetMode(2);
    }
}
