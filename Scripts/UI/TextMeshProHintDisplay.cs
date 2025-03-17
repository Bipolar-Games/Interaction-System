using UnityEngine;

namespace Bipolar.InteractionSystem
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class TextMeshProHintDisplay : HintDisplay
    {
        private TMPro.TMP_Text label;
		
        private void Awake()
		{
			label = GetComponent<TMPro.TMP_Text>();
		}

		protected override void Refresh(IHint hint)
        {
            string hintMessage = hint != default ? hint.Message : string.Empty;
            label.enabled = !string.IsNullOrEmpty(hintMessage);
            label.text = hintMessage;
        }
    }
}
