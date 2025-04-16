using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class InteractorDescriptor : MonoBehaviour
	{
		[SerializeField]
		private List<InteractionHandler> interactionHandlers;

		private readonly Dictionary<InteractionType, IInteractionHandler> interactionHandlersByType = new Dictionary<InteractionType, IInteractionHandler>();

		private void OnEnable()
		{
			InteractionTypeUtility.PopulateDictionary(interactionHandlers, interactionHandlersByType);
		}

		public bool CanInteract(InteractionType type) => interactionHandlersByType.ContainsKey(type);

		public bool TryGetInteractionHandler(InteractionType type, out IInteractionHandler interaction) => interactionHandlersByType.TryGetValue(type, out interaction);

		//public bool AddInteraction(IInteractionHandler interaction)
		//{
		//	var interactionType = interaction.GetInteractionTypes();
		//	if (HasInteraction(interactionType))
		//		return false;

		//	interactionHandlers.Add(new InteractionHandler { Value = interaction });
		//	interactionHandlersByType.Add(interactionType, interaction);
		//	return true;
		//}

		public bool RemoveInteraction(InteractionType type)
		{
			if (CanInteract(type) == false)
				return false;

			RemoveInteraction(interactionHandlers, type);
			interactionHandlersByType.Remove(type);
			return true;
		}

		private static void RemoveInteraction<TInteraction>(List<TInteraction> interactions, InteractionType type) 
			where TInteraction : ISerializedInteraction
		{
			interactions.RemoveAll(i => i.GetInteractionTypes().Any(t => t == type));
		}

		private static void RemoveWhere<T>(List<T> list, System.Predicate<T> predicate)
		{
			int index = list.FindIndex(predicate);
			if (index >= 0)
				list.RemoveAt(index);
		}	

		public void GetAvailableInteractions(InteractiveObject interactiveObject, IList<InteractionType> results)
		{
			results.Clear();
			foreach (var type in interactiveObject.InteractionTypes)
				if (CanInteract(type))
					results.Add(type);
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				InteractionTypeUtility.PopulateDictionary(interactionHandlers, interactionHandlersByType);
			}
		}
	}
}
