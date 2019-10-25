using UnityEngine;

public class UIController : MonoBehaviour
{
    Vector2 referenceResolution;
    float scalerMatchWidthOrHeight = .5f;

    public static float Scale;

    private void Awake() {
        //resolution change event
        RecalculateScale();
    }

    public void RecalculateScale()
    {
        Scale = GetScale();
    }

    private float GetScale ()
    {
        return Mathf.Pow (Screen.width / referenceResolution.x, 1f - scalerMatchWidthOrHeight) *
            Mathf.Pow (Screen.height / referenceResolution.y, scalerMatchWidthOrHeight);
    }
}