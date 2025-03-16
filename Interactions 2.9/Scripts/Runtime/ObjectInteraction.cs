using System.Collections.Generic;

namespace Bipolar.Interactions
{
	public interface IObjectInteraction : IInteraction
	{
		void Interact(Interactor interactor, IInteractorInteraction interactorInteraction);
	}

	[System.Serializable]
	public class ObjectInteraction : Serialized<IObjectInteraction>, IObjectInteraction
	{
		public IReadOnlyList<InteractionType> GetInteractionTypes() => Value.GetInteractionTypes();

		public void Interact(Interactor interactor, IInteractorInteraction interactorInteraction) => Value.Interact(interactor, interactorInteraction);
	}
}
