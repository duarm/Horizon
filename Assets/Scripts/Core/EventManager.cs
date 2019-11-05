using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kurenaiz.Management.Events
{
    public class EventManager : MonoBehaviour
    {

        private Dictionary<string, Action> eventDictionary;

        public static Action<PlanetData> OnFocus { get; private set; }
        public static Action<bool> OnShowOrbits { get; private set; }
        public static Action<OrbitType> SetOrbitType { get; private set; }

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

        public static void SubscribeToFocus (Action<PlanetData> action) => OnFocus += action;
        public static void SubscribeToShowOrbits (Action<bool> action) => OnShowOrbits += action;
        public static void SubscribeToSetOrbitType (Action<OrbitType> action) => SetOrbitType += action;

        public static void StartListening (string eventName, Action listener)
        {
            if (instance.eventDictionary.TryGetValue (eventName, out Action thisEvent))
            {
                thisEvent += listener;
                instance.eventDictionary[eventName] = thisEvent;
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
            if (instance.eventDictionary.TryGetValue (eventName, out Action thisEvent))
            {
                thisEvent -= listener;
                instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent (string eventName)
        {
            if (instance.eventDictionary.TryGetValue (eventName, out Action thisEvent))
            {
                thisEvent.Invoke ();
            }
        }
    }
}