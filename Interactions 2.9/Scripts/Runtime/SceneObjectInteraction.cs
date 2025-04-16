using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public abstract class SceneObjectInteraction : MonoBehaviour, IObjectInteraction
	{
		[SerializeField]
		private InteractionType[] types;

		public IReadOnlyList<InteractionType> GetInteractionTypes() => types;

		public abstract void Interact(Interactor interactor, IInteractionHandler interactorInteraction);
	}
}
