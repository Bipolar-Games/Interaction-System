using UnityEngine;

namespace Bipolar.InteractionSystem
{
    public interface IHint
    {
        event System.Action<IHint> OnHintChanged;
        string Message { get; set; }
    }

    public class Hint : MonoBehaviour, IHint
    {
        public virtual event System.Action<IHint> OnHintChanged;

        [SerializeField]
        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnHintChanged?.Invoke(this);
            }
        }
    }
}
