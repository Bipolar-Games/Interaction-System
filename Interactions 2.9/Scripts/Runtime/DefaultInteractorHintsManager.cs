using Bipolar.InteractionSystem;
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
		public event System.Action<IHint> OnNameHintUpdated;

		[SerializeField]
		private DefaultInteractor interactor;

		[Header("States")]
		[SerializeReference]
		private IHint interactiveObjectNameHint;

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
			interactiveObjectNameHint = newObject 
				? new BasicHint { Message = newObject.name }
				: null;

			OnNameHintUpdated?.Invoke(interactiveObjectNameHint);
		}

		private void OnDisable()
		{
			interactor.OnInteractiveObjectChanged -= Interactor_OnInteractiveObjectChanged;
		}
	}
}
