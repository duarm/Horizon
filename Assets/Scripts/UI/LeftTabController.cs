using Kurenaiz.Management.Events;
using UnityEngine;
using UnityEngine.UI;

public class LeftTabController : MonoBehaviour
{
    [SerializeField] GameObject leftBar;
    [SerializeField] Animator leftTabAnimator;

    [Header ("Skyboxes")]
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] Material skybox4;

    #region LeftTab

    private void OnValidate ()
    {
        if (!leftBar)
            leftBar = transform.GetChild (0).gameObject;
    }

    private void Start ()
    {
        EventManager.StartListening ("OnEscape", OpenLeftBar);
    }

    public void OpenLeftBar ()
    {
        leftBar.SetActive (true);
    }

    public void HideLeftTab ()
    {
        leftTabAnimator.SetTrigger ("Pressed");
    }

    public void ShowOrbits (bool show)
    {
        EventManager.OnShowOrbits?.Invoke (show);
    }

    public void UpwardMotion (bool value)
    {
        EventManager.TriggerEvent("UpwardMotion");
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