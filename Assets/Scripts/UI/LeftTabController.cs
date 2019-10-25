using UnityEngine;

public class LeftTabController : MonoBehaviour
{
    [SerializeField] Animator leftTabAnimator;

    #region LeftTab

    public void ToggleLeftBar ()
    {

    }

    public void HideLeftTab ()
    {
        leftTabAnimator.SetTrigger ("Pressed");
    }

    public void Quit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public void Options ()
    {

    }

    public void BackToMenu ()
    {

    }

    #endregion
}