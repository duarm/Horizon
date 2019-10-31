using UnityEngine;
public class SolarSystemController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] new CameraController camera;
    [SerializeField] LocalController[] planets;
    [Space]
    [SerializeField] LocalController sun;

    [Header ("Simulation")]
    [SerializeField] float upwardMotionSpeed = 10f;
    [SerializeField] float timeScale = 1;

    [Header ("Configuration")]
    [Space]
    [SerializeField] Texture2D focusTexture;

    bool showingOrbit = true;
    bool upwardMotion = true;
    float resolution;

    public bool ShowingOrbit
    {
        set
        {
            showingOrbit = value;
            SetOrbitVisiblity (showingOrbit);
        }
        private get
        {
            return showingOrbit;
        }
    }

    public LocalController Sun { get { return sun; } }

    void OnValidate ()
    {
        if (planets == null)
            planets = FindObjectsOfType<LocalController> ();
        if (camera == null)
            camera = FindObjectOfType<CameraController> ();
    }

    public void Initialize (float scale)
    {
        resolution = UIController.Scale;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].InitializeAsPlanet (scale, resolution, focusTexture);
        }

        sun.InitializeAsPlanet (scale, resolution, focusTexture);
    }


    private void FixedUpdate ()
    {
        if (upwardMotion)
            ToggleUpwardMotion ();
    }

    [ContextMenu ("Update Speed Scale")]
    public void SetSpeedScale ()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeScale);
        }
    }

    public void ToggleUpwardMotion ()
    {
        sun.orbit.SetPosition (new Vector3 (0, sun.transform.localPosition.y + (upwardMotionSpeed * Time.fixedDeltaTime), 0));

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].orbitRenderer.Redraw ();
        }
    }

    public void SetSpeedScale (string value)
    {
        var timeScale = float.Parse (value);
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeScale);
        }
    }

    /*public void SetAllNamesVisibility (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planet.SetNameVisilibity (value);
        }
    }

    public void HideAllNamesExcept (Planet planet)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].planet.Equals (planet))
                continue;

            planets[i].planet.SetNameVisilibity (false);
        }
    }

    public void HideAllOrbitsExcept (Planet planet)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].planet.Equals (planet))
                continue;

            planets[i].orbitRenderer.SetOrbitVisiblity (false);
        }
    }*/

    public void EnterLocalSpace (LocalController local, bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].Equals (local))
            {
                local.EnterLocalSpace (value);
                continue;
            }

            planets[i].OnLocalSpace (!value);
        }
    }

    public void Zoom (float direction, float percentage)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].Zoom (direction, percentage);
        }
        sun.Zoom (direction, percentage);
    }

    void SetOrbitVisiblity (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitVisiblity (value);
        }
    }
}