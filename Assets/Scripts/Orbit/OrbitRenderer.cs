using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LineRenderer), typeof (OrbitMotion))]
public class OrbitRenderer : MonoBehaviour
{
    [SerializeField] OrbitMotion orbitMotion;
    LineRenderer lineRenderer;

    [Range (3, 36)]
    public int segments;

    private void OnValidate ()
    {
        if (!lineRenderer)
            lineRenderer = GetComponent<LineRenderer> ();

        if (!orbitMotion)
            orbitMotion = GetComponent<OrbitMotion> ();

        CalculateOrbit ();
    }

    private void Awake ()
    {
        orbitMotion.orbit.OnOrbitUpdate += CalculateOrbit;
    }

    void CalculateOrbit ()
    {
        //+1 to close the circle
        var points = new Vector3[segments + 1];

        for (int i = 0; i < segments; i++)
        {
            var pos = orbitMotion.orbit.Evaluate ((float) i / (float) segments);
            points[i] = new Vector3 (pos.x, 0, pos.y);
        }

        points[segments] = points[0];
        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions (points);
    }

}