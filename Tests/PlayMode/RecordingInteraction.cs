namespace Bipolar.InteractionSystem.Tests
{
    /// <summary>An interaction type that is also an interaction and a behavior, and counts how often it was used.</summary>
    public class RecordingInteraction : SimpleInteraction
    {
        public int Count { get; private set; }

        public override void Interact(Interactor interactor, IInteractorBehavior behavior)
        {
            Count++;
        }
    }
}
