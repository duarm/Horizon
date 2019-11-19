using Kurenaiz.Management.Events;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] GameObject[] rotate;
    [SerializeField] Vector3 axis;
    [SerializeField] float speed = 1;
    float timeScale = 1f;

    private void Start()
    {
        EventManager.SubscribeToTimeScaleChanged(UpdateTimeScale);
    }

    public void UpdateTimeScale(float timeScale) => this.timeScale = timeScale;

    void FixedUpdate()
    {
        for (int i = 0; i < rotate.Length; i++)
        {
            rotate[i].transform.Rotate(axis * speed * timeScale);     
        } 
    }
}
