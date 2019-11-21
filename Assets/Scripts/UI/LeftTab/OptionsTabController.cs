using Kurenaiz.Management.Events;
using UnityEngine;

public class OptionsTabController : MonoBehaviour
{
    [Header ("Skyboxes")]
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] Material skybox4;

    public void ShowOrbits (bool show)
    {
        EventManager.OnShowOrbits?.Invoke (show);
    }

    public void UpwardMotion (bool value)
    {
        EventManager.TriggerEvent ("UpwardMotion");
    }

    public void SetOrbitType (int index)
    {
        EventManager.SetOrbitType?.Invoke ((OrbitType) index);
    }

    public void ChangeSkybox (int index)
    {
        switch (index)
        {
            case 1:
                RenderSettings.skybox = skybox1;
                break;
            case 2:
                RenderSettings.skybox = skybox2;
                break;
            case 3:
                RenderSettings.skybox = skybox3;
                break;
            case 4:
                RenderSettings.skybox = skybox4;
                break;
        }
    }

}