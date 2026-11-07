using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
	[CreateAssetMenu(menuName = Paths.Root + "Simple Interaction")]
	public class SimpleInteraction : InteractionType, IObjectInteraction, IInteractorBehavior
	{
        public IEnumerable<InteractionType> GetInteractionTypes()
        {
            yield return this;
        }

        public virtual bool Interact(InteractionContext context)
		{
			Debug.Log($"Interacted with {name}");
			return true;
		}
	}
}
