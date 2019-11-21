using Kurenaiz.Management.Core;
using Kurenaiz.Management.Events;
using UnityEngine;

public class Rotator : MonoBehaviour, IFixedUpdate, IStart
{
    [SerializeField] GameObject[] rotate;
    [SerializeField] Vector3 axis;
    [SerializeField] float speed = 1;
    float timeScale = 1f;

    private void OnEnable()
    {
        UpdateManager.Subscribe(this as IFixedUpdate);
        UpdateManager.Subscribe(this as IStart);
    }

    private void OnDisable()
    {
        UpdateManager.Unsubscribe(this as IFixedUpdate);
        UpdateManager.Unsubscribe(this as IStart);
    }

    void IStart.MStart ()
    {
        EventManager.SubscribeToTimeScaleChanged (UpdateTimeScale);
    }

    void IFixedUpdate.MFixedUpdate ()
    {
        for (int i = 0; i < rotate.Length; i++)
        {
            rotate[i].transform.Rotate (axis * speed * timeScale * Time.fixedDeltaTime);
        }
    }

    public void UpdateTimeScale (float timeScale) => this.timeScale = timeScale;
}