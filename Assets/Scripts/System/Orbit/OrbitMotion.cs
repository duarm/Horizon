using System.Collections;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class OrbitMotion : MonoBehaviour
{
    //[SerializeField] Transform velocityHandle;
    public Orbit orbit;

    [Header ("Debug")]
    [Range (0f, 1f)]
    [Tooltip ("Current orbit progress. (0 to 1)")]
    [SerializeField] float progress = 0f;

    [Tooltip ("How much time in seconds to complete the orbit")]
    [SerializeField] float period = 3f;
    [SerializeField] float smoothTime = .5f;

    public bool lockOrbit = true;
    [SerializeField] bool debug = false;

    float timeScale = 1f;
    float minOrbit;
    float maxOrbit;
    float orbitDifference;

    Vector3 velocity;
    public Rigidbody rb { get; private set; }

    private void OnValidate ()
    {
        if (debug)
            SetOrbitingObjectPosition ();
    }

    private void Awake ()
    {
        if(!rb)
            rb = GetComponent<Rigidbody>();

        transform.localPosition = Vector3.zero;

        if (!rb)
        {
            lockOrbit = true;
            return;
        }
    }

    // STATE

    public void InitializeAsPlanet (PlanetData data, float scale)
    {
        CalculateScales (scale);
        SetOrbitingObjectPosition ();
        Toggle (true);
    }

    public void InitializeAsMoon (PlanetData data)
    {
        SetOrbitingObjectPosition ();
        Toggle (false);
    }

    public void Toggle (bool on)
    {
        lockOrbit = !on;
        if (on)
            StartCoroutine (AnimateOrbit ());
    }

    public void OnEnterLocalSpace ()
    {

    }

    public void OnExitLocalSpace ()
    {

    }

    // ORBIT MOTION
    void CalculateScales (float scale)
    {
        minOrbit = orbit.XAxis;
        maxOrbit = orbit.XAxis * scale;
        orbitDifference = maxOrbit - minOrbit;
    }

    public void SetPosition (Vector3 newPos)
    {
        transform.localPosition = newPos;
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
        var orbitPos = orbit.Evaluate (progress);
        transform.localPosition = new Vector3 (orbitPos.x, 0, orbitPos.y);
    }

    IEnumerator AnimateOrbit ()
    {
        if (period < 0.1f)
            period = 0.1f;

        var orbitVelocity = 1f / period;

        while (true)
        {
            progress += Time.fixedDeltaTime * orbitVelocity * timeScale;
            progress %= 1f;

            if (lockOrbit)
                break;

            SetOrbitingObjectPosition ();
            yield return new WaitForEndOfFrame ();
        }

        yield return null;
    }
}