using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
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

		public IObjectInteraction GetInteraction(InteractionType type) => 
			interactionsByType.TryGetValue(type, out var interaction) 
			? interaction 
			: null;

		private void OnEnable()
		{
			InteractionTypeUtility.PopulateDictionary(interactions, interactionsByType);
		}
	}
}
