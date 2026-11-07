using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.InteractionSystem
{
	public interface IObjectInteraction : IInteraction
	{
		/// <summary>Carries the interaction out. Returns <c>false</c> when the object refuses (locked, busy...).</summary>
		bool Interact(InteractionContext context);
	}

	[System.Serializable]
	public sealed class ObjectInteraction : SerializedInterface<IObjectInteraction>, IObjectInteraction
	{
        public IEnumerable<InteractionType> GetInteractionTypes() => Value == null ? System.Array.Empty<InteractionType>() : Value.GetInteractionTypes();

        public bool Interact(InteractionContext context) => Value != null && Value.Interact(context);
	}

    public abstract class SceneObjectInteraction : MonoBehaviour, IObjectInteraction
    {
        [SerializeField]
        protected InteractionType[] interactionTypes;
        public IEnumerable<InteractionType> GetInteractionTypes() => interactionTypes;

        public abstract bool Interact(InteractionContext context);
    }

    public abstract class ObjectInteractionAsset : ScriptableObject, IObjectInteraction
    {
        [SerializeField]
        protected List<InteractionType> interactionTypes;
        public IEnumerable<InteractionType> GetInteractionTypes() => interactionTypes;

        public abstract bool Interact(InteractionContext context);
    }
}
