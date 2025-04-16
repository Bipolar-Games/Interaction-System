using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.Interactions
{
	public class GenericObjectInteraction : SceneObjectInteraction
	{
		[SerializeField]
		private UnityEvent interactAction;

		public override void Interact(Interactor interactor, IInteractionHandler interactorInteraction)
		{
			interactAction.Invoke();
		}
	}
}
