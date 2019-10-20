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

    public float timeScale = 1f;

    public bool lockOrbit = false;

    float minOrbitDistance;
    float maxOrbitDistance;

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
        CalculateScales();

        orbitMotionRoutine = StartCoroutine (AnimateOrbit ());
    }

    public void CalculateScales()
    {
        minOrbitDistance = orbit.XAxis;
        maxOrbitDistance = orbit.XAxis * SolarSystemController.GetOrbitScale();
    }

    public void IncreaseOrbit (float direction, float rate)
    {
        var newOrbit = orbit.XAxis + (((rate / 100) * maxOrbitDistance) * direction);
        if (newOrbit < minOrbitDistance)
            newOrbit = minOrbitDistance;
        else if (newOrbit > maxOrbitDistance)
            newOrbit = maxOrbitDistance;

        orbit.SetOrbit (newOrbit);
    }

    void SetOrbitingObjectPosition ()
    {
        Vector2 orbitPos = orbit.Evaluate (progress);
        orbitingObject.localPosition = new Vector3 (orbitPos.x, 0, orbitPos.y);
    }

    IEnumerator AnimateOrbit ()
    {
        if (period < 0.1f)
            period = 0.1f;

        float orbitVelocity = 1f / period * timeScale;

        while (!lockOrbit)
        {
            progress += Time.deltaTime * orbitVelocity;
            progress %= 1f;
            SetOrbitingObjectPosition ();
            yield return null;
        }
    }
}