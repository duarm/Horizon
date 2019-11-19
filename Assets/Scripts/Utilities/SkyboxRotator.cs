using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    // Update is called once per frame
    float startValue;

    private void Start() {
        startValue = RenderSettings.skybox.GetFloat("_Rotation");
    }
    
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", startValue + Time.time * speed);
    }
}
