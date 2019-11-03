using UnityEngine;

[RequireComponent (typeof (OrbitMotion))]
public class OrbitRenderer : MonoBehaviour
{
    [SerializeField] OrbitMotion orbitMotion;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TrailRenderer trailRenderer;

    [Range (3, 360)]
    public int segments;

    bool showOrbit = true;
    OrbitType type = OrbitType.LINE;

    private void OnValidate ()
    {
        if (!orbitMotion)
            orbitMotion = GetComponent<OrbitMotion> ();

        if (!lineRenderer)
            lineRenderer = GetComponent<LineRenderer> ();

        if (!trailRenderer)
            trailRenderer = GetComponent<TrailRenderer> ();

        CalculateOrbit (segments);
    }

    //STATE

    public void InitializeAsPlanet ()
    {
        SetOrbitVisiblity (true);
        SetTrailVisiblity (false);
    }

    public void InitializeAsMoon ()
    {

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

    //RENDERER

    public void OnRedraw (int segments = 0)
    {
        CalculateOrbit (segments);
    }

    void CalculateOrbit (int segments)
    {
        if (!lineRenderer)
            return;

        if (segments == 0)
            segments = this.segments;

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