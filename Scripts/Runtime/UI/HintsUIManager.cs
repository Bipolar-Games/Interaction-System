using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem.UI
{
	public class HintsUIManager : MonoBehaviour
	{
		[SerializeField]
		private DefaultInteractorHintsManager hintsManager;
		public DefaultInteractorHintsManager HintsManager
		{
			get => hintsManager;
			set
			{
				UnsubscribeEvents();
				hintsManager = value;
				SubscribeEvents();
			}
		}

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
			SubscribeEvents();
		}

		private void SubscribeEvents()
		{
			if (hintsManager)
				hintsManager.OnHintsUpdated += HintsManager_OnHintsUpdated;
		}

		private void HintsManager_OnHintsUpdated()
		{
			objectNameDisplay.CurrentHint = hintsManager.InteractiveObjectNameHint;
			var hints = hintsManager.InteractionsHints;
			for (int i = 0; i < hintDisplays.Count; i++)
			{
				hintDisplays[i].CurrentHint = i < hints.Count ? hints[i] : null;
			}
		}

		private void OnDisable()
		{
			UnsubscribeEvents();
		}

		private void UnsubscribeEvents()
		{
			if (hintsManager)
				hintsManager.OnHintsUpdated -= HintsManager_OnHintsUpdated;
		}
	}
}
