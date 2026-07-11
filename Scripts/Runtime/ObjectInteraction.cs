using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
	public interface IObjectInteraction : IInteraction
	{
		void Interact(Interactor interactor, IInteractorBehavior behavior);
	}

	[System.Serializable]
	public class ObjectInteraction : SerializedInterface<IObjectInteraction>, IObjectInteraction
	{
        public IEnumerable<InteractionType> GetInteractionTypes() => Value.GetInteractionTypes();

        public void Interact(Interactor interactor, IInteractorBehavior behavior) => Value.Interact(interactor, behavior);
	}

    public abstract class SceneObjectInteraction : MonoBehaviour, IObjectInteraction
    {
        [SerializeField]
        protected InteractionType[] interactionTypes;
        public IEnumerable<InteractionType> GetInteractionTypes() => interactionTypes;

        public abstract void Interact(Interactor interactor, IInteractorBehavior behavior);
    }

    public abstract class ObjectInteractionAsset : ScriptableObject, IObjectInteraction
    {
        [SerializeField]
        protected List<InteractionType> interactionTypes;
        public IEnumerable<InteractionType> GetInteractionTypes() => interactionTypes;

        public abstract void Interact(Interactor interactor, IInteractorBehavior behavior);
    }
}
