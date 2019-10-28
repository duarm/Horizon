using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour, IEquatable<Planet>
{
    public string planetName;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<LocalController> moons;

    bool showName = true;
    bool beingFocused = false;

    [SerializeField, ReadOnly] float minSize;
    [SerializeField, ReadOnly] float maxSize;
    [SerializeField, ReadOnly] float sizeDifference;

    MeshRenderer mesh;
    Texture2D focusTexture;
    static readonly GUIStyle guiStyle = new GUIStyle ();

    private void OnValidate ()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (mesh == null)
            mesh = GetComponent<MeshRenderer> ();
        guiStyle.normal.textColor = new Color (1, 0.53f, 0);
    }

    public void InitializeAsPlanet (float scale, Texture2D texture, float resolution)
    {
        CalculateScales (scale);
        focusTexture = texture;
        guiStyle.fontSize = Mathf.FloorToInt(15 * resolution);
        Debug.Log(resolution);
    }

    public void InitializeAsMoon ()
    {

    }

    public void Disable ()
    {
        mesh.enabled = false;
        showName = false;
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

    public void SetMeshVisibility (bool value)
    {
        mesh.enabled = value;
    }

    public void SetNameVisilibity (bool value)
    {
        showName = value;
    }

    public void SetFocus (bool value)
    {
        beingFocused = value;
    }

    public void SetMoonsVisibility (bool value)
    {
        for (int i = 0; i < moons.Count; i++)
        {
            moons[i].planet.InitializeAsMoon ();
        }
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