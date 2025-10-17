using System.Collections.Generic;

namespace Bipolar.InteractionSystem
{
	public interface IInteractorInteraction : IInteraction
	{ }

    [System.Serializable]
    public class InteractorInteraction : Serialized<IInteractorInteraction>, IInteractorInteraction
    {
        public IEnumerable<InteractionType> GetInteractionTypes() => Value.GetInteractionTypes();
    }
}
