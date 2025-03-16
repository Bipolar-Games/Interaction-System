using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
    [DisallowMultipleComponent]
    public abstract class Interaction : MonoBehaviour
    {
        [Space, SerializeField]
        private InteractionTrigger trigger;
        
        [Space, SerializeField]
        private List<InteractionTrigger> additionalTriggers;

        [SerializeField]
        private List<Object> normalObjects;

        public bool CheckTrigger() => trigger != null && trigger.Check();
        public abstract void Interact(Interactor interactor);
        public virtual bool CanInteract(in Interactor interactor) => true;
        protected virtual void Start() { }

        public static bool CanInteract(Interaction interaction, Interactor interactor) =>
            interaction != null 
            && interaction.isActiveAndEnabled 
            && interaction.CanInteract(interactor);
    }
}
