using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.InteractionSystem
{
	public class GenericObjectInteraction : SceneObjectInteraction
	{
		[SerializeField]
		private UnityEvent interactAction;

		public override bool Interact(InteractionContext context)
		{
			interactAction.Invoke();
			return true;
		}
	}
}
