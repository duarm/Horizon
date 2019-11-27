using System.Collections;
using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using UnityEngine;
public class SolarSystemController : MonoBehaviour //, IFixedUpdate
{
    [Header ("Configuration")]
    [Space]
    [SerializeField] Texture2D focusTexture;
    [SerializeField] int localOrbitResolutionIncreasing = 64;
    [SerializeField] float upwardMotionSpeed = 10f;

    [Header ("References")]
    [SerializeField] new CameraController camera;
    [SerializeField] LocalController[] planets;
    [Space]
    [SerializeField] LocalController sun;

    Coroutine upwardMotionCoroutine;
    bool upwardMotion = false;
    float resolution;
    float timeScale = 1;

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
        Popups ();
    }

    private void Popups ()
    {
        PopupController.Popup ("Bem vindo ao Horizon!", 5);
        PopupController.Popup ("O Horizon é um visualizador do sistema solar, não um simulador completo.", 10);
        PopupController.Popup ("Se tiver dúvida sobre algum termo ou icone, deixe seu mouse em cima ou consultar na aba 'Ajuda', no menu da direita.", 11);
    }

    public void ToggleUpwardMotion ()
    {
        upwardMotion = !upwardMotion;

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].OnUpwardMotion ();
        }
    }

    void Update ()
    {
        if (upwardMotion)
        {
            var newPos = new Vector3 (0, sun.transform.localPosition.y + (upwardMotionSpeed * timeScale * Time.deltaTime), 0);

            //sun.SetPosition (Vector3.Lerp (sun.transform.position, new Vector3 (0, sun.transform.localPosition.y + (upwardMotionSpeed * timeScale), 0), elapsedTime));
            sun.SetPosition (newPos);

            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].OnRedraw ();
            }

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
            planets[i].SetTimeScale (timeScale);
        }
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