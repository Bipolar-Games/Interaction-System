using System.Collections.Generic;

namespace Bipolar.Interactions
{
	public interface IInteractorInteraction : IInteraction
	{ }

	[System.Serializable]
	public class InteractorInteraction : Serialized<IInteractorInteraction>, IInteractorInteraction
	{
		public IReadOnlyList<InteractionType> GetInteractionTypes() => Value.GetInteractionTypes();
	}
}
