﻿using System.Collections.Generic;

namespace Bipolar.Interactions
{
	public interface IInteraction
	{
		IReadOnlyList<InteractionType> GetInteractionTypes();
	}
}
