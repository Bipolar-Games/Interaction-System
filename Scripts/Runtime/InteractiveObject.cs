using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
	public sealed class InteractiveObject : MonoBehaviour
	{
		[SerializeField]
		private List<ObjectInteraction> interactions;

		private readonly Dictionary<InteractionType, IObjectInteraction> interactionsByType = new Dictionary<InteractionType, IObjectInteraction>();

		public ICollection<InteractionType> InteractionTypes => interactionsByType.Keys;

		/// <summary>
		/// Carries out the interaction of the given type. Returns the interaction that was carried out, or <c>null</c>
		/// when this object does not know the type or the interaction refused. Reached only through
		/// <see cref="Interactor"/> so that interactions are never triggered around it.
		/// </summary>
		internal IObjectInteraction Interact(InteractionType type, Interactor interactor, IInteractorBehavior behavior)
		{
			if (type == null || interactionsByType.TryGetValue(type, out var interaction) == false)
				return null;

			var context = new InteractionContext(interactor, behavior);
			return interaction.Interact(context) ? Unwrap(interaction) : null;
		}

		/// <summary>The interaction that handles the type (your own class or asset), or <c>null</c>.</summary>
		public IObjectInteraction GetInteraction(InteractionType type) =>
			type != null && interactionsByType.TryGetValue(type, out var interaction)
			? Unwrap(interaction)
			: null;

		// The list holds serialized-interface wrappers; callers want the real interaction.
		private static IObjectInteraction Unwrap(IObjectInteraction interaction) =>
			interaction is ObjectInteraction wrapper ? wrapper.Value : interaction;

		private void OnEnable()
		{
			InteractionTypeUtility.PopulateDictionary(interactions, interactionsByType, this);
		}
	}
}
