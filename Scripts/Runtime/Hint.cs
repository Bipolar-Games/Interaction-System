using UnityEngine;

namespace Bipolar.InteractionSystem
{
    public interface IHint
    {
        event System.Action<IHint> OnChanged;
        string Message { get; set; }
    }

    public class Hint : MonoBehaviour, IHint
    {
        public virtual event System.Action<IHint> OnChanged;

        [SerializeField]
        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnChanged?.Invoke(this);
            }
        }
    }
}
