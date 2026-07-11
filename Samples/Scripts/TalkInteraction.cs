using UnityEngine;

namespace Bipolar.InteractionSystem.Samples
{
	public class TalkInteraction : SceneObjectInteraction
	{
		public override void Interact(Interactor interactor, IInteractorBehavior behavior)
		{
			Debug.Log($"{interactor.name} talks with {transform.root.name}");
		}
	}
}
