using UnityEngine;
using UnityEngine.EventSystems;

namespace Bipolar.InteractionSystem.Triggers
{
    [CreateAssetMenu(menuName = "Bipolar/Interaction System/Triggers/Mouse Input Interaction Trigger")]
    public class MouseInputInteractionTrigger : GlobalInteractionTrigger
    {
        [SerializeField]
        private PointerEventData.InputButton mouseButton;

        public override bool Check()
        {
            return UnityEngine.Input.GetMouseButtonUp((int)mouseButton);
        }
    }
}
