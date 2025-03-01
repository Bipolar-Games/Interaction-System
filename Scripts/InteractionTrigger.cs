using UnityEngine;

namespace Bipolar.InteractionSystem
{
    public interface IInteractionTrigger
    {
        bool Check();
    }

    [System.Serializable]
    public class InteractionTrigger : Serialized<IInteractionTrigger>, IInteractionTrigger
    {
        public bool Check() => Value.Check();
    }
 
    public abstract class GlobalInteractionTrigger : ScriptableObject, IInteractionTrigger
    {
        public abstract bool Check();
    }

    public abstract class SceneInteractionTrigger : MonoBehaviour, IInteractionTrigger
    {
        public abstract bool Check();
    }
}
