using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController Instance;

    [SerializeField] Vector2 referenceResolution;
    [SerializeField] float scalerMatchWidthOrHeight = .5f;

    [SerializeField, ReadOnly] float scale;
    public static float Scale { get { return Instance.scale; } }

    private void Awake ()
    {
        Instance = this;
        //resolution change event
        RecalculateScale ();
    }

    public void RecalculateScale ()
    {
        scale = GetScale ();
    }

    private float GetScale ()
    {
        return Mathf.Pow (Screen.width / referenceResolution.x, 1f - scalerMatchWidthOrHeight) *
            Mathf.Pow (Screen.height / referenceResolution.y, scalerMatchWidthOrHeight);
    }
}