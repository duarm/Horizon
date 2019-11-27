using Kurenaiz.Management.Events;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header ("GAME")]
    [Tooltip ("-1 to platform framerate")]
    [SerializeField] int InGame_Target_FrameRate = 60;
    [SerializeField] VsyncType InGame_Vsync_Count = VsyncType.NO_SYNC;
    [Header ("EDITOR")]
    [Tooltip ("-1 to platform framerate")]
    [SerializeField] int Editor_Target_FrameRate = 30;
    [SerializeField] VsyncType Editor_Vsync_Count = VsyncType.NO_SYNC;

    void Start ()
    {
        UpdateValues ();
    }

    [ContextMenu ("Rerun")]
    public void UpdateValues ()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = (int) Editor_Vsync_Count; // VSync must be disabled
        Application.targetFrameRate = Editor_Target_FrameRate;
#else
        QualitySettings.vSyncCount = (int) InGame_Vsync_Count; // VSync must be disabled
        Application.targetFrameRate = InGame_Target_FrameRate;
#endif
    }

    enum VsyncType
    {
        NO_SYNC,
        PANEL_REFRESH,
        HALF_PANEL_REFRESH
    }
}