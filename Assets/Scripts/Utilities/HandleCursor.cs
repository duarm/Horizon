using Kurenaiz.Management.Events;
using UnityEngine;

public class HandleCursor : MonoBehaviour
{
    [Header ("Sprites")]
    [SerializeField] Texture2D grabCursor;
    [SerializeField] Texture2D normalCursor;
    [Header ("Configuration")]
    //[SerializeField] Transform hotspot;
    [SerializeField] Vector2 hotspot;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;

    void Start ()
    {
        EventManager.StartListening ("OnRightClickDown", GrabCursor);
        EventManager.StartListening ("OnRightClickUp", NormalCursor);
        NormalCursor ();
    }

    public void GrabCursor () => Cursor.SetCursor (grabCursor, hotspot, cursorMode);

    public void NormalCursor () => Cursor.SetCursor (normalCursor, hotspot, cursorMode);
}