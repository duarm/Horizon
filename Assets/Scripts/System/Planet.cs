using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    [SerializeField] PlanetData data;
    [SerializeField] GameObject displayPlanet;
    [SerializeField] Camera mainCamera;
    [SerializeField] LocalController[] moons;

    [SerializeField, ReadOnly] float minSize;
    [SerializeField, ReadOnly] float maxSize;
    [SerializeField, ReadOnly] float sizeDifference;

    MeshRenderer mesh;
    Texture2D focusTexture;
    bool showName = true;
    bool beingFocused = false;

    static readonly GUIStyle guiStyle = new GUIStyle ();

    public PlanetData Data { get { return data; } }
    public GameObject DisplayPlanet { get { return displayPlanet; } }

    private void OnValidate ()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (mesh == null)
            mesh = GetComponent<MeshRenderer> ();

        int i = 0;
        if (data.moonCount != 0)
        {
            moons = new LocalController[data.moonCount];
            foreach (Transform moon in transform)
            {
                if (moons.Length == i)
                    break;

                moons[i++] = moon.GetComponent<LocalController> ();
            }
        }
    }

    private void Start ()
    {
        guiStyle.normal.textColor = new Color (1, 0.53f, 0);
    }

    public void InitializeAsPlanet (float scale, float resolution, Texture2D texture)
    {
        Initialize (scale, resolution, texture);
        CalculateScales (scale);
        focusTexture = texture;
        InitializeMoons (resolution);
    }

    private void InitializeMoons (float resolution)
    {
        foreach (var moon in moons)
        {
            if (moon == null)
                break;

            moon.InitializeAsMoon (resolution);
        }
    }

    public void Toggle (bool on)
    {
        SetNameVisilibity (on);
    }

    public void InitializeAsMoon (float resolution)
    {
        showName = false;
        SetMeshVisibility (false);
    }

    private void Initialize (float scale, float resolution, Texture2D texture)
    {
        guiStyle.fontSize = Mathf.FloorToInt (15 * resolution);
    }

    void CalculateScales (float scale)
    {
        minSize = transform.localScale.x;
        maxSize = transform.localScale.x * scale;
        sizeDifference = maxSize - minSize;
    }

    public void Scale (float direction, float rate)
    {
        var increasedBy = rate * sizeDifference * direction;
        var newSize = transform.localScale.x + increasedBy;
        if (newSize > maxSize)
            newSize = maxSize;
        else if (newSize < minSize)
            newSize = minSize;
        transform.localScale = new Vector3 (newSize, newSize, newSize);
    }

    public void ToggleDisplayPlanet (bool value)
    {
        displayPlanet.SetActive(value);
    }

    public void OnEnterLocalSpace ()
    {
        SetMoonsVisibility (true);
        SetMeshVisibility (true);
    }

    public void OnExitLocalSpace ()
    {
        SetMoonsVisibility (false);
        SetMeshVisibility (false);
    }

    public void SetMeshVisibility (bool show)
    {
        if (mesh)
            mesh.enabled = show;
    }

    public void SetNameVisilibity (bool show)
    {
        showName = show;
    }

    public void SetFocus (bool value)
    {
        beingFocused = value;
    }

    public void SetTimeScale(float value)
    {
        for (int i = 0; i < moons.Length; i++)
        {
            moons[i]?.SetTimeScale(value);
        }
    }

    public void SetMoonsVisibility (bool on)
    {
        if (moons == null)
            return;

        for (int i = 0; i < moons.Length; i++)
        {
            if (moons[i] == null)
            {
                Debug.LogWarning($"The planet {Data.name} has a null moon.");
                break;
            }

            moons[i].SetMoonVisibility (on);
        }
    }

    void OnGUI ()
    {
        var planetPos = mainCamera.WorldToScreenPoint (transform.position);
        if (showName)
        {
            if (planetPos.z > 0)
            {
                GUI.Label (new Rect (planetPos.x, Screen.height - planetPos.y - 20, 40, 20), data.name, guiStyle);
            }
        }

        if (beingFocused)
        {
            GUI.DrawTexture (new Rect (planetPos.x - 20, Screen.height - planetPos.y - 20, 30, 30), focusTexture);
        }
    }
}