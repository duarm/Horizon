using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController Instance;
    public LayerMask focusMask;
    public Transform worldCoordinatesOverride;

    [Header ("Movement")]
    public float movementSpeed = 5f;
    public float movementSlowDownRate = .25f;
    public float transitionDuration = 2.5f;
    public float transitionSmoothTime = .5f;

    [Header ("Orbit")]
    public float xSpeed = 20.0f;
    public float ySpeed = 20.0f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float smoothTime = 2f;

    [Header ("Zoom")]
    public int orbitShowThreshold = 10;

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
    int zoomValue = 0;

    new Camera camera;
    Coroutine transition;
    public OrbitMotion follow;

    private void Awake ()
    {
        Instance = this;
    }

    private void Start ()
    {
        currentPos = transform.localPosition;
        currentZoomPos = transform.localPosition;
        oldPos = transform.localPosition;

        camera = Camera.main;
    }

    public void MoveCamera (Vector3 input)
    {
        Vector3 fowardMovement = worldCoordinatesOverride.forward * movementSpeed * input.y;
        Vector3 rightMovement = transform.right * movementSpeed * input.x;
        newPos = (fowardMovement + rightMovement) * Time.fixedDeltaTime;

        if (input != Vector3.zero)
            follow = null;
    }

    public void ZoomCamera (float input)
    {
        velocityZoom = input;
    }

    //TODO: UPDATE MANAGER
    void LateUpdate ()
    {
        HandleMovement ();
        HandleRotation ();
        HandleZoom ();

        if(!follow)
        {
            transform.SetPositionAndRotation (currentPos, currentRotation);
        }
        else
        {
            transition = transition ?? StartCoroutine(Transition());
        }

        worldCoordinatesOverride.eulerAngles = new Vector3 (0, currentRotation.eulerAngles.y, 0);
        velocityX = Mathf.Lerp (velocityX, 0, Time.fixedDeltaTime * smoothTime);
        velocityY = Mathf.Lerp (velocityY, 0, Time.fixedDeltaTime * smoothTime);
    }

    IEnumerator Transition ()
    {
        Vector3 velocity = Vector3.zero;
        while (follow != null)
        {
            transform.SetPositionAndRotation (Vector3.SmoothDamp (transform.position, follow.transform.position, ref velocity, transitionSmoothTime), currentRotation);
            yield return 0;
        }

        transition = null;
    }

    public void SetPosition (Vector3 newPos) => currentPos = newPos;
    public void Follow (Transform planet) => currentPos = planet.position;

    public void Focus ()
    {
        var ray = camera.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray, out var hit, 100000, focusMask))
        {
            //TODO: PHYSICS CACHING
            OrbitMotion planet = hit.transform.GetComponent<OrbitMotion> ();
            Debug.Log ("Focusing:" + planet.name);
            transform.position = hit.transform.position;
            follow = planet;
        }
    }

    public void HandleZoom ()
    {
        if (velocityZoom != 0)
        {
            var velocity = velocityZoom > 0 ? 1 : -1;
            zoomValue += velocity;
            if (zoomValue < 0)
                zoomValue = 0;

            SolarSystemController.StartZoom (velocity, zoomValue);
        }
    }

    public void HandleMovement ()
    {
        oldPos = transform.localPosition;
        currentPos = oldPos + newPos;
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