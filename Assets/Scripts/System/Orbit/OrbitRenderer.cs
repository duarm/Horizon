using UnityEngine;

[RequireComponent (typeof (OrbitMotion))]
public class OrbitRenderer : MonoBehaviour
{
    [SerializeField] OrbitMotion orbitMotion;
    [SerializeField] LineRenderer orbitRenderer;
    [SerializeField] TrailRenderer trailRenderer;

    [Range (3, 360)]
    public int segments;
    bool anyRenderer = false;

    private void OnValidate ()
    {
        if (!orbitMotion)
            orbitMotion = GetComponent<OrbitMotion> ();

        if (!orbitRenderer)
            orbitRenderer = GetComponent<LineRenderer> ();

        if (!trailRenderer)
            trailRenderer = GetComponent<TrailRenderer> ();

        CalculateOrbit ();
    }

    void Awake ()
    {
        if (orbitRenderer || trailRenderer)
        {
            anyRenderer = true;
            orbitMotion.orbit.OnOrbitUpdate += CalculateOrbit;
        }
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

    public void OnLocalSpace (bool value)
    {
        SetOrbitVisiblity (value);
    }

    public void StateAsMoon (bool on)
    {
        SetOrbitVisiblity (on);
        SetTrailVisiblity (on);
    }

    public void SetOrbitVisiblity (bool value)
    {
        if (orbitRenderer)
            orbitRenderer.enabled = value;
    }

    public void SetTrailVisiblity (bool value)
    {
        if (trailRenderer)
            trailRenderer.enabled = value;
    }

    //RENDERER

    public void Redraw ()
    {
        CalculateOrbit ();
    }

    void CalculateOrbit ()
    {
        if (!anyRenderer)
            return;

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

}