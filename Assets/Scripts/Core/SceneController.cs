using Kurenaiz.Management.Events;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] Texture2D grabCursor;
    [SerializeField] Texture2D normalCursor;
    [SerializeField] Vector2 hotspot = Vector2.zero;
    void Start ()
    {
        EventManager.StartListening ("OnRightClickDown", GrabCursor);
        EventManager.StartListening ("OnRightClickUp", NormalCursor);
    }

    public void GrabCursor ()
    {
        Cursor.SetCursor (grabCursor, hotspot, CursorMode.Auto);
    }

    public void NormalCursor ()
    {
        Cursor.SetCursor (normalCursor, hotspot, CursorMode.Auto);
    }
}