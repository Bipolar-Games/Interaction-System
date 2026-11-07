using TMPro;
using UnityEngine;

namespace Bipolar.InteractionSystem.UI
{
    /// <summary>Shows the message of a hint in a TextMeshPro label.</summary>
    public class TMPHintDisplay : HintDisplay
    {
        [SerializeField]
        private TMP_Text label;

        private void Reset()
        {
            label = GetComponent<TMP_Text>();
        }

        protected override void Refresh(IHint hint)
        {
            if (label)
                label.text = hint == null ? string.Empty : hint.Message;
        }
    }
}
