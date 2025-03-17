using UnityEngine;

namespace Bipolar.Interactions
{
	public class PlayerInteractorController : MonoBehaviour
	{
		[SerializeField]
		private DefaultInteractor interactor;

		[SerializeField]
		private InteractionToKeyMapping[] controls;

		private void Update()
		{
			for (int i = 0; i < controls.Length; i++)
			{
				if (Input.GetKeyDown(controls[i].Key))
				{
					interactor.TryInteract(controls[i].Interaction);
				}
			}
		}

		[System.Serializable]
		private struct InteractionToKeyMapping
		{
			public InteractionType Interaction;
			public KeyCode Key;
		}
    }
}
