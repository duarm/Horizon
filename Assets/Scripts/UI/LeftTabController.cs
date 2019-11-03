using Kurenaiz.Management.Events;
using UnityEngine;
using UnityEngine.UI;

public class LeftTabController : MonoBehaviour
{
    [SerializeField] Animator leftTabAnimator;
    [SerializeField] Dropdown orbitTypeDropdown;

    [Header ("Skyboxes")]
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] Material skybox4;

    #region LeftTab

    public void ToggleLeftBar ()
    {

    }

    public void HideLeftTab ()
    {
        leftTabAnimator.SetTrigger ("Pressed");
    }

    public void ShowOrbits (bool show)
    {
        EventManager.OnShowOrbits?.Invoke (show);
    }

    public void SetOrbitType (int index)
    {
        EventManager.SetOrbitType?.Invoke ((OrbitType) index);
    }

    public void Quit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
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

    #endregion
}