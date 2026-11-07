using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.InteractionSystem.Samples
{
    /// <summary>
    /// Sample: calls <see cref="Interactor.TryInteract"/> when an input action is performed.
    /// The module does not depend on any input; copy this and adapt it to your own.
    /// </summary>
    public class InteractionInput : MonoBehaviour
    {
        [SerializeField]
        private Interactor interactor;

        [SerializeField]
        private InteractionBinding[] bindings;

        private void Reset()
        {
            interactor = GetComponentInParent<Interactor>();
        }

        private void OnEnable()
        {
            if (interactor == null)
            {
                Debug.LogError("The interactor must be assigned.", this);
                enabled = false;
                return;
            }

            foreach (var binding in bindings)
            {
                if (binding.Action == null)
                    continue;

                binding.Action.action.performed += OnActionPerformed;
                binding.Action.action.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (var binding in bindings)
                if (binding.Action)
                    binding.Action.action.performed -= OnActionPerformed;
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            foreach (var binding in bindings)
                if (binding.Action && binding.Action.action == context.action)
                    interactor.TryInteract(binding.Interaction);
        }

        [System.Serializable]
        private struct InteractionBinding
        {
            public InteractionType Interaction;
            public InputActionReference Action;
        }
    }
}
