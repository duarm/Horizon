using UnityEngine;

[RequireComponent (typeof (OrbitMotion))]
public class OrbitRenderer : MonoBehaviour
{
    [SerializeField] OrbitMotion orbitMotion;
    [SerializeField] LineRenderer orbitRenderer;
    [SerializeField] TrailRenderer trailRenderer;

    [Range (3, 360)]
    public int segments;

    private void OnValidate ()
    {
        if (!orbitMotion)
            orbitMotion = GetComponent<OrbitMotion> ();

        if (!orbitRenderer)
            orbitRenderer = GetComponent<LineRenderer> ();

        if (!trailRenderer)
            trailRenderer = GetComponent<TrailRenderer> ();

        CalculateOrbit (segments);
    }

    // DYNAMIC SIMULATION PUT ON HOLD
    /*
        void Awake ()
        {
            if (orbitRenderer || trailRenderer)
            {
                anyRenderer = true;
                orbitMotion.orbit.OnOrbitUpdate += CalculateOrbit;
            }
        }
    */
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

    public void SetOrbitVisiblity (bool value)
    {
        if (orbitRenderer)
            orbitRenderer.enabled = value;
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
        if (!orbitRenderer)
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
        orbitRenderer.positionCount = segments + 1;
        orbitRenderer.SetPositions (points);
    }

    void CalculateOrbit ()
    {
        if (!orbitRenderer)
            return;

        //+1 to close the circle
        var points = new Vector3[this.segments + 1];

        for (int i = 0; i < this.segments; i++)
        {
            var pos = orbitMotion.orbit.Evaluate ((float) i / (float) this.segments);
            points[i] = new Vector3 (pos.x, transform.localPosition.y, pos.y);
        }

        points[this.segments] = points[0];
        orbitRenderer.positionCount = this.segments + 1;
        orbitRenderer.SetPositions (points);
    }

}