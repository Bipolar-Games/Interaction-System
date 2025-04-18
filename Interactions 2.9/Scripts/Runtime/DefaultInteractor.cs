using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class DefaultInteractor : Interactor
	{
		public delegate void InteractiveObjectChangedEventHandler(DefaultInteractor sender, InteractiveObject oldObject, InteractiveObject newObject);

		public event InteractiveObjectChangedEventHandler OnInteractiveObjectChanged;

		[SerializeField]
		private InteractorDescriptor descriptor;

		[Space]
		[SerializeField]
		private InteractiveObject currentInteractiveObject;

		private readonly List<InteractionType> interactions = new List<InteractionType>();
		public IReadOnlyList<InteractionType> AvailableInteractions => interactions;

		public InteractiveObject InteractiveObject
		{
			get => currentInteractiveObject;
			set
			{
				if (currentInteractiveObject == value)
					return;

				var old = currentInteractiveObject;
				currentInteractiveObject = value;
				UpdateAvailableInteractions();
				OnInteractiveObjectChanged?.Invoke(this, old, value);
			}
		}

		public override bool TryInteract(InteractionType interaction)
		{
			if (InteractiveObject == null)
				return false;

			if (descriptor.TryGetInteraction(interaction, out var interactorInteraction) == false)
				return false;

			return InteractiveObject.Interact(interaction, this, interactorInteraction) != null;
		}

		private void UpdateAvailableInteractions()
		{
			interactions.Clear();
			if (InteractiveObject == null)
				return;

			descriptor.GetAvailableInteractions(InteractiveObject, interactions);
		}

		[ContextMenu("Interact")]
		private void InteractInEditor()
		{
			InteractWithFirstInteraction();
		}

		private void InteractWithFirstInteraction(int interactionIndex = 0)
		{
			if (interactions.Count > 0 && InteractiveObject)
			{
				var interaction = interactions[interactionIndex];
				InteractiveObject.Interact(interaction, this, null);
			}
		}
	}
}
