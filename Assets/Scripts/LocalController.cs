using SimpleKeplerOrbits;
using UnityEngine;

public class LocalController : MonoBehaviour
{
    public Planet planet;
    public OrbitMotion orbit;
    [Autohook] LineRenderer orbitLine;

    private void OnValidate ()
    {
        planet = GetComponentInChildren<Planet> ();
        orbit = GetComponentInChildren<OrbitMotion> ();
        orbitLine = orbit?.GetComponent<LineRenderer> ();
    }
    
    public void SetOrbitVisiblity (bool value)
    {
        if (orbitLine)
            orbitLine.enabled = value;
    }

    public void SetTimeScale (float value)
    {
        if (orbit != null)
        {
            orbit.timeScale = value;
        }
    }
}