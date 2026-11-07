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
			return interaction.Interact(context) ? interaction : null;
		}

		/// <summary>The interaction that handles the type (your own class or asset), or <c>null</c>.</summary>
		public IObjectInteraction GetInteraction(InteractionType type) =>
			type != null && interactionsByType.TryGetValue(type, out var interaction)
			? interaction
			: null;

		private void OnEnable()
		{
			// The serialized list holds wrappers; the dictionary holds the real interactions.
			var unwrapped = new List<IObjectInteraction>(interactions.Count);
			foreach (var wrapper in interactions)
				unwrapped.Add(wrapper == null ? null : wrapper.Value);

			InteractionTypeUtility.PopulateDictionary(unwrapped, interactionsByType, this);
		}
	}
}
