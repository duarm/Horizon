using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationController : MonoBehaviour
{
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

    public void SetValues (PlanetData data)
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
    }
}