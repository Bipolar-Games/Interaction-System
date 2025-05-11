using UnityEngine;

namespace Bipolar.RaycastSystem
{
    [RequireComponent(typeof(Collider)), DisallowMultipleComponent]
    public class RaycastCollider : MonoBehaviour
    {
        [SerializeField]
        private RaycastTarget[] raycastTargets;
        public RaycastTarget RaycastTarget
        {
            get
            {
                foreach (var target in raycastTargets)
                    if (target.isActiveAndEnabled)
                        return target;

                return default;
            }
        }

        private void Reset()
        {
            raycastTargets = GetComponentsInParent<RaycastTarget>();
        }
    }
}
