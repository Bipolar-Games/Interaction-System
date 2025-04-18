using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
    [CreateAssetMenu(menuName = Paths.Root + "Interaction Type")]
    public class InteractionType : ScriptableObject
    { }

    public static class InteractionTypeUtility
    {
        internal static void PopulateDictionary<TListInteraction, TInteraction>(IList<TListInteraction> interactions, IDictionary<InteractionType, TInteraction> map)
            where TListInteraction : TInteraction
            where TInteraction : IInteraction
        {
            map.Clear();
            for (int i = 0; i < interactions.Count; i++)
            {
                var interaction = interactions[i];
                if (interaction == null)
                {
                    interactions.RemoveAt(i);
                    continue;
                }

                foreach (var type in interaction.GetInteractionTypes())
                {
                    if (type == null || map.ContainsKey(type))
                    {
                        interactions.RemoveAt(i);
                        continue;
                    }
                    map.Add(type, interaction);
                }
            }
        }
    }
}




