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
            foreach (var interaction in interactions)
                foreach (var type in interaction.GetInteractionTypes())
                    interactionsByType.Add(type, interaction);
        }

        public bool CanInteract(InteractionType type) => interactionsByType.ContainsKey(type);

        public bool TryGetInteraction(InteractionType type, out IInteractorInteraction interaction) => interactionsByType.TryGetValue(type, out interaction);

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
                InteractionTypeUtility.PopulateDictionary(interactions, interactionsByType);
            }
        }
    }
}
