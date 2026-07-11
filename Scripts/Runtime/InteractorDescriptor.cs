using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
    public class InteractorDescriptor : MonoBehaviour
    {
        [SerializeField]
        private List<InteractorBehavior> behaviors;

        private readonly Dictionary<InteractionType, IInteractorBehavior> behaviorsByType = new Dictionary<InteractionType, IInteractorBehavior>();

        private void OnEnable()
        {
            behaviorsByType.Clear();
            foreach (var beh in behaviors)
                foreach (var type in beh.GetInteractionTypes())
                    behaviorsByType.Add(type, beh);
        }

        public bool CanInteract(InteractionType type) => behaviorsByType.ContainsKey(type);

        public bool TryGetBehavior(InteractionType type, out IInteractorBehavior behavior) => behaviorsByType.TryGetValue(type, out behavior);

        public void GetAvailableBehaviors(InteractiveObject interactiveObject, IList<InteractionType> results)
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
                InteractionTypeUtility.PopulateDictionary(behaviors, behaviorsByType);
            }
        }

        [ContextMenu("Gather Interactor Behaviors")]
        private void GatherBehaviors()
        {
            var childrenBehaviors = GetComponentsInChildren<IInteractorBehavior>();
            for (int i = 0; i < childrenBehaviors.Length; i++) 
                if (behaviors.Contains(behaviors[i]) == false)
                    behaviors.Add(behaviors[i]);
        }
    }
}
