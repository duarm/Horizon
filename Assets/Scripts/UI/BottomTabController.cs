using UnityEngine;
using TMPro;

public class BottomTabController : MonoBehaviour
{
    [SerializeField] Animator bottomTabAnimator;

    [SerializeField] TMP_InputField timeInput;

    [SerializeField] SolarSystemController solarSystem;

    private void OnValidate() {
        if(!solarSystem)
            solarSystem = FindObjectOfType<SolarSystemController>();
    }

    public void SetTimeScale()
    {
        solarSystem.SetTimeScale(string.IsNullOrEmpty(timeInput.text) ? 1 : float.Parse(timeInput.text));
    }

    public void SetTimeScale(float value)
    {
        solarSystem.SetTimeScale(value);
    }
}