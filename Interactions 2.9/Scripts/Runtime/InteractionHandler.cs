using System.Collections.Generic;

namespace Bipolar.Interactions
{
	public interface IInteractionHandler : IInteraction
	{ }

	internal interface ISerializedInteractionHandler : IInteractionHandler, ISerializedInteraction
	{ }

	[System.Serializable]
	public class InteractionHandler : Serialized<IInteractionHandler>, IInteractionHandler
	{ }
}
