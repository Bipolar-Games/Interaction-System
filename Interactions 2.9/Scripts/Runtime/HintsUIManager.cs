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
				hintsManager.OnHintsUpdated += HintsManager_OnHintsUpdated;
		}

		private void HintsManager_OnHintsUpdated()
		{
			objectNameDisplay.CurrentHint = hintsManager.InteractiveObjectNameHint;
			int interactionHintsCount = Mathf.Min(hintDisplays.Count, hintsManager.InteractionsHints.Count);
			for (int i = 0; i < interactionHintsCount; i++)
			{
				hintDisplays[i].CurrentHint = hintsManager.InteractionsHints[i];
			}
		}

		private void OnDisable()
		{
			if (hintsManager)
				hintsManager.OnHintsUpdated -= HintsManager_OnHintsUpdated;
		}
	}
}
