using System.Collections;
using Kurenaiz.Management.Events;
using UnityEngine;
using UnityEngine.Profiling;

public class CameraController : MonoBehaviour
{
    [SerializeField] SolarSystemController solarSystem;
    [SerializeField] LayerMask focusMask;
    [SerializeField] Transform worldCoordinatesOverride;

    [Header ("Movement")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float movementSlowDownRate = .25f;
    [SerializeField] float transitionSmoothTime = .5f;

    [Header ("Orbit")]
    [SerializeField] float xSpeed = 20.0f;
    [SerializeField] float ySpeed = 20.0f;
    [SerializeField] float yMinLimit = -90f;
    [SerializeField] float yMaxLimit = 90f;
    [SerializeField] float smoothTime = 2f;

    [Header ("Zoom")]
    public int orbitShowThreshold = 10;
    [Range (0, 1)]
    [SerializeField] float circularOrbitScalePercentage = .05f;
    [SerializeField] float scale = 9f;

    Vector3 newPos;
    Vector3 oldPos;
    Vector3 currentZoomPos;
    Vector3 currentPos;
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
    [ReadOnly, SerializeField] int zoomMax;
    [ReadOnly, SerializeField] int zoomMin;
    [ReadOnly, SerializeField] int zoomLevel = 0;

    private void OnValidate ()
    {
        if (!solarSystem)
            solarSystem = FindObjectOfType<SolarSystemController> ();
    }

    private void Start ()
    {
        currentPos = transform.localPosition;
        currentZoomPos = transform.localPosition;
        oldPos = transform.localPosition;
        camera = Camera.main;
        sun = solarSystem.Sun;

        solarSystem.Initialize (scale);

        zoomMin = 0;
        zoomMax = Mathf.CeilToInt (1 / circularOrbitScalePercentage);
    }

    //TODO: UPDATE MANAGER
    void LateUpdate ()
    {
        //Profiler.BeginSample("Camera Movement");
        HandleRotation ();
        HandleZoom ();

        if (follower)
        {
            transition = transition ?? StartCoroutine (Transition ());
        }
        else
        {
            HandleMovement ();
            transform.SetPositionAndRotation (currentPos, currentRotation);
        }

        worldCoordinatesOverride.eulerAngles = new Vector3 (0, currentRotation.eulerAngles.y, 0);
        velocityX = Mathf.Lerp (velocityX, 0, Time.fixedDeltaTime * smoothTime);
        velocityY = Mathf.Lerp (velocityY, 0, Time.fixedDeltaTime * smoothTime);
        //Profiler.EndSample();
    }

    public void SetPosition (Vector3 newPos) => currentPos = newPos;
    public void Follow (Transform planet) => currentPos = planet.position;
    public void ZoomCamera (float input) => velocityZoom = input;

    public void MoveCamera (Vector3 input)
    {
        //slow down the movement as the zoom level increases to give the impression of a bigger universe
        var speed = (movementSpeed - (zoomLevel * movementSlowDownRate));
        speed = speed < 0 ? 0 : speed;
        Vector3 fowardMovement = worldCoordinatesOverride.forward * speed * input.y;
        Vector3 rightMovement = transform.right * speed * input.x;
        newPos = (fowardMovement + rightMovement) * Time.fixedDeltaTime;

        if (input != Vector3.zero)
            FocusOn (null);
    }

    IEnumerator Transition ()
    {
        Vector3 velocity = Vector3.zero;
        while (follower != null)
        {
            transform.SetPositionAndRotation (Vector3.SmoothDamp (transform.position, follower.orbit.transform.position, ref velocity, transitionSmoothTime), currentRotation);
            yield return 0;
        }

        transition = null;
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
        //EventManager.TriggerEvent ("OnFocus");
    }

    public void HandleZoom ()
    {
        var velocity = 0;

        if (velocityZoom > 0)
        {
            if (zoomLevel < zoomMax)
            {
                velocity = 1;
                solarSystem.Zoom (velocity, circularOrbitScalePercentage);
            }
            else if (follower)
            {
                if (zoomLevel == zoomMax)
                    solarSystem.Focus (follower, true);

                velocity = 1;
                Zoom (velocity);
            }
        }
        else if (velocityZoom < 0)
        {
            if (zoomLevel > zoomMin && zoomLevel <= zoomMax)
            {
                velocity = -1;
                solarSystem.Zoom (velocity, circularOrbitScalePercentage);
            }
            else if (follower)
            {
                if (zoomLevel == (zoomMax + 1))
                    solarSystem.Focus (follower, false);

                velocity = -1;
                Zoom (velocity);
            }
        }

        zoomLevel += velocity;
    }

    public void Zoom (int direction)
    {

    }

    public void HandleMovement ()
    {
        oldPos = transform.localPosition;
        var pos = oldPos + newPos;
        currentPos = new Vector3 (pos.x, sun.transform.position.y, pos.z);
    }

    public void HandleRotation ()
    {
        if (Input.GetKey (KeyCode.Mouse1))
        {
            velocityX += xSpeed * Input.GetAxis ("Mouse X") * 0.02f;
            velocityY += ySpeed * Input.GetAxis ("Mouse Y") * 0.02f;
        }

        rotationYAxis += velocityX * Time.fixedDeltaTime;
        rotationXAxis -= velocityY * Time.fixedDeltaTime;
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