using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public interface IObjectInteraction : IInteraction
	{
		void Interact(Interactor interactor, IInteractionHandler interactorInteraction);
	}


	[System.Serializable]
	public class ObjectInteraction : Serialized<IObjectInteraction>, IObjectInteraction
	{
		[SerializeField]
		private List<InteractionType> interactionTypes;

		public void Interact(Interactor interactor, IInteractionHandler interactorInteraction) => Value.Interact(interactor, interactorInteraction);
	}
}
