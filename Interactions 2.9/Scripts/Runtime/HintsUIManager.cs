using Bipolar.InteractionSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class HintsUIManager : MonoBehaviour
	{
		[SerializeField]
		private DefaultInteractorHintsManager hintsManager;

		[SerializeField]
		private HintDisplay objectNameDisplay;
		public HintDisplay ObjectNameDisplay
		{
			get => objectNameDisplay;
			set => objectNameDisplay = value;
		}

		[SerializeField]
		private List<HintDisplay> hintDisplays;
		public List<HintDisplay> HintDisplays => hintDisplays;

		private void OnEnable()
		{
			if (hintsManager)
				hintsManager.OnNameHintUpdated += HintsManager_OnNameHintUpdated;
		}

		private void HintsManager_OnNameHintUpdated(IHint hint)
		{
			objectNameDisplay.CurrentHint = hint;
		}

		private void OnDisable()
		{
			if (hintsManager)
				hintsManager.OnNameHintUpdated -= HintsManager_OnNameHintUpdated;
		}
	}
}
