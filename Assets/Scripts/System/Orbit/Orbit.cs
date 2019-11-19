using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Orbit
{
    [SerializeField] float xAxis = 5;
    [SerializeField] float yAxis = 5;

    public float XAxis { get { return xAxis; } }
    public float YAxis { get { return yAxis; } }

    public Orbit (float xAxis, float yAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }

    public void SetOrbit (float newOrbit)
    {
        xAxis = newOrbit;
        yAxis = newOrbit;
    }

    public Vector2 Evaluate (float t)
    {
        var angle = Mathf.Deg2Rad * 360 * t;
        var x = Mathf.Sin (angle) * xAxis;
        var y = Mathf.Cos (angle) * yAxis;
        return new Vector2 (x, y);
    }
}