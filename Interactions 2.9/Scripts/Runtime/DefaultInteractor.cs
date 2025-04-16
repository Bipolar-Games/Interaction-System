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

		[SerializeField]
		private InteractiveObject interactiveObject;

		private readonly List<InteractionType> interactions = new List<InteractionType>();
		public IReadOnlyList<InteractionType> AvailableInteractions => interactions;

		public InteractiveObject InteractiveObject
		{
			get => interactiveObject;
			set
			{
				if (interactiveObject == value)
					return;

				var old = interactiveObject;
				interactiveObject = value;
				UpdateAvailableInteractions();
				OnInteractiveObjectChanged?.Invoke(this, old, value);
			}
		}

		public override bool TryInteract(InteractionType interaction)
		{
			if (InteractiveObject == null)
				return false;

			if (descriptor.TryGetInteractionHandler(interaction, out var interactorInteraction) == false)
				return false;

			return InteractiveObject.Interact(interaction, this, interactorInteraction) != null;
		}

		private void Update()
		{
			UpdateAvailableInteractions();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Interact();
			}
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
			Interact();
		}

		private void Interact(int interactionIndex = 0)
		{
			if (interactions.Count > 0 && InteractiveObject)
			{
				var interaction = interactions[interactionIndex];
				InteractiveObject.Interact(interaction, this, null);
			}
		}
	}
}
