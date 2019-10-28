using SimpleKeplerOrbits;
using UnityEngine;

public class LocalController : MonoBehaviour
{
    public Planet planet;
    public OrbitMotion orbit;
    public OrbitRenderer orbitRenderer;

    bool getRenderer;

    private void OnValidate ()
    {
        if (!planet)
            planet = GetComponentInChildren<Planet> ();
        if (!orbit)
            orbit = GetComponentInChildren<OrbitMotion> ();

        if (!orbitRenderer && !getRenderer)
        {
            orbitRenderer = orbit?.GetComponent<OrbitRenderer> ();
            getRenderer = true;
        }
    }

    public void Initialize (float scale, Texture2D texture, float resolution)
    {
        planet.InitializeAsPlanet (scale, texture, resolution);
        orbit.Initialize (scale);
    }

    public void SetOrbitVisiblity (bool value)
    {
        if (orbitRenderer)
            orbitRenderer.SetOrbitVisiblity (value);
    }

    public void SetFocus (bool value)
    {
        planet.SetFocus (value);
    }

    public void SetTimeScale (float value)
    {
        if (orbit != null)
        {
            orbit.SetTimeScale (value);
        }
    }

    public override string ToString ()
    {
        return this.planet.planetName;
    }
}