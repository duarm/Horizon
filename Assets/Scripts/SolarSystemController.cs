using Kurenaiz.Management.Events;
using UnityEngine;
using UnityEngine.Events;
public class SolarSystemController : MonoBehaviour
{
    #region Singleton
    private static SolarSystemController s_Instance;
    #endregion

    [Header ("Physics")]

    [Header ("Configuration")]
    [SerializeField] float sizeScalePercentage = 5f;
    [SerializeField] float orbitScalePercentage = 10f;
    [SerializeField] new CameraController camera;

    [SerializeField] LocalController[] planets;
    [SerializeField] Planet sun;

    [Header ("DEBBUGING")]
    [SerializeField] float timeScale = 1;
    //UnityAction onEnterUpdateBounds;

    [SerializeField] float sizeScale = 5f;
    [SerializeField] float orbitScale = 5f;

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
    public static float GetOrbitScale () => s_Instance.sizeScale;
    public static float GetSizeScale () => s_Instance.orbitScale;

    #region Debug
    [ContextMenu ("Update Orbit Scale")]
    public void UpdateOrbitScales ()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].orbit.CalculateScales ();
        }
    }

    [ContextMenu ("Update Size Scale")]
    public void UpdateSizeScales ()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planet.CalculateScales ();
        }
    }
    #endregion

    private void OnValidate ()
    {
        if (planets == null)
            planets = FindObjectsOfType<LocalController> ();
        if (camera == null)
            camera = FindObjectOfType<CameraController> ();
    }

    private void Awake ()
    {
        s_Instance = this;
    }

    // Start is called before the first frame update
    void Start ()
    {
        //EventManager.StartListening ("UpdateLineRenderer", onEnterUpdateBounds);
    }

    [ContextMenu ("Update Speed Scale")]
    public void SetSpeedScale ()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeScale);
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

    public void SetNameVisiblity (bool value)
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

    private void Zoom (float zoomRate, float zoomDistance)
    {
        if (zoomDistance >= 1 && ShowingOrbit)
        {
            //ShowingOrbit = false;
        }
        else if (zoomDistance == 0 && !ShowingOrbit)
        {
            //ShowingOrbit = true;
        }

        ScalePlanet (zoomRate);
        ExpandUniverse (zoomRate);
    }

    void SetOrbitVisiblity (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitVisiblity (value);
        }
    }

    void ScalePlanet (float direction)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planet.Scale (direction, sizeScalePercentage);
        }
    }

    void ExpandUniverse (float direction)
    {
        LocalController previousPlanet = null;
        for (int i = 0; i < planets.Length; i++)
        {
            if (i == 0)
            {
                previousPlanet = planets[i];
                continue;
            }

            planets[i].orbit.IncreaseOrbit (direction, orbitScalePercentage);

            previousPlanet = planets[i];
        }
    }

    public static void StartZoom (float zoomRate, float zoomDistance)
    {
        s_Instance.Zoom (zoomRate, zoomDistance);
    }
}