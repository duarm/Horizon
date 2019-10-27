using Kurenaiz.Management.Events;
using UnityEngine;
using UnityEngine.Events;
public class SolarSystemController : MonoBehaviour
{
    #region Singleton
    private static SolarSystemController s_Instance;
    #endregion

    [Header ("References")]
    [SerializeField] new CameraController camera;
    [SerializeField] LocalController[] planets;
    [Space]
    [SerializeField] LocalController sun;

    [Header ("Simulation")]
    [SerializeField] float timeScale = 1;

    [Header ("Configuration")]
    [Space]
    [SerializeField] Texture2D focusTexture;

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

    bool showingOrbit = true;
    bool upwardMotion = true;

    public static LocalController Sun () => s_Instance.sun;
    public static Texture2D FocusTexture () => s_Instance.focusTexture;

    void OnValidate ()
    {
        if (planets == null)
            planets = FindObjectsOfType<LocalController> ();
        if (camera == null)
            camera = FindObjectOfType<CameraController> ();
    }

    void Awake ()
    {
        s_Instance = this;
    }

    public static void Initialize (float scale)
    {
        s_Instance.UpdateScales (scale);
    }

    [ContextMenu ("Update Scales")]
    public void UpdateScales (float scale)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].orbit.CalculateScales (scale);
            planets[i].planet.CalculateScales (scale);
        }

        sun.planet.CalculateScales(scale);
    }

    [ContextMenu ("Update Speed Scale")]
    public void SetSpeedScale ()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeScale);
        }
    }

    private void FixedUpdate ()
    {
        if (upwardMotion)
            ToggleUpwardMotion ();
    }

    public void ToggleUpwardMotion ()
    {
        sun.rb.position = new Vector3 (0, sun.transform.localPosition.y + (10 * Time.deltaTime), 0);

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

    public void ToggleNameVisiblity (bool value)
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

    void Zoom (float direction, float percentage)
    {
        ScalePlanets (direction, percentage);
        ExpandUniverse (direction, percentage);
    }

    void SetOrbitVisiblity (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitVisiblity (value);
        }
    }

    void ScalePlanets (float direction, float percentage)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planet.Scale (direction, percentage);
        }

        sun.planet.Scale (direction, percentage);
    }

    void ExpandUniverse (float direction, float percentage)
    {
        LocalController previousPlanet = null;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].orbit.IncreaseOrbit (direction, percentage);
            previousPlanet = planets[i];
        }
    }

    public static void StartZoom (float direction, float percentage)
    {
        s_Instance.Zoom (direction, percentage);
    }
}