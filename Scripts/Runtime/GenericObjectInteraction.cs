using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.InteractionSystem
{
	public class GenericObjectInteraction : SceneObjectInteraction
	{
		[SerializeField]
		private UnityEvent interactAction;

		public override void Interact(Interactor interactor, IInteractorInteraction interactorInteraction)
		{
			interactAction.Invoke();
		}
	}
}
