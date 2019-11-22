using System.Collections;
using Kurenaiz.Management.Events;
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
    [SerializeField] int localOrbitResolutionIncreasing = 64;

    Coroutine upwardMotionCoroutine;
    bool upwardMotion = false;
    bool timeStopped = false;
    float resolution;

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
        EventManager.SubscribeToShowOrbits (ToggleOrbit);
        EventManager.SubscribeToSetOrbitType (SetOrbitType);
        EventManager.SubscribeToTimeScaleChanged (SetTimeScale);
        EventManager.StartListening ("UpwardMotion", ToggleUpwardMotion);

        resolution = UIController.Scale;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].InitializeAsPlanet (scale, resolution, focusTexture);
        }

        sun.InitializeAsPlanet (scale, resolution, focusTexture);
    }

    private void Start() {
        EventManager.OnTimeScaleChanged?.Invoke(timeScale);
    }

    public void ToggleUpwardMotion ()
    {
        upwardMotion = !upwardMotion;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].OnUpwardMotion ();
        }

        if (upwardMotion)
            upwardMotionCoroutine = StartCoroutine (UpwardMotion ());
        else
            StopCoroutine (upwardMotionCoroutine);
    }

    IEnumerator UpwardMotion ()
    {
        while (upwardMotion)
        {
            sun.SetPosition (new Vector3 (0, sun.transform.localPosition.y + (upwardMotionSpeed * timeScale * Time.fixedDeltaTime), 0));

            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].OnRedraw ();
            }

            yield return null;
        }
    }

    public void SetOrbitType (OrbitType type)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitType (type);
        }
    }

    public void SetTimeScale (float value)
    {
        timeScale = value;
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeStopped ? 0 : timeScale);
        }
    }

    public void ZaWarudo (bool stop)
    {
        timeStopped = stop;
        SetTimeScale (timeScale);
    }

    public void EnterLocalSpace (LocalController local)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].Equals (local))
            {
                local.OnEnterLocalSpace (localOrbitResolutionIncreasing);
                continue;
            }

            planets[i].SetVisibility (false);
        }
    }

    public void ExitLocalSpace (LocalController local)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].Equals (local))
            {
                local.OnExitLocalSpace ();
                continue;
            }

            planets[i].SetVisibility (true);
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

    public void ToggleOrbit (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].ToggleOrbit (value);
        }
    }

    void SetOrbitVisiblity (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitVisiblity (value);
        }
    }
}