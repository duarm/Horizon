using System.Collections;
using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class OrbitRenderer : MonoBehaviour
{
    [Header("Configuration")]
    [Range (3, 360)]
    [SerializeField] int segments;
    [Header("References")]
    [SerializeField] OrbitMotion orbitMotion;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TrailRenderer trailRenderer;


    bool showOrbit = true;

    private void OnValidate ()
    {
        if (!orbitMotion)
            orbitMotion = GetComponentInChildren<OrbitMotion> ();

        if (!lineRenderer)
            lineRenderer = GetComponent<LineRenderer> ();

        if (!trailRenderer)
        {
            trailRenderer = orbitMotion.GetComponent<TrailRenderer> ();
            if(trailRenderer == null)
            {
                trailRenderer = orbitMotion.gameObject.AddComponent(typeof(TrailRenderer)) as TrailRenderer;
            }

            trailRenderer.receiveShadows = false;
            trailRenderer.time = 25;
        }

        CalculateOrbit (segments);
    }

    //STATE
    //Common Initialization Logic
    public void Initialize(PlanetData data)
    {
        
    }

    public void InitializeAsPlanet (PlanetData data)
    {
        Initialize(data);

        trailRenderer.widthMultiplier = 1f;

        SetOrbitVisiblity (true);
        SetTrailVisiblity (false);
    }

    public void InitializeAsMoon (PlanetData data)
    {
        Initialize(data);

        lineRenderer.widthMultiplier = .2f;
        trailRenderer.widthMultiplier = .3f;
    }

    public void OnEnterLocalSpace ()
    {

    }

    public void OnExitLocalSpace ()
    {

    }

    public void Toggle (bool on)
    {
        SetOrbitVisiblity (on);
        SetTrailVisiblity (on);
    }

    public void SetOrbitType (OrbitType type)
    {
        switch (type)
        {
            case OrbitType.LINE:
                SetOrbitVisiblity (true);
                SetTrailVisiblity (false);
                type = OrbitType.LINE;
                break;
            case OrbitType.TRAIL:
                SetTrailVisiblity (true);
                SetOrbitVisiblity (false);
                type = OrbitType.TRAIL;
                break;
        }
    }

    public void ToggleOrbit (bool value)
    {
        showOrbit = value;
        SetOrbitVisiblity (value);
    }

    public void SetOrbitVisiblity (bool value)
    {
        if (!showOrbit)
        {
            lineRenderer.enabled = showOrbit;
            return;
        }

        if (lineRenderer)
            lineRenderer.enabled = value;
    }

    public void SetTrailVisiblity (bool on)
    {
        if (trailRenderer)
        {
            if (on)
                trailRenderer.Clear ();

            trailRenderer.enabled = on;
        }
    }

    public void ClearTrail ()
    {
        if (trailRenderer.enabled)
            StartCoroutine (Clear ());
    }

    IEnumerator Clear ()
    {
        yield return new WaitForEndOfFrame ();
        trailRenderer.Clear ();
    }

    //RENDERER
    public void Redraw (int segments = 0)
    {
        CalculateOrbit (segments);
    }

    void CalculateOrbit (int segments)
    {
        if (!lineRenderer)
            return;

        segments += this.segments;

        //+1 to close the circle
        var points = new Vector3[segments + 1];

        for (int i = 0; i < segments; i++)
        {
            var pos = orbitMotion.orbit.Evaluate ((float) i / (float) segments);
            points[i] = new Vector3 (pos.x, transform.localPosition.y, pos.y);
        }

        points[segments] = points[0];
        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions (points);
    }

    void CalculateOrbit ()
    {
        if (!lineRenderer)
            return;

        //+1 to close the circle
        var points = new Vector3[this.segments + 1];

        for (int i = 0; i < this.segments; i++)
        {
            var pos = orbitMotion.orbit.Evaluate ((float) i / (float) this.segments);
            points[i] = new Vector3 (pos.x, transform.localPosition.y, pos.y);
        }

        points[this.segments] = points[0];
        lineRenderer.positionCount = this.segments + 1;
        lineRenderer.SetPositions (points);
    }

}