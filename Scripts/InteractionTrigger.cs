namespace Bipolar.InteractionSystem
{
    public interface IInteractionTrigger
    {
        bool Check();
    }

    public class InteractionTrigger : Bipolar.Serialized<IInteractionTrigger>, IInteractionTrigger
    {
        public bool Check() => Value.Check();
    }
}
