﻿using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour, IEquatable<Planet>
{
    public string planetName;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform moonsParent;
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

        if (moonsParent != null)
        {
            if (moonsParent.childCount != moons.Count)
            {
                foreach (Transform moon in moonsParent)
                {
                    moons.Add (moon.GetComponent<LocalController> ());
                }
            }
        }
    }

    private void Start() {
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
            moon.InitializeAsMoon (resolution);
        }
    }

    public void StateAsMoon (bool on)
    {
        SetNameVisilibity (on);
    }

    public void InitializeAsMoon (float resolution)
    {
        showName = false;
        mesh.enabled = false;
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

    public void OnLocalSpace (bool value)
    {
        SetNameVisilibity (value);
    }

    public void EnterLocalSpace (bool value)
    {
        SetMoonsVisibility (value);
        SetMeshVisibility (value);
    }

    public void SetMeshVisibility (bool show)
    {
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

    public void SetMoonsVisibility (bool on)
    {
        for (int i = 0; i < moons.Count; i++)
        {
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