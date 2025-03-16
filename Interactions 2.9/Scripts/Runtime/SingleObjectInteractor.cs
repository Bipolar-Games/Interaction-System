﻿using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class SingleObjectInteractor : Interactor
	{
		public delegate void InteractiveObjectChangedEventHandler (SingleObjectInteractor sender, InteractiveObject oldObject, InteractiveObject newObject);

		public event InteractiveObjectChangedEventHandler OnInteractiveObjectChanged;

		[SerializeField]
		private InteractorDescriptor descriptor;

		[SerializeField]
		private InteractiveObject interactiveObject;

		private readonly List<InteractionType> interactions = new List<InteractionType>();

		public InteractiveObject InteractiveObject
		{
			get => interactiveObject;
			set
			{
				if (interactiveObject == value)
					return;

				var old = interactiveObject;
				interactiveObject = value;
				OnInteractiveObjectChanged?.Invoke(this, old, value);
			}
		}

		public override bool TryInteract(InteractionType interaction)
		{
			if (InteractiveObject == null)
				return false;

			if (descriptor.TryGetInteraction(interaction, out var interactorInteraction) == false)
				return false;

			InteractiveObject.Interact(interaction, this, interactorInteraction);
			return true;
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
