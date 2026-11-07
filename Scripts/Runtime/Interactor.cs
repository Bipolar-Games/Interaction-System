using UnityEngine;

namespace Bipolar.InteractionSystem
{
	public abstract class Interactor : MonoBehaviour
	{
		public abstract bool TryInteract(InteractionType interaction);

		/// <summary>
		/// The only way to carry out an interaction on an object. Call it from <see cref="TryInteract"/>.
		/// Returns the interaction that was carried out, or <c>null</c> when the object does not know the type
		/// or the interaction refused.
		/// </summary>
		protected IObjectInteraction Perform(InteractiveObject target, InteractionType type, IInteractorBehavior behavior)
		{
			return target == null ? null : target.Interact(type, this, behavior);
		}
	}
}
