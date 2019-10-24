﻿using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, InputMaster.IGameplayActions
{
    protected static InputHandler s_Instance;
    public CameraController cameraController;
    InputMaster inputMaster;

    private void OnValidate()
    {
        if(cameraController == null)
            cameraController = GetComponent<CameraController>();
    }

    private void Awake ()
    {
        inputMaster = new InputMaster ();
        inputMaster.Gameplay.SetCallbacks (this);

        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException ("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    private void Update ()
    {
        var wheel = Input.GetAxis ("Mouse ScrollWheel");
        cameraController.ZoomCamera(wheel);
    }

    private void OnEnable ()
    {
        inputMaster.Enable ();
    }

    private void OnDisable ()
    {
        inputMaster.Disable ();
    }

    public void OnCameraMovement (InputAction.CallbackContext context)
    {
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

    /* public static Vector2 GetDirectionalInput()
    {
        return s_Instance.directionalInput;
    }

    public static float GetVerticalInput()
    {
        return s_Instance.directionalInput.y;
    }

    public static float GetHorizontalInput()
    {
        return s_Instance.directionalInput.x;
    } */
}