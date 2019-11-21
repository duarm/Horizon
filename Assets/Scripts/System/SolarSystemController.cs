using System.Collections;
using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using UnityEngine;
public class SolarSystemController : MonoBehaviour, IStart
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

    private void OnEnable()
    {
        UpdateManager.Subscribe(this);
    }

    private void OnDisable()
    {
        UpdateManager.Unsubscribe(this);
    }

    void IStart.MStart()
    {
        // expanda a aba para mais informações.
        PopupController.Popup("Bem vindo ao Horizon!", 5);
        PopupController.Popup("O Horizon é um visualizador do sistema solar, não um simulador completo.", 13);
        PopupController.Popup("O intuito do programa é observar o sistema solar de uma maneira mais intuitiva.", 14);
        PopupController.Popup("Expanda os menus da direita ou da esquerda para saber suas funções e navegar pelo Horizon.", 15);
        PopupController.Popup("Se tiver dúvida sobre algum termo ou icone, deixe seu mouse em cima ou consultar na aba 'Ajuda', no menu da direita.", 16);
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