using UnityEngine;
using Kurenaiz.Management.Events;

public class TriggerNotifier : MonoBehaviour
{
    // TODO: INCREASE ORBIT RES
    bool startUpdate = false;
    private void OnTriggerEnter(Collider other)
    {
        if(startUpdate)
        {
            EventManager.TriggerEvent("UpdateLineRenderer");
        }
        else
        {
            startUpdate = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(startUpdate)
        {
            EventManager.TriggerEvent("UpdateLineRenderer");
        }
        else
        {
            startUpdate = true;
        }
    }
}
