using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
	/// <summary>
	/// The player's side of an interaction: what it can do (<see cref="behaviors"/>) and which interactive object
	/// it is facing now. A detector chooses the object; anything (input, voice, AI) calls <see cref="TryInteract"/>.
	/// </summary>
	public class DefaultInteractor : Interactor
	{
		public delegate void InteractiveObjectChangedEventHandler(DefaultInteractor sender, InteractiveObject oldObject, InteractiveObject newObject);

		public event InteractiveObjectChangedEventHandler OnInteractiveObjectChanged;

		[Header("Settings")]
		[Tooltip("What this interactor can do. Each behavior declares the interaction types it performs.")]
		[SerializeField]
		private List<InteractorBehavior> behaviors = new List<InteractorBehavior>();

		[Header("States")]
		[SerializeField]
		private InteractiveObject currentInteractiveObject;

		private readonly Dictionary<InteractionType, IInteractorBehavior> behaviorsByType = new Dictionary<InteractionType, IInteractorBehavior>();
		private readonly List<InteractionType> interactions = new List<InteractionType>();

		/// <summary>The interaction types that both the current object and this interactor know.</summary>
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

		private void OnEnable()
		{
			RefreshBehaviors();
		}

		public bool CanInteract(InteractionType type) => type != null && behaviorsByType.ContainsKey(type);

		public bool TryGetBehavior(InteractionType type, out IInteractorBehavior behavior)
		{
			behavior = null;
			return type != null && behaviorsByType.TryGetValue(type, out behavior);
		}

		public override bool TryInteract(InteractionType interaction)
		{
			if (InteractiveObject == null)
				return false;

			if (TryGetBehavior(interaction, out var behavior) == false)
				return false;

			return InteractiveObject.Interact(interaction, this, behavior) != null;
		}

		private void RefreshBehaviors()
		{
			InteractionTypeUtility.PopulateDictionary(behaviors, behaviorsByType, this);
			UpdateAvailableInteractions();
		}

		private void UpdateAvailableInteractions()
		{
			interactions.Clear();
			if (InteractiveObject == null)
				return;

			foreach (var type in InteractiveObject.InteractionTypes)
				if (CanInteract(type))
					interactions.Add(type);
		}

		private void OnValidate()
		{
			if (Application.isPlaying && isActiveAndEnabled)
				RefreshBehaviors();
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
