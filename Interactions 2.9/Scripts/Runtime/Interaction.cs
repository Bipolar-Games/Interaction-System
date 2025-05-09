﻿using System.Collections.Generic;

namespace Bipolar.Interactions
{
	public interface IInteraction
    {
        IEnumerable<InteractionType> GetInteractionTypes();
    }
}