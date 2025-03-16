using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	[CreateAssetMenu(menuName = Paths.Root + "Simple Interaction")]
	public class SimpleInteraction : InteractionType, IObjectInteraction, IInteractorInteraction
	{
		private InteractionType[] cachedType;

		public IReadOnlyList<InteractionType> GetInteractionTypes()
		{
			if (cachedType == null || cachedType.Length == 0) 
				cachedType = new InteractionType[] { this };
			return cachedType;
		}

		public virtual void Interact(Interactor interactor, IInteractorInteraction interactorInteraction)
		{
			Debug.Log($"Interacted with {name}");
		}
	}
}




