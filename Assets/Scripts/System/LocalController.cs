using SimpleKeplerOrbits;
using UnityEngine;

public class LocalController : MonoBehaviour
{
    public Planet planet;
    public OrbitMotion orbit;
    [Autohook] public OrbitRenderer orbitRenderer;

    private void OnValidate ()
    {
        planet = GetComponentInChildren<Planet> ();
        orbit = GetComponentInChildren<OrbitMotion> ();
        orbitRenderer = GetComponentInChildren<OrbitRenderer> ();
    }
    
    public void SetOrbitVisiblity (bool value)
    {
        if (orbitRenderer)
            orbitRenderer.SetOrbitVisiblity(value);
    }

    public void SetTimeScale (float value)
    {
        if (orbit != null)
        {
            orbit.timeScale = value;
        }
    }
}