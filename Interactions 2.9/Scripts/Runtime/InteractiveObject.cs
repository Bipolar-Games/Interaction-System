using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class InteractiveObject : MonoBehaviour
	{
		[SerializeField]
		private List<ObjectInteraction> interactions;

		private readonly Dictionary<InteractionType, IObjectInteraction> interactionsByType = new Dictionary<InteractionType, IObjectInteraction>();

		public ICollection<InteractionType> InteractionTypes => interactionsByType.Keys;

		public IObjectInteraction Interact(InteractionType type, Interactor interactor, IInteractorInteraction interactorInteraction)
		{
			var interaction = GetInteraction(type);
			if (interaction == null)
				return null;

			interaction.Interact(interactor, interactorInteraction); 
			return interaction;
		}

		public IObjectInteraction GetInteraction(InteractionType type)
		{
			if (interactionsByType.TryGetValue(type, out var interaction) == false)
				return null;

			return interaction;
		}

		private void OnEnable()
		{
			InteractionTypeUtility.PopulateDictionary(interactions, interactionsByType);
		}
	}
}
