using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	[CreateAssetMenu(menuName = Paths.Root + "Simple Interaction")]
	public class SimpleInteraction : InteractionType, IObjectInteraction, IInteractorInteraction
	{
        public IEnumerable<InteractionType> GetInteractionTypes()
        {
            yield return this;
        }

        public virtual void Interact(Interactor interactor, IInteractorInteraction interactorInteraction)
		{
			Debug.Log($"Interacted with {name}");
		}
	}
}
