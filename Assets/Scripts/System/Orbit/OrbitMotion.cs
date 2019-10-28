using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    [SerializeField] Transform velocityHandle;
    [SerializeField] Transform orbitingObject;
    [SerializeField] Rigidbody rb;
    public Orbit orbit;

    [Header ("Debug")]
    [Range (0f, 1f)]
    [Tooltip ("Current orbit progress. (0 to 1)")]
    [SerializeField] float progress = 0f;

    [Tooltip ("How much time in seconds to complete the orbit")]
    [SerializeField] float period = 3f;

    public bool lockOrbit = false;
    [SerializeField] bool debug = false;

    float timeScale = 1f;
    float minOrbit;
    float maxOrbit;
    float orbitDifference;


    private void OnValidate ()
    {
        if(debug)
            SetOrbitingObjectPosition ();
    }

    private void Awake ()
    {
        transform.position = Vector3.zero;

        if (!orbitingObject)
        {
            lockOrbit = true;
            return;
        }
    }

    public void Initialize (float scale)
    {
        CalculateScales (scale);
        SetOrbitingObjectPosition ();
        StartCoroutine (AnimateOrbit ());
    }

    void CalculateScales (float scale)
    {
        minOrbit = orbit.XAxis;
        maxOrbit = orbit.XAxis * scale;
        orbitDifference = maxOrbit - minOrbit;
    }

    public void SetPosition (Vector3 newPos)
    {
        rb.position = newPos;
    }

    public void SetTimeScale (float value)
    {
        timeScale = value;
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

    IEnumerator AnimateOrbit ()
    {
        if (period < 0.1f)
            period = 0.1f;

        var orbitVelocity = 1f / period;

        while (!lockOrbit)
        {
            progress += Time.deltaTime * orbitVelocity * timeScale;
            progress %= 1f;
            SetOrbitingObjectPosition ();
            yield return null;
        }
    }
}