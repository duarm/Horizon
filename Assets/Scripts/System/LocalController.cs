using System;
using UnityEngine;

//TODO: SPLIT MOON/PLANET
public class LocalController : MonoBehaviour, IEquatable<LocalController>
{
    [SerializeField] Planet planet;
    [SerializeField] OrbitMotion orbit;
    [SerializeField] OrbitRenderer orbitRenderer;

    bool getRenderer;

    public Vector3 OrbitPosition { get { return orbit.rb.position; } }
    public PlanetData Data { get { return planet.Data; } }

    private void OnValidate ()
    {
        if (!planet)
            planet = GetComponentInChildren<Planet> ();
        if (!orbit)
            orbit = GetComponentInChildren<OrbitMotion> ();

        if (!orbitRenderer && !getRenderer)
        {
            orbitRenderer = GetComponent<OrbitRenderer> ();
            getRenderer = true;
        }
    }

    public void OnEnterLocalSpace (int orbitRes)
    {
        planet.OnEnterLocalSpace ();
        orbit.OnEnterLocalSpace ();
        orbitRenderer.OnEnterLocalSpace ();
        orbitRenderer.Redraw (orbitRes);
    }

    public void OnExitLocalSpace ()
    {
        planet.OnExitLocalSpace ();
        orbitRenderer.OnExitLocalSpace ();
        orbit.OnExitLocalSpace ();
    }

    public void SetMoonVisibility (bool on)
    {
        gameObject.SetActive (on);
        planet.Toggle (on);
        orbitRenderer.Toggle (on);
        orbit.Toggle (on);
    }

    public void SetVisibility (bool on)
    {
        orbitRenderer.SetOrbitVisiblity (on);
        planet.Toggle (on);
    }

    private void Initialize ()
    {
        transform.localEulerAngles = new Vector3 (0, 0, Data.orbitInclination);
        planet.transform.localEulerAngles = new Vector3 (0, 0, Data.inclination);
    }

    public void InitializeAsPlanet (float scale, float resolution, Texture2D texture)
    {
        Initialize ();
        planet?.InitializeAsPlanet (scale, resolution, texture);
        orbit?.InitializeAsPlanet (Data, scale);
        orbitRenderer?.InitializeAsPlanet ();
    }

    public void InitializeAsMoon (float resolution)
    {
        Initialize ();
        planet?.InitializeAsMoon (resolution);
        orbit?.InitializeAsMoon (Data);
        orbitRenderer?.InitializeAsMoon ();
    }

    public void Zoom (float direction, float percentage)
    {
        orbit.IncreaseOrbit (direction, percentage);
        planet.Scale (direction, percentage);
        if (orbitRenderer)
        {
            orbitRenderer.Redraw ();
            orbitRenderer.ClearTrail ();
        }
    }

    public void OnUpwardMotion ()
    {
        orbitRenderer?.ClearTrail ();
    }

    public void OnRedraw (int segments = 0)
    {
        orbitRenderer.Redraw (segments);
    }

    public void ToggleOrbit (bool value)
    {
        orbitRenderer.ToggleOrbit (value);
    }

    public void SetOrbitType (OrbitType type)
    {
        orbitRenderer.SetOrbitType (type);
    }

    public void SetOrbitVisiblity (bool value)
    {
        if (orbitRenderer)
            orbitRenderer.SetOrbitVisiblity (value);
    }

    public void SetPosition (Vector3 newPosition)
    {
        transform.localPosition = newPosition;
    }

    public void SetFocus (bool value)
    {
        planet.ToggleDisplayPlanet (value);
        planet.SetFocus (value);
    }

    public void SetTimeScale (float value)
    {
        orbit?.SetTimeScale (value);
        planet.SetTimeScale (value);
    }

    public override string ToString ()
    {
        return Data.name;
    }

    public bool Equals (LocalController other)
    {
        if (other == null)
            return false;
        if (Data.name.Equals (other.ToString ()))
            return true;
        return false;
    }
}

// Solar System > delegates ==> Local controller > delegates ==> motion,renderer,planet
//TODO: STATE CONTROL
/*
public enum States 
{
    OUT_LOCAL_SPACE,
    IN_LOCAL_SPACE,
}*/