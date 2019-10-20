using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Kurenaiz.Management.Events
{
    public class EventManager : MonoBehaviour {

        private Dictionary <string, UnityEvent> unityEventDictionary;
        private Dictionary <string, UnityEvent> eventDictionary;

        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init (); 
                    }
                }

                return eventManager;
            }
        }

        void Init ()
        {
            if (unityEventDictionary == null)
            {
                unityEventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        public static void StartListening (string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (instance.unityEventDictionary.TryGetValue (eventName, out thisEvent))
            {
                thisEvent.AddListener (listener);
            } 
            else
            {
                thisEvent = new UnityEvent ();
                thisEvent.AddListener (listener);
                instance.unityEventDictionary.Add (eventName, thisEvent);
            }
        }

        public static void StopListening (string eventName, UnityAction listener)
        {
            if (eventManager == null) return;
            UnityEvent thisEvent = null;
            if (instance.unityEventDictionary.TryGetValue (eventName, out thisEvent))
            {
                thisEvent.RemoveListener (listener);
            }
        }

        public static void TriggerEvent (string eventName)
        {
            UnityEvent thisEvent = null;
            if (instance.unityEventDictionary.TryGetValue (eventName, out thisEvent))
            {
                thisEvent.Invoke ();
            }
        }
    }
}