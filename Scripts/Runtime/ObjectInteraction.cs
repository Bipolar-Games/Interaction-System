using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
	public interface IObjectInteraction : IInteraction
	{
		void Interact(Interactor interactor, IInteractorInteraction interactorInteraction);
	}

	[System.Serializable]
	public class ObjectInteraction : Serialized<IObjectInteraction>, IObjectInteraction
	{
        public IEnumerable<InteractionType> GetInteractionTypes() => Value.GetInteractionTypes();

        public void Interact(Interactor interactor, IInteractorInteraction interactorInteraction) => Value.Interact(interactor, interactorInteraction);
	}

    public abstract class SceneObjectInteraction : MonoBehaviour, IObjectInteraction
    {
        [SerializeField]
        protected InteractionType[] interactionTypes;
        public IEnumerable<InteractionType> GetInteractionTypes() => interactionTypes;

        public abstract void Interact(Interactor interactor, IInteractorInteraction interactorInteraction);
    }

    public abstract class ObjectInteractionAsset : ScriptableObject, IObjectInteraction
    {
        [SerializeField]
        protected List<InteractionType> interactionTypes;
        public IEnumerable<InteractionType> GetInteractionTypes() => interactionTypes;

        public abstract void Interact(Interactor interactor, IInteractorInteraction interactorInteraction);
    }
}
