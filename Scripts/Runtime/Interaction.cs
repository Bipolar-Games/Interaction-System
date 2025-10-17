using System.Collections.Generic;

namespace Bipolar.InteractionSystem
{
	public interface IInteraction
    {
        IEnumerable<InteractionType> GetInteractionTypes();
    }
}