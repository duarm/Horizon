using System;
using System.Collections;
using System.Collections.Generic;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class DateTimeController : MonoBehaviour
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

    public void OnTimeScaleChanged (float value) => timeScale = value;

    private void FixedUpdate ()
    {
        var value = referencePlanet.Orbit.Progress * referencePlanet.Data.orbitPeriod;
        int dayOfYear = Mathf.CeilToInt (value);
        var today = new DateTime(year, 1, 1).AddDays(dayOfYear -1);
        dateText.text = today.ToString("yyyy-MM-dd");

        if(lastMonth == 12 && today.Month == 1)
            year++;

        lastMonth = today.Month;
    }


}