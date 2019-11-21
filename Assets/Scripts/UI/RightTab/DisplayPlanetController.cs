using System;
using UnityEngine;

public class DisplayPlanetController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] GameObject eyeOpen;
    [SerializeField] GameObject eyeClosed;

    Planet planet;
    bool on = false;

    public void OnUpdateBar (Planet planet)
    {
        this.planet?.ToggleDisplayPlanet (false);
        this.planet = planet;
        TogglePlanet ();
        ToggleAtmosphere ();
    }

    public void TogglePlanet() => planet.ToggleDisplayPlanet(true);

    public void OnToggleAtmosphere() => ToggleAtmosphere();

    void ToggleAtmosphere ()
    {
        on = !on && planet.HasAtmosphere;
        eyeOpen.SetActive (on);
        eyeClosed.SetActive (!on);

        planet.ToggleAtmosphere (on);
    }
}