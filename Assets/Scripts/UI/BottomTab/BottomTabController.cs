using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class BottomTabController : MonoBehaviour
{
    [SerializeField] Animator bottomTabAnimator;
    [SerializeField] TMP_InputField timeInput;

    public void SetTimeScale ()
    {
        EventManager.OnTimeScaleChanged?.Invoke (string.IsNullOrEmpty (timeInput.text) ? 1 : float.Parse (timeInput.text));
    }
}