using UnityEngine;

namespace Bipolar.RaycastSystem
{
    public abstract class RayProvider : MonoBehaviour
    {
        public abstract Ray GetRay();
    }

    public class RaycastController : MonoBehaviour
    {
        public delegate void RaycastTargetChangeEventHandler(RaycastTarget target);

        public event RaycastTargetChangeEventHandler OnRayEntered;
        public event RaycastTargetChangeEventHandler OnRayExited;

        [Header("Settings")]
        [SerializeField]
        private RayProvider rayProvider;
        public RayProvider RayProvider
        {
            get => rayProvider;
            set
            {
                rayProvider = value;
            }
        }

        [SerializeField]
        private LayerMask raycastedLayers;
        public LayerMask RaycastedLayers 
        { 
            get => raycastedLayers; 
            set => raycastedLayers = value; 
        }
        
        [SerializeField]
        private float raycastDistance = 3;
        public float RaycastDistance
        {
            get => raycastDistance;
            set => raycastDistance = value;
        }

        [SerializeField]
        private QueryTriggerInteraction triggerDetection = QueryTriggerInteraction.UseGlobal;
        public QueryTriggerInteraction TriggerDetection
        {
            get => triggerDetection;
            set => triggerDetection = value;
        }

        [Header("States")]     
        [SerializeField]
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.ReadOnly]
#endif
        private RaycastTarget currentTarget;
        public RaycastTarget Target => enabled ? currentTarget : null;

        private Ray ray;

        private void Update()
        {
            DoRaycast();
        }

        private void DoRaycast()
        {
            if (TryGetRaycastTarget(out var target) == false)
            {
                ExitCurrentTarget();
            }
            else if (target != currentTarget)
            {
                CallExitEvents(currentTarget);
                currentTarget = target;
                CallEnterEvents(currentTarget);
            }
        }

        private void ExitCurrentTarget()
        {
            if (currentTarget != null)
            {
                var exitedTarget = currentTarget;
                currentTarget = null;
                CallExitEvents(exitedTarget);
            }
        }

        public bool TryGetRaycastTarget(out RaycastTarget target)
        {
            RayProvider provider = rayProvider;
            ray = provider.GetRay();
            return TryGetRaycastTarget(ray, out target);
        }

        private bool TryGetRaycastTarget(Ray ray, out RaycastTarget target)
        {
            target = null;
            if (Physics.Raycast(ray, out var hit, raycastDistance, raycastedLayers, triggerDetection) == false)
                return false;

            if (hit.collider.TryGetComponent<RaycastCollider>(out var raycastCollider) == false)
                return false;

            return TryGetRaycastTarget(raycastCollider, out target);
        }

        private static bool TryGetRaycastTarget(RaycastCollider collider, out RaycastTarget target)
        {
            target = collider.RaycastTarget;
            return target != null;
        }

        private void CallEnterEvents(RaycastTarget target)
        {
            if (target != null)
            {
                OnRayEntered?.Invoke(target);
                target.RayEnter();
            }
        }

        private void CallExitEvents(RaycastTarget target)
        {
            if (target != null)
            {
                OnRayExited?.Invoke(target);
                target.RayExit();
            }
        }

        private void OnDisable()
        {
            ExitCurrentTarget();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(ray.origin, ray.direction * raycastDistance);
        }
    }
}
