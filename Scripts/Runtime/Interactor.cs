using UnityEngine;

namespace Bipolar.InteractionSystem
{
	public abstract class Interactor : MonoBehaviour
	{
		public abstract bool TryInteract(InteractionType interaction);
	}
}
