﻿using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour, IEquatable<Planet>
{
    public string planetName;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<LocalController> moons;
    [SerializeField] TrailRenderer trail;
    [SerializeField] float sizeScaling = 1.1f;

    float originalSize;
    float maxSize;
    bool showName = true;

    GUIStyle guiStyle = new GUIStyle (); //create a new variable

    private void OnValidate ()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        guiStyle.normal.textColor = new Color (1, 0.53f, 0);
        guiStyle.fontSize = 20;
    }

    private void Start ()
    {
        CalculateScales();
    }

    public void CalculateScales()
    {
        originalSize = transform.localScale.x;
        maxSize = transform.localScale.x * SolarSystemController.GetSizeScale();
    }

    public void Scale (float direction, float rate)
    {
        var size = transform.localScale.x + (rate * maxSize * direction);
        if (size > maxSize)
            size = maxSize;
        else if (size < originalSize)
            size = originalSize;
        transform.localScale = new Vector3 (size, size, size);
    }

    public void SetNameVisilibity(bool value)
    {
        showName = value;
    }

    void OnGUI ()
    {
        if(showName)
        {
            var objectPos = mainCamera.WorldToScreenPoint (transform.position);
            if (objectPos.z > 0)
            {
                GUI.Label (new Rect (objectPos.x, Screen.height - objectPos.y - 20, 40, 20), planetName, guiStyle);
            }
        }
    }

    public bool Equals (Planet other)
    {
        return this.planetName.Equals (other.planetName);
    }
}