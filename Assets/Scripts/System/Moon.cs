using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] Planet planet;
    [SerializeField] OrbitMotion orbit;
    [SerializeField] OrbitRenderer orbitRenderer;

    public void Initialize (float resolution)
    {
        if (planet)
            planet.InitializeAsMoon (resolution);
        if (orbit)
            orbit.InitializeAsMoon (planet.data);
        if (orbitRenderer)
            orbitRenderer.InitializeAsMoon ();

        orbit.transform.localEulerAngles = new Vector3 (0, 0, planet.data.inclination);
    }

    public void SetMoonVisibility (bool on)
    {
        gameObject.SetActive (on);
        planet.Toggle (on);
        orbitRenderer.Toggle (on);
        orbit.Toggle (on);
    }
}