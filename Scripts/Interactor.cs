using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
    public delegate void InteractiveObjectChangeEventHandler(InteractiveObject oldObject, InteractiveObject newObject);
    public delegate void InteractionEventHandler(InteractiveObject interactiveObject, Interaction interaction);

    public class Interactor : MonoBehaviour
    {
        public event InteractiveObjectChangeEventHandler OnInteractiveObjectChanged;
        public event InteractionEventHandler OnInteracted;

        [SerializeField]
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.ReadOnly]
#endif
        private InteractiveObject currentInteractiveObject;
        public InteractiveObject CurrentInteractiveObject
        {
            get => currentInteractiveObject;
            set
            {
                if (currentInteractiveObject == value)
                    return;

                var oldInteractiveObject = currentInteractiveObject;
                currentInteractiveObject = value;
                OnInteractiveObjectChanged?.Invoke(oldInteractiveObject, value);
            }
        }

        public void CheckInteractions()
        {
            if (CurrentInteractiveObject && CurrentInteractiveObject.isActiveAndEnabled)
                if (CurrentInteractiveObject.TryInteract(this, out var interaction))
                    OnInteracted?.Invoke(CurrentInteractiveObject, interaction);
        }
    }
}
