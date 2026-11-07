using UnityEngine;

namespace Bipolar.InteractionSystem.Samples
{
	public class TalkInteraction : SceneObjectInteraction
	{
		public override bool Interact(InteractionContext context)
		{
			Debug.Log($"{context.Interactor.name} talks with {transform.root.name}");
			return true;
		}
	}
}
