using UnityEngine;

namespace Bipolar.Interactions.Samples
{
	public class TalkInteraction : SceneObjectInteraction
	{
		public override void Interact(Interactor interactor, IInteractionHandler interaction)
		{
			Debug.Log($"{interactor.name} talks with {transform.root.name}");
		}
	}
}
