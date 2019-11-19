using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class BottomTabController : MonoBehaviour
{
    [SerializeField] Animator bottomTabAnimator;
    [SerializeField] TMP_InputField timeInput;
    [SerializeField] float maxValue = 1000;
    bool timeStopped = false;
    float timeScale = 1;

    public void SetTimeScale ()
    {
        if (timeStopped)
        {
            EventManager.OnTimeScaleChanged?.Invoke (0);
            return;
        }

        var value = string.IsNullOrEmpty (timeInput.text) ? 1 : float.Parse (timeInput.text);

        if (value <= maxValue)
            EventManager.OnTimeScaleChanged?.Invoke (value);
        else
        {
            value = maxValue;
            EventManager.OnTimeScaleChanged?.Invoke (maxValue);
        }

        timeInput.text = value.ToString ();

    }

    public void ZaWarudo (bool stop)
    {
        timeStopped = stop;
        SetTimeScale ();
    }
}