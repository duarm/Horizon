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
        CalculateScales();

        orbitMotionRoutine = StartCoroutine (AnimateOrbit ());
    }

    public void CalculateScales()
    {
        minOrbitDistance = orbit.XAxis;
        maxOrbitDistance = orbit.XAxis * SolarSystemController.GetOrbitScale();
    }

    public float IncreaseOrbit (float direction, float rate)
    {
        var increasedBy = rate * maxOrbitDistance * direction;
        var newOrbit = orbit.XAxis + increasedBy;

        if (newOrbit < minOrbitDistance)
        {
            increasedBy = minOrbitDistance - orbit.XAxis;
            newOrbit = minOrbitDistance;
        }
        else if (newOrbit > maxOrbitDistance)
        {
            increasedBy = orbit.XAxis - maxOrbitDistance;
            newOrbit = maxOrbitDistance;
        }

        orbit.SetOrbit (newOrbit);
        return increasedBy;
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