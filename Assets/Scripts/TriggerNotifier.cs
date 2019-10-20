using UnityEngine;
using Kurenaiz.Management.Events;

public class TriggerNotifier : MonoBehaviour
{
    bool startUpdate = false;
    private void OnTriggerEnter(Collider other)
    {
        if(startUpdate)
        {
            //Debug.Log("Exiting " + this.gameObject.name);
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
            //Debug.Log("Exiting " + this.gameObject.name);
            EventManager.TriggerEvent("UpdateLineRenderer");
        }
        else
        {
            startUpdate = true;
        }
    }
}
