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
    [SerializeField] LocalController sun;

    [Header ("Configuration")]
    [Range (0, 1)]
    [SerializeField] float sizeScalePercentage = .05f;
    [Range (0, 1)]
    [SerializeField] float orbitScalePercentage = .1f;
    [SerializeField] float timeScale = 1;
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
    bool upwardMotion = true;
    float zoomValue = 0;

    public static float GetOrbitScale () => s_Instance.sizeScale;
    public static float GetSizeScale () => s_Instance.orbitScale;
    public static float GetZoomValue () => s_Instance.zoomValue;
    public static LocalController Sun () => s_Instance.sun;

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

    [ContextMenu ("Update Speed Scale")]
    public void SetSpeedScale ()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeScale);
        }
    }
    #endregion

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

    // Start is called before the first frame update
    void Start ()
    {
        //EventManager.StartListening ("UpdateLineRenderer", onEnterUpdateBounds);
    }

    private void FixedUpdate ()
    {
        if (upwardMotion)
            ToggleUpwardMotion ();
    }

    public void ToggleUpwardMotion ()
    {
        sun.transform.localPosition = new Vector3 (0, sun.transform.localPosition.y + (10 * Time.deltaTime), 0);

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

    void Zoom (float direction, OrbitMotion follower)
    {
        /*if (zoomDistance >= 1 && ShowingOrbit)
        {
            //ShowingOrbit = false;
        }
        else if (zoomDistance == 0 && !ShowingOrbit)
        {
            //ShowingOrbit = true;
        }*/

        ScalePlanets (direction);
        ExpandUniverse (direction, follower);
    }

    void SetOrbitVisiblity (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitVisiblity (value);
        }
    }

    void ScalePlanets (float direction)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planet.Scale (direction, sizeScalePercentage);
        }

        sun.planet.Scale (direction, sizeScalePercentage);
    }

    void ExpandUniverse (float direction, OrbitMotion follower)
    {
        LocalController previousPlanet = null;
        float difference = 0;

        for (int i = 0; i < planets.Length; i++)
        {
            difference = planets[i].orbit.IncreaseOrbit (direction, orbitScalePercentage);
            previousPlanet = planets[i];
            zoomValue += difference != 0 ? direction : 0;
        }
    }

    public static void StartZoom (float zoomRate, OrbitMotion follower)
    {
        s_Instance.Zoom (zoomRate, follower);
    }
}