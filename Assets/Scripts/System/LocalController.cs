using System;
using UnityEngine;

//TODO: SPLIT MOON/PLANET
public class LocalController : MonoBehaviour, IEquatable<LocalController>
{
    [SerializeField] Planet planet;
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

    public void EnterLocalSpace (bool value)
    {
        planet.EnterLocalSpace (value);
    }

    public void OnLocalSpace (bool value)
    {
        planet.OnLocalSpace (value);
        orbitRenderer.OnLocalSpace (value);
    }

    public void SetMoonVisibility (bool on)
    {
        gameObject.SetActive (on);
        planet.Toggle (on);
        orbitRenderer.Toggle (on);
        orbit.Toggle (on);
    }

    public void InitializeAsPlanet (float scale, float resolution, Texture2D texture)
    {
        if (planet)
            planet.InitializeAsPlanet (scale, resolution, texture);
        if (orbit)
            orbit.InitializeAsPlanet (scale);
        if (orbitRenderer)
            orbitRenderer.InitializeAsPlanet ();

        orbit.transform.localEulerAngles = new Vector3 (0, 0, planet.data.inclination);
    }

    public void InitializeAsMoon (float resolution)
    {
        planet.InitializeAsMoon (resolution);
        orbit.InitializeAsMoon ();
        orbitRenderer.InitializeAsMoon ();
    }

    public void Zoom (float direction, float percentage)
    {
        orbit.IncreaseOrbit (direction, percentage);
        planet.Scale (direction, percentage);
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
        return this.planet.data.name;
    }

    public bool Equals (LocalController other)
    {
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