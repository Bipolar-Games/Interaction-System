using UnityEngine;

namespace Bipolar.Interactions.UI
{
    public abstract class HintDisplay : MonoBehaviour
    {
        [SerializeReference]
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.ReadOnly]
#endif
        private IHint currentHint;
        public IHint CurrentHint
        {
            get => currentHint;
            set
            {
                if (currentHint != null)
                    currentHint.OnChanged -= Refresh;
                currentHint = value;
                if (currentHint != null)
                {
                    Refresh(currentHint);
                    currentHint.OnChanged += Refresh;
                }
                else
                {
                    Refresh(null);
                }
            }
        }

        private void OnEnable()
        {
            Refresh(currentHint);
            if (currentHint != null)
                currentHint.OnChanged += Refresh;
        }

        protected abstract void Refresh(IHint hint);

        private void OnDisable()
        {
            if (currentHint != null)
                currentHint.OnChanged -= Refresh;
        }
    }
}
