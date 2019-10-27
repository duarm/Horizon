using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (OrbitMotion), typeof(LineRenderer), typeof(TrailRenderer))]
public class OrbitRenderer : MonoBehaviour
{
    [SerializeField] OrbitMotion orbitMotion;
    LineRenderer orbitRenderer;
    TrailRenderer trailRenderer;

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

        CalculateOrbit ();
    }

    void Awake ()
    {
        orbitMotion.orbit.OnOrbitUpdate += CalculateOrbit;
    }

    public void SetOrbitVisiblity (bool value)
    {
        orbitRenderer.enabled = value;
    }

    public void SetTrailVisiblity (bool value)
    {
        trailRenderer.enabled = value;
    }

    public void Redraw()
    {
        CalculateOrbit();
    }

    void CalculateOrbit ()
    {
        //+1 to close the circle
        var points = new Vector3[segments + 1];

        for (int i = 0; i < segments; i++)
        {
            var pos = orbitMotion.orbit.Evaluate ((float) i / (float) segments);
            points[i] = new Vector3 (pos.x, transform.position.y, pos.y);
        }

        points[segments] = points[0];
        orbitRenderer.positionCount = segments + 1;
        orbitRenderer.SetPositions (points);
    }

}