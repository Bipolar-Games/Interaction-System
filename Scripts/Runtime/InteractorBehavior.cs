using System.Collections.Generic;

namespace Bipolar.InteractionSystem
{
	public interface IInteractorBehavior : IInteraction
	{ }

    [System.Serializable]
    public class InteractorBehavior : SerializedInterface<IInteractorBehavior>, IInteractorBehavior
    {
        public IEnumerable<InteractionType> GetInteractionTypes() => Value == null ? System.Array.Empty<InteractionType>() : Value.GetInteractionTypes();
        public override string ToString() => Value.ToString();
    }
}
