using UnityEngine;

namespace Bipolar.InteractionSystem
{

    public abstract class InteractionTriggerAsset: ScriptableObject, IInteractionTrigger
    {
        public abstract bool Check();
    }
}
