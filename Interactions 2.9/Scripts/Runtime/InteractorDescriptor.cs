using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class InteractorDescriptor : MonoBehaviour
	{
		[SerializeField]
		private List<InteractorInteraction> interactions;

		private readonly Dictionary<InteractionType, IInteractorInteraction> interactionsByType = new Dictionary<InteractionType, IInteractorInteraction>();

		private void OnEnable()
		{
			InteractionTypeUtility.PopulateDictionary(interactions, interactionsByType);
		}

		public bool HasInteraction(InteractionType type) => interactionsByType.ContainsKey(type);

		public bool TryGetInteraction(InteractionType type, out IInteractorInteraction interaction) => interactionsByType.TryGetValue(type, out interaction);

		//public bool AddInteraction(IInteractorInteraction interaction)
		//{
		//	var interactionType = interaction.GetInteractionTypes();
		//	if (HasInteraction(interactionType))
		//		return false;

		//	interactions.Add(new InteractorInteraction { Value = interaction });
		//	interactionsByType.Add(interactionType, interaction);
		//	return true;
		//}

		public bool RemoveInteraction(InteractionType type)
		{
			if (HasInteraction(type) == false)
				return false;

			RemoveInteraction(interactions, type);
			interactionsByType.Remove(type);
			return true;
		}

		private static void RemoveInteraction<TInteraction>(List<TInteraction> interactions, InteractionType type) 
			where TInteraction : IInteractorInteraction
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
				if (HasInteraction(type))
					results.Add(type);
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				InteractionTypeUtility.PopulateDictionary(interactions, interactionsByType);
			}
		}
	}
}
