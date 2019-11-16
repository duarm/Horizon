using System;
using System.Collections.Generic;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RightTabController : MonoBehaviour
{
    [SerializeField] bool infoIsDefaultTab = true;
    [SerializeField] GameObject rightTab;
    [SerializeField] Animator rightTabAnimator;
    [SerializeField] Transform triviaParent;

    [Header("Tabs")]
    [SerializeField] GameObject infoContent;
    [SerializeField] GameObject triviaContent;
    [SerializeField] GameObject infoSelected;
    [SerializeField] GameObject triviaSelected;

    [Header("Info")]
    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI inclinationField;
    [SerializeField] TextMeshProUGUI perihelionField;
    [SerializeField] TextMeshProUGUI aphelionField;
    [SerializeField] TextMeshProUGUI orbitPeriodField;
    [SerializeField] TextMeshProUGUI rotationPeriodField;
    [SerializeField] TextMeshProUGUI orbitalSpeedField;
    [SerializeField] TextMeshProUGUI diameterField;
    [SerializeField] TextMeshProUGUI surfaceAreaField;
    [SerializeField] TextMeshProUGUI averageTemperatureField;
    [SerializeField] TextMeshProUGUI minTemperatureField;
    [SerializeField] TextMeshProUGUI maxTemperatureField;
    [SerializeField] TextMeshProUGUI gravityField;
    [SerializeField] TextMeshProUGUI massField;
    [SerializeField] TextMeshProUGUI densityField;

    [SerializeField] TextMeshProUGUI[] triviaTexts;

    private void OnValidate ()
    {
        if (!rightTab)
            rightTab = transform.GetChild (0).gameObject;
    }

    private void Awake() {
        triviaTexts = triviaParent.GetComponentsInChildren<TextMeshProUGUI>(true);
    }

    private void Start ()
    {
        EventManager.SubscribeToFocus (OpenRightBar);

        if(infoIsDefaultTab)
            OpenInfoTab();
        else
            OpenTriviaTab();
    }

    public void OpenRightBar (PlanetData data)
    {
        rightTab.SetActive (true);
        SetValues (data);
    }

    public void OpenTriviaTab ()
    {
        infoSelected.SetActive(false);
        triviaSelected.SetActive(true);
        infoContent.SetActive(false);
        triviaContent.SetActive(true);
    }

    public void OpenInfoTab ()
    {
        triviaSelected.SetActive(false);
        infoSelected.SetActive(true);
        triviaContent.SetActive(false);
        infoContent.SetActive(true);
    }

    void SetValues (PlanetData data)
    {
        if (data == null)
            return;

        nameField.text = data.name;
        inclinationField.text = data.inclination + "°";
        perihelionField.text = data.perihelion + " km";
        aphelionField.text = data.aphelion + " km";
        orbitPeriodField.text = data.orbitPeriod + " dias";
        rotationPeriodField.text = data.rotationPeriod + " dias";
        orbitalSpeedField.text = data.orbitalSpeed + " km/s";
        diameterField.text = data.diameter + " km";
        surfaceAreaField.text = data.surfaceArea + " km²";
        averageTemperatureField.text = data.averageTemperature + " °C";
        minTemperatureField.text = data.minTemperature != 0 ? data.minTemperature + " °C" : "-";
        maxTemperatureField.text = data.maxTemperature != 0 ? data.maxTemperature + " °C" : "-";
        gravityField.text = data.gravity + " m/s²";
        massField.text = data.mass;
        densityField.text = data.density + " g/cm³";

        ShowTrivias(data);
    }

    private void ShowTrivias(PlanetData data)
    {
        for (int i = 0; i < triviaTexts.Length; i++)
        {
            if(i >= data.trivias.Count)
            {
                triviaTexts[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                triviaTexts[i].text = data.trivias[i];
                triviaTexts[i].transform.parent.gameObject.SetActive(true);
            }
        }
    }
}