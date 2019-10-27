using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour, IEquatable<Planet>
{
    public string planetName;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<LocalController> moons;
    [SerializeField] float sizeScaling = 1.1f;

    bool showName = true;
    bool beingFocused = false;

    [SerializeField, ReadOnly] float minSize;
    [SerializeField, ReadOnly] float maxSize;
    [SerializeField, ReadOnly] float sizeDifference;

    static Texture2D focusTexture;
    static readonly GUIStyle guiStyle = new GUIStyle ();

    private void OnValidate ()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        guiStyle.normal.textColor = new Color (1, 0.53f, 0);
        guiStyle.fontSize = 20;
    }

    private void Start ()
    {
        focusTexture = SolarSystemController.FocusTexture ();
    }

    public void CalculateScales (float scale)
    {
        minSize = transform.localScale.x;
        maxSize = transform.localScale.x * scale;
        sizeDifference = maxSize - minSize;
        Debug.Log("Calculating Scales");
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

    public void SetNameVisilibity (bool value)
    {
        showName = value;
    }

    public void SetFocus (bool value)
    {
        beingFocused = value;
    }

    void OnGUI ()
    {
        var planetPos = mainCamera.WorldToScreenPoint (transform.position);
        if (showName)
        {
            if (planetPos.z > 0)
            {
                GUI.Label (new Rect (planetPos.x, Screen.height - planetPos.y - 20, 40, 20), planetName, guiStyle);
            }
        }

        if (beingFocused)
        {
            GUI.DrawTexture (new Rect (planetPos.x - 20, Screen.height - planetPos.y - 20, 30, 30), focusTexture);
        }
    }

    public bool Equals (Planet other)
    {
        return this.planetName.Equals (other.planetName);
    }
}