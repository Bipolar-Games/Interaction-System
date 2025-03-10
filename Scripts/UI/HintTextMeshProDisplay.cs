﻿using UnityEngine;

namespace Bipolar.InteractionSystem
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class HintTextMeshProDisplay : HintDisplay
    {
        private TMPro.TMP_Text _label;
        public TMPro.TMP_Text Label
        {
            get
            {
                if (_label == null)
                    TryGetComponent(out _label);
                return _label;
            }
        }

        protected override void Refresh(Hint hint)
        {
            string hintMessage = hint ? hint.Message : string.Empty;
            Label.enabled = !string.IsNullOrEmpty(hintMessage);
            Label.text = hintMessage;
        }
    }
}
