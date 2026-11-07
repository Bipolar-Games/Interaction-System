using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
    [CreateAssetMenu(menuName = Paths.Root + "Interaction Type")]
    public class InteractionType : ScriptableObject
    { }

    public static class InteractionTypeUtility
    {
        /// <summary>
        /// Maps every interaction type to the interaction that handles it. Empty slots, missing types and types
        /// already handled by an earlier interaction are skipped (with a warning) and the list is left untouched.
        /// </summary>
        internal static void PopulateDictionary<TListInteraction, TInteraction>(IList<TListInteraction> interactions, IDictionary<InteractionType, TInteraction> map, Object context = null)
            where TListInteraction : TInteraction
            where TInteraction : IInteraction
        {
            map.Clear();
            for (int i = 0; i < interactions.Count; i++)
            {
                var interaction = interactions[i];
                if (interaction == null)
                {
                    Debug.LogWarning($"Interaction {i} is empty.", context);
                    continue;
                }

                foreach (var type in interaction.GetInteractionTypes())
                {
                    if (type == null)
                    {
                        Debug.LogWarning($"Interaction {i} has an empty interaction type.", context);
                        continue;
                    }

                    if (map.ContainsKey(type))
                    {
                        Debug.LogWarning($"Interaction type '{type.name}' is handled twice; the first one wins.", context);
                        continue;
                    }

                    map.Add(type, interaction);
                }
            }
        }
    }
}
