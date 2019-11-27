using Horizon.Input;
using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, InputMaster.IGameActions, IUpdate
{
    //TODO: TIGHT SCHEDULE, EXTINGUISH THE OLD INPUT SYSTEM
    protected static InputHandler s_Instance;
    public CameraController cameraController;
    InputMaster inputMaster;

    private void OnValidate ()
    {
        if (cameraController == null)
            cameraController = GetComponent<CameraController> ();
    }

    private void Awake ()
    {
        inputMaster = new InputMaster ();
        inputMaster.Game.SetCallbacks (this);

        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException ("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    private void OnEnable ()
    {
        UpdateManager.Subscribe (this);
        inputMaster.Enable ();
    }

    private void OnDisable ()
    {
        UpdateManager.Unsubscribe (this);
        inputMaster.Disable ();
    }

    void IUpdate.MUpdate ()
    {
        cameraController.ZoomCamera (Input.GetAxis ("Mouse ScrollWheel"));
    }

    public void OnCameraMovement (InputAction.CallbackContext context)
    {
        /*
        if(context.performed)
        {
            EventManager.TriggerEvent("OnRightClickDown");
        }

        if(context.canceled)
        {
            EventManager.TriggerEvent("OnRightClickUp");
        }*/

        cameraController.MoveCamera (context.ReadValue<Vector2> ());
    }

    public void OnZoom (InputAction.CallbackContext context)
    {
        //cameraController.ZoomCamera (context.ReadValue<float> () / 240);
    }

    public void OnFocus (InputAction.CallbackContext context)
    {
        if (context.performed)
            cameraController.Focus ();
    }

    public void OnEscape (InputAction.CallbackContext context)
    {
        if (context.performed)
            EventManager.TriggerEvent ("OnEscape");
    }
}