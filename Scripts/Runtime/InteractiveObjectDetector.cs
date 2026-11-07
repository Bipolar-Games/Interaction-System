using UnityEngine;

namespace Bipolar.InteractionSystem
{
    /// <summary>
    /// The contract of a detector: every frame it says which interactive object the interactor faces now,
    /// or <c>null</c> when there is none. The base class writes that answer to the interactor, so a detector
    /// never has to remember to clear it. Derive from it to detect in any way (overlap, ray, trigger, a list...).
    /// </summary>
    public abstract class InteractiveObjectDetector : MonoBehaviour
    {
        [SerializeField]
        private DefaultInteractor interactor;
        public DefaultInteractor Interactor => interactor;

        protected virtual void Reset()
        {
            interactor = GetComponentInParent<DefaultInteractor>();
            if (interactor == null)
                interactor = GetComponentInChildren<DefaultInteractor>();
        }

        protected virtual void OnEnable()
        {
            if (interactor == null)
            {
                Debug.LogError("The detector needs an interactor.", this);
                enabled = false;
            }
        }

        private void Update()
        {
            interactor.InteractiveObject = Detect();
        }

        protected virtual void OnDisable()
        {
            // A detector that is off cannot tell what is in front of the player.
            if (interactor)
                interactor.InteractiveObject = null;
        }

        /// <summary>The interactive object to interact with right now, or <c>null</c> when there is none.</summary>
        protected abstract InteractiveObject Detect();

        /// <summary>The interactive object a collider belongs to (it may sit on a parent), or <c>null</c>.</summary>
        protected static InteractiveObject GetInteractiveObject(Collider collider)
        {
            return collider ? collider.GetComponentInParent<InteractiveObject>() : null;
        }
    }
}
