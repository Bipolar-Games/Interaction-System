using Bipolar.InteractionSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	[System.Serializable]
	public struct BasicHint : IHint
	{
		public event System.Action<IHint> OnHintChanged;

		[field: SerializeField]
		public string Message { get; set; }
	}

	public class DefaultInteractorHintsManager : MonoBehaviour
	{
		public event System.Action OnHintsUpdated;

		[SerializeField]
		private DefaultInteractor interactor;

		[Header("States")]
		[SerializeReference]
		private IHint interactiveObjectNameHint;
		public IHint InteractiveObjectNameHint => interactiveObjectNameHint;

		[SerializeReference]
		private List<IHint> interactionsHints;
		public IReadOnlyList<IHint> InteractionsHints => interactionsHints;

		private void Reset()
		{
			interactor = GetComponent<DefaultInteractor>();
		}

		private void OnEnable()
		{
			interactor.OnInteractiveObjectChanged += Interactor_OnInteractiveObjectChanged;
		}

		private void Interactor_OnInteractiveObjectChanged(DefaultInteractor sender, InteractiveObject oldObject, InteractiveObject newObject)
		{
			interactiveObjectNameHint = GetNameHint(newObject);

			interactionsHints.Clear();
			foreach (var interaction in sender.AvailableInteractions)
			{
				var hint = GetInteractionHint(interaction);
				interactionsHints.Add(hint);
			}
			OnHintsUpdated?.Invoke();
		}

		private IHint GetNameHint(InteractiveObject newObject)
		{
			if (newObject == null)
				return null;
				
			return new BasicHint 
			{ 
				Message = newObject.name 
			};
		}

		private IHint GetInteractionHint(InteractionType interactionType)
		{
			if (interactionType == null)
				return null;

			return new BasicHint
			{
				Message = interactionType.name
			};
		}

		private void OnDisable()
		{
			interactor.OnInteractiveObjectChanged -= Interactor_OnInteractiveObjectChanged;
		}
	}
}
