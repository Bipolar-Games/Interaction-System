using System.Collections.Generic;

namespace Bipolar.InteractionSystem
{
	public interface IInteractorInteraction : IInteraction
	{ }

    [System.Serializable]
    public class InteractorInteraction : SerializedInterface<IInteractorInteraction>, IInteractorInteraction
    {
        public IEnumerable<InteractionType> GetInteractionTypes() => Value.GetInteractionTypes();
        public override string ToString() => Value.ToString();
    }
}
