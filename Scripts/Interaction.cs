using UnityEngine;

namespace Bipolar.InteractionSystem
{
    [DisallowMultipleComponent]
    public abstract class Interaction : MonoBehaviour
    {
        public bool CheckTrigger() => false;
        public abstract void Interact(Interactor interactor);
        public virtual bool CanInteract(in Interactor interactor) => true;
        protected virtual void Start() { }

        public static bool CanInteract(Interaction interaction, Interactor interactor) =>
            interaction != null 
            && interaction.isActiveAndEnabled 
            && interaction.CanInteract(interactor);
    }
}
