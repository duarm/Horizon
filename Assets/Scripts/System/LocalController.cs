using System;
using UnityEngine;

//TODO: SPLIT MOON/PLANET
public class LocalController : MonoBehaviour, IEquatable<LocalController>
{
    [SerializeField] Planet planet;
    [SerializeField] OrbitMotion orbit;
    [SerializeField] OrbitRenderer orbitRenderer;

    bool getRenderer;

    public Vector3 OrbitPosition { get { return orbit.transform.position; } }
    public PlanetData Data { get { return planet.data; } }

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

    public void OnEnterLocalSpace ()
    {
        planet.OnEnterLocalSpace ();
        orbit.OnEnterLocalSpace ();
        orbitRenderer.OnEnterLocalSpace ();
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

    public void InitializeAsPlanet (float scale, float resolution, Texture2D texture)
    {
        if (planet)
            planet.InitializeAsPlanet (scale, resolution, texture);
        if (orbit)
            orbit.InitializeAsPlanet (planet.data, scale);
        if (orbitRenderer)
            orbitRenderer.InitializeAsPlanet ();

        orbit.transform.localEulerAngles = new Vector3 (0, 0, planet.data.inclination);
    }

    public void InitializeAsMoon (float resolution)
    {
        if (planet)
            planet.InitializeAsMoon (resolution);
        if (orbit)
            orbit.InitializeAsMoon (planet.data);
        if (orbitRenderer)
            orbitRenderer.InitializeAsMoon ();

        orbit.transform.localEulerAngles = new Vector3 (0, 0, planet.data.inclination);
    }

    public void Zoom (float direction, float percentage)
    {
        orbit.IncreaseOrbit (direction, percentage);
        planet.Scale (direction, percentage);
    }

    public void Redraw (int segments = 0)
    {
        orbitRenderer.OnRedraw (segments);
    }

    public void SetOrbitVisiblity (bool value)
    {
        if (orbitRenderer)
            orbitRenderer.SetOrbitVisiblity (value);
    }

    public void SetPosition (Vector3 newPosition)
    {
        orbit.SetPosition (newPosition);
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
        return this.planet.data.name;
    }

    public bool Equals (LocalController other)
    {
        if (other == null)
            return false;
        if (planet.data.name.Equals (other.ToString ()))
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