using Kurenaiz.Management.Events;
using TMPro;
using UnityEngine;

public class BottomTabController : MonoBehaviour
{
    [SerializeField] Animator bottomTabAnimator;
    [SerializeField] TMP_InputField timeInput;
    [SerializeField] float maxValue = 1000;

    public void SetTimeScale ()
    {
        var value = string.IsNullOrEmpty (timeInput.text) ? 1 : float.Parse (timeInput.text);
        if(value <= maxValue)
            EventManager.OnTimeScaleChanged?.Invoke (value);
        else
        {
            timeInput.text = maxValue.ToString();
            EventManager.OnTimeScaleChanged?.Invoke (maxValue);
        }
    }
}