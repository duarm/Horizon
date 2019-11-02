using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kurenaiz.Management.Events
{
    public class EventManager : MonoBehaviour
    {

        private Dictionary<string, Action> eventDictionary;

        public static Action<PlanetData> OnFocus { get; private set; }

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
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, Action> ();
            }
        }

        public static void SubscribeToFocus(Action<PlanetData> action)
        {
            OnFocus += action;
        }

        public static void StartListening (string eventName, Action listener)
        {
            Action thisEvent = null;
            if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
            {
                thisEvent += listener;
            }
            else
            {
                thisEvent += listener;
                instance.eventDictionary.Add (eventName, thisEvent);
            }
        }

        public static void StopListening (string eventName, Action listener)
        {
            if (eventManager == null) return;
            Action thisEvent = null;
            if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
            {
                thisEvent -= listener;
            }
        }

        public static void TriggerEvent (string eventName)
        {
            Action thisEvent = null;
            if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
            {
                thisEvent.Invoke ();
            }
        }
    }
}