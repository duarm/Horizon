using System.Collections;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class OrbitMotion : MonoBehaviour
{
    public Orbit orbit;
    [Range (0f, 1f)]
    [Tooltip ("Current orbit progress. (0 to 1)")]
    [SerializeField] float progress = 0f;
    [Tooltip ("How much time in seconds to complete the orbit")]
    [SerializeField] bool debug = false;

    float timeScale = 1f;
    float minOrbit;
    float maxOrbit;
    float orbitDifference;
    float period;
    bool lockOrbit = true;

    Vector3 velocity;
    public float Progress { get { return progress; } }
    public Vector3 Position { get { return rb.position; } }

    Rigidbody rb;

    private void OnValidate ()
    {
        if (debug)
            SetOrbitingObjectPosition ();

        if (!rb)
        {
            rb = GetComponent<Rigidbody> ();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void Awake ()
    {
        transform.localPosition = Vector3.zero;

        if (!rb)
        {
            lockOrbit = true;
            return;
        }
    }

    // STATE

    //Common initialization logic
    public void Initialize (PlanetData data)
    {
        period = data.orbitPeriod;
        SetOrbitingObjectPosition ();
    }

    public void InitializeAsPlanet (PlanetData data, float scale)
    {
        CalculateScales (scale);
        Initialize (data);
        Toggle (true);
    }

    public void InitializeAsMoon (PlanetData data)
    {
        Initialize (data);
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
            progress += Time.deltaTime * orbitVelocity * timeScale;
            progress %= 1f;

            if (lockOrbit)
                break;

            SetOrbitingObjectPosition ();
            yield return null;
        }
    }
}