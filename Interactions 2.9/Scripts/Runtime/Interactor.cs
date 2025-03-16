using UnityEngine;

namespace Bipolar.Interactions
{
	public abstract class Interactor : MonoBehaviour
	{
		public abstract bool TryInteract(InteractionType interaction);
	}
}
