using System.Collections;
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

    bool upwardMotion = true;
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
        resolution = UIController.Scale;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].InitializeAsPlanet (scale, resolution, focusTexture);
        }

        sun.InitializeAsPlanet (scale, resolution, focusTexture);

        if (upwardMotion)
            StartCoroutine (UpwardMotion ());
    }

    IEnumerator UpwardMotion ()
    {
        while (upwardMotion)
        {
            sun.SetPosition (new Vector3 (0, sun.transform.localPosition.y + (upwardMotionSpeed * Time.fixedDeltaTime), 0));

            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].Redraw ();
            }

            yield return null;
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

    public void SetSpeedScale (string value)
    {
        var timeScale = float.Parse (value);
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTimeScale (timeScale);
        }
    }

    public void EnterLocalSpace (LocalController local)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].Equals (local))
            {
                local.OnEnterLocalSpace ();
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

    void SetOrbitVisiblity (bool value)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetOrbitVisiblity (value);
        }
    }
}