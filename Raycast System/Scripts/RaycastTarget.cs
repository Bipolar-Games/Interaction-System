using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.RaycastSystem
{
    public class RaycastTarget : MonoBehaviour
    {
        public event Action OnRayEntered;
        public event Action OnRayExited;

        [SerializeField]
        private UnityEvent onRayEnter;
        [SerializeField]
        private UnityEvent onRayExit;

#if UNITY_EDITOR && !UNITY_6000_0_OR_NEWER
        private void Start() { } // required for "enabled" toggle showing up in inspector 
#endif
        internal void RayEnter()
        { 
            onRayEnter.Invoke();
            OnRayEntered?.Invoke();
        }

        internal void RayExit()
        {
            onRayExit.Invoke();
            OnRayExited?.Invoke();
        }
    }
}
