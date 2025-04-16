using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	internal interface ISerializedInteraction : IInteraction
	{
		IReadOnlyList<InteractionType> GetInteractionTypes();
	}

	public class SceneInteraction : MonoBehaviour, ISerializedInteraction
	{
		[SerializeField]
		protected List<InteractionType> interactionTypes;
		public IReadOnlyList<InteractionType> GetInteractionTypes() => interactionTypes;
	}
	
	public class InteractionAsset : ScriptableObject, ISerializedInteraction
	{
		[SerializeField]
		protected List<InteractionType> interactionTypes;
		public IReadOnlyList<InteractionType> GetInteractionTypes() => interactionTypes;
	}

}
