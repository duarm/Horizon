using System.Collections;
using System.Collections.Generic;
using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class DateTimeController : MonoBehaviour
{
    [SerializeField] Planet referencePlanet;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI timeText;
    float timeScale = 1;
    float currentTime;

    private void Awake ()
    {
        currentTime = Time.deltaTime;
        EventManager.SubscribeToTimeScaleChanged (OnTimeScaleChanged);
    }

    private void FixedUpdate()
    {
        var time = referencePlanet.Data.orbitPeriod / currentTime;
        currentTime += Time.deltaTime;
    }

    public void OnTimeScaleChanged (float value) => timeScale = value;

}