using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    [SerializeField] Transform velocityHandle;
    [SerializeField] Transform orbitingObject;
    public Orbit orbit;

    [Header ("Debug")]
    [Range (0f, 1f)]
    [Tooltip ("Current orbit progress. (0 to 1)")]
    public float progress = 0f;

    [Tooltip ("How much time in seconds to complete the orbit")]
    public float period = 3f;


    public bool lockOrbit = false;

    float timeScale = 1f;

    float minOrbit;
    float maxOrbit;
    float orbitDifference;

    float orbitVelocity;

    Coroutine orbitMotionRoutine;

    private void OnValidate ()
    {
        SetOrbitingObjectPosition ();
    }

    private void Start ()
    {
        transform.position = Vector3.zero;

        if (!orbitingObject)
        {
            lockOrbit = true;
            return;
        }

        SetOrbitingObjectPosition ();
        orbitMotionRoutine = StartCoroutine (AnimateOrbit ());
    }

    public void CalculateScales(float scale)
    {
        minOrbit = orbit.XAxis;
        maxOrbit = orbit.XAxis * scale;
        orbitDifference = maxOrbit - minOrbit;
    }

    public void IncreaseOrbit (float direction, float rate)
    {
        var increasedBy = rate * orbitDifference * direction;
        var newOrbit = orbit.XAxis + increasedBy;

        if (newOrbit < minOrbit)
            newOrbit = minOrbit;
        else if (newOrbit > maxOrbit)
            newOrbit = maxOrbit;

        orbit.SetOrbit (newOrbit);
    }

    void SetOrbitingObjectPosition ()
    {
        Vector2 orbitPos = orbit.Evaluate (progress);
        orbitingObject.localPosition = new Vector3 (orbitPos.x, 0, orbitPos.y);
    }

    public void SetTimeScale(float value)
    {
        timeScale = value;
    }

    IEnumerator AnimateOrbit ()
    {
        if (period < 0.1f)
            period = 0.1f;

        orbitVelocity = 1f / period * timeScale;

        while (!lockOrbit)
        {
            progress += Time.deltaTime * orbitVelocity;
            progress %= 1f;
            SetOrbitingObjectPosition ();
            yield return null;
        }
    }
}