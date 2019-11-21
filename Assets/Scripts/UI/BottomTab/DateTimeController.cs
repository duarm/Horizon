using System;
using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class DateTimeController : MonoBehaviour, IFixedUpdate
{
    [SerializeField] LocalController referencePlanet;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] int startingYear = 2019;
    float timeScale = 1;

    int year;
    int lastMonth = 1;

    private void Awake ()
    {
        year = startingYear;
        EventManager.SubscribeToTimeScaleChanged (OnTimeScaleChanged);
    }

    private void OnEnable ()
    {
        UpdateManager.Subscribe (this);
    }

    private void OnDisable ()
    {
        UpdateManager.Unsubscribe (this);
    }

    public void OnTimeScaleChanged (float value) => timeScale = value;

    void IFixedUpdate.MFixedUpdate ()
    {
        var value = referencePlanet.Orbit.Progress * referencePlanet.Data.orbitPeriod;
        int dayOfYear = Mathf.CeilToInt (value);
        var today = new DateTime (year, 1, 1).AddDays (dayOfYear - 1);
        dateText.text = today.ToString ("yyyy-MM-dd");

        if (lastMonth == 12 && today.Month == 1)
            year++;

        lastMonth = today.Month;
    }
}