using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.RaycastSystem
{
    public class RaycastTarget : MonoBehaviour
    {
        public event Action OnRayEntered;
        public event Action OnRayExited;

#if UNITY_EDITOR && !UNITY_2023_1_OR_NEWER
        private void Start() { } // required for "enabled" toggle showing up in inspector 
#endif
        internal void RayEnter()
        { 
            OnRayEntered?.Invoke();
        }

        internal void RayExit()
        {
            OnRayExited?.Invoke();
        }
    }
}
