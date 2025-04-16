using UnityEngine;

namespace Bipolar.Interactions
{
	[CreateAssetMenu(menuName = Paths.Root + "Simple Interaction")]
	public class SimpleInteraction : InteractionType, IObjectInteraction, IInteractionHandler
	{
		public virtual void Interact(Interactor interactor, IInteractionHandler interactorInteraction)
		{
			Debug.Log($"Interacted with {name}");
		}
	}
}
