﻿using System.Collections;
using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using UnityEngine;

public class CameraController : MonoBehaviour, IStart
{
    [SerializeField] SolarSystemController solarSystem;
    [SerializeField] LayerMask focusMask;
    [SerializeField] Transform worldCoordinatesOverride;

    [Header ("Movement")]
    [SerializeField] float normalSpeed = 500f;
    [SerializeField] float speedMultiplier = 2f;
    [SerializeField] float movementSlowDownRate = .25f;
    [SerializeField] float transitionSmoothTime = .5f;

    [Header ("Orbit")]
    [SerializeField] float xSpeed = 20.0f;
    [SerializeField] float ySpeed = 20.0f;
    [SerializeField] float yMinLimit = -90f;
    [SerializeField] float yMaxLimit = 90f;
    [SerializeField] float smoothTime = 2f;
    [Header ("Zoom")]
    [SerializeField] int zoomInThreshold = 90;
    [SerializeField] int zoomOutThreshold = 90;
    [SerializeField] float zoomSpeed = 10f;

    [Range (0, 1)]
    [SerializeField] float circularOrbitScalePercentage = .05f;
    [SerializeField] float scale = 9f;

    Vector3 newPos;
    Vector3 oldPos;
    Vector3 currentZoomPos;
    Vector3 currentPosition;
    Vector3 currentVelocity;
    Quaternion currentRotation;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    float velocityZoom = 0.0f;

    new Camera camera;
    Coroutine transition;
    LocalController follower;
    LocalController sun;
    new Rigidbody rigidbody;
    [ReadOnly, SerializeField] int zoomMax;
    [ReadOnly, SerializeField] int zoomMin;
    [ReadOnly, SerializeField] int zoomLevel = 0;

    public void SetPosition (Vector3 newPos) => currentPosition = newPos;
    public void Follow (Transform planet) => currentPosition = planet.position;
    public void ZoomCamera (float input) => velocityZoom = input;

    private void OnValidate ()
    {
        if (!solarSystem)
            solarSystem = FindObjectOfType<SolarSystemController> ();
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody> ();
    }

    private void OnEnable()
    {
        UpdateManager.Subscribe(this);
    }

    private void OnDisable()
    {
        UpdateManager.Unsubscribe(this);
    }

    void IStart.MStart ()
    {
        currentPosition = transform.localPosition;
        currentZoomPos = transform.localPosition;
        oldPos = transform.localPosition;
        camera = Camera.main;
        sun = solarSystem.Sun;

        solarSystem.Initialize (scale);

        zoomMin = 0;
        zoomMax = Mathf.CeilToInt (1 / circularOrbitScalePercentage);
    }

    void FixedUpdate ()
    {
        HandleRotation ();
        HandleZoom ();

        if (follower)
        {
            var target = follower.Orbit.Position;
            transform.SetPositionAndRotation (Vector3.SmoothDamp (transform.position, target, ref currentVelocity, transitionSmoothTime), currentRotation);
        }
        else
        {
            HandleMovement ();
            //rigidbody.MovePosition (currentPosition);
            //rigidbody.MoveRotation (currentRotation);
            rigidbody.position = currentPosition;
            rigidbody.rotation = currentRotation;
            //camera.transform.SetPositionAndRotation(currentPosition , currentRotation);
            //transform.SetPositionAndRotation (currentPosition, currentRotation);
        }

        worldCoordinatesOverride.eulerAngles = new Vector3 (0, currentRotation.eulerAngles.y, 0);
        velocityX = Mathf.Lerp (velocityX, 0, Time.deltaTime * smoothTime);
        velocityY = Mathf.Lerp (velocityY, 0, Time.deltaTime * smoothTime);
    }

    public void MoveCamera (Vector3 input)
    {
        //cannot move while zoomed
        if (zoomLevel > zoomMax)
            return;

        //slow down the movement as the zoom level increases to give the impression of a bigger universe
        var speed = (normalSpeed - (zoomLevel * movementSlowDownRate));
        speed = speed < 0 ? 0 : speed;
        Vector3 fowardMovement = worldCoordinatesOverride.forward * speed * input.y;
        Vector3 rightMovement = transform.right * speed * input.x;
        newPos = (fowardMovement + rightMovement) * Time.deltaTime;

        if (input != Vector3.zero && follower)
            Unfocus ();
    }

    public void Focus ()
    {
        var ray = camera.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray, out var hit, 100000, focusMask))
        {
            //TODO: PHYSICS CACHING
            LocalController planet = hit.collider.GetComponentInParent<LocalController> ();
            FocusOn (planet);
        }
    }

    public void FocusOn (LocalController planet)
    {
        follower?.SetFocus (false);
        follower = planet;
        follower?.SetFocus (true);

        EventManager.OnFocus?.Invoke (planet);
    }

    public void Unfocus ()
    {
        follower?.SetFocus (false);
        follower = null;
    }

    public void HandleZoom ()
    {
        var velocity = 0;

        if (velocityZoom > 0)
        {
            if (zoomLevel < zoomMax && zoomLevel >= zoomMin)
            {
                velocity = 1;
                solarSystem.Zoom (velocity, circularOrbitScalePercentage);
            }
            else
            {
                if (zoomLevel == zoomMax && follower)
                    solarSystem.EnterLocalSpace (follower);

                if ((zoomMax + zoomInThreshold) > zoomLevel && follower)
                {
                    velocity = 1;
                    Zoom (velocity);
                }
                else if (zoomLevel < zoomMin)
                {
                    velocity = 1;
                    Zoom (velocity);
                }
            }
        }
        else if (velocityZoom < 0)
        {
            if (zoomLevel > zoomMin && zoomLevel <= zoomMax)
            {
                velocity = -1;
                solarSystem.Zoom (velocity, circularOrbitScalePercentage);
            }
            else
            {
                if (zoomLevel == (zoomMax + 1))
                    solarSystem.ExitLocalSpace (follower);

                if ((zoomMin - zoomOutThreshold) < zoomLevel)
                {
                    velocity = -1;
                    Zoom (velocity);
                }
            }
        }

        zoomLevel += velocity;
    }

    public void Zoom (int direction)
    {
        var pos = camera.transform.localPosition;
        camera.transform.position += camera.transform.forward * zoomSpeed * direction;
    }

    public void HandleMovement ()
    {
        var speeding = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
        oldPos = transform.localPosition;
        var pos = oldPos + newPos * (speeding ? speedMultiplier : 1);
        currentPosition = new Vector3 (pos.x, sun.transform.position.y, pos.z);
    }

    public void HandleRotation ()
    {
        if (Input.GetKeyDown (KeyCode.Mouse1))
            EventManager.TriggerEvent ("OnRightClickDown");

        if (Input.GetKeyUp (KeyCode.Mouse1))
            EventManager.TriggerEvent ("OnRightClickUp");

        if (Input.GetKey (KeyCode.Mouse1))
        {
            velocityX += xSpeed * Input.GetAxis ("Mouse X") * 0.02f;
            velocityY += ySpeed * Input.GetAxis ("Mouse Y") * 0.02f;
        }

        rotationYAxis += velocityX * Time.deltaTime;
        rotationXAxis -= velocityY * Time.deltaTime;
        rotationXAxis = ClampAngle (rotationXAxis, yMinLimit, yMaxLimit);
        Quaternion toRotation = Quaternion.Euler (rotationXAxis, rotationYAxis, 0);
        currentRotation = toRotation;
    }

    public float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp (angle, min, max);
    }
}