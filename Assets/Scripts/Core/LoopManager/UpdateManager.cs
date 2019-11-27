using System.Collections.Generic;
using UnityEngine;

namespace Kurenaiz.Management.Core
{
    public class UpdateManager : MonoBehaviour
    {
        private static UpdateManager Instance;

        List<IUpdate> update = new List<IUpdate> ();
        List<IFixedUpdate> fixedUpdate = new List<IFixedUpdate> ();
        List<IStart> start = new List<IStart> ();

        private void Awake() => Instance = this;

        void Start ()
        {
            for (int i = 0; i < start.Count; i++)
            {
                start[i].MStart ();
            }
        }

        void Update ()
        {
            for (int i = 0; i < update.Count; i++)
            {
                update[i].MUpdate ();
            }
        }

        void FixedUpdate ()
        {
            for (int i = 0; i < fixedUpdate.Count; i++)
            {
                fixedUpdate[i].MFixedUpdate ();
            }
        }

        public static void Subscribe (IStart obj) => Instance.SubscribeToStart (obj);
        public static void Subscribe(IFixedUpdate obj) => Instance.SubscribeToFixedUpdate(obj);
        public static void Subscribe(IUpdate obj) => Instance.SubscribeToUpdate(obj);

        public static void Unsubscribe(IStart obj) => Instance.UnsubscribeToStart(obj);
        public static void Unsubscribe(IFixedUpdate obj) => Instance.UnsubscribeToFixedUpdate(obj);
        public static void Unsubscribe(IUpdate obj) => Instance.UnsubscribeToUpdate(obj);

        void SubscribeToStart(IStart obj) => start.Add(obj);

        void SubscribeToFixedUpdate(IFixedUpdate obj) => fixedUpdate.Add(obj);

        void SubscribeToUpdate(IUpdate obj) => update.Add(obj);

        void UnsubscribeToStart(IStart obj) => start.Remove(obj);

        void UnsubscribeToFixedUpdate(IFixedUpdate obj) => fixedUpdate.Remove(obj);

        void UnsubscribeToUpdate(IUpdate obj) => update.Remove(obj);
    }
}