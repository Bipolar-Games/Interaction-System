namespace Bipolar.InteractionSystem.Tests
{
    /// <summary>An interaction type that is also an interaction and a behavior; it counts uses and can refuse.</summary>
    public class RecordingInteraction : SimpleInteraction
    {
        public int Count { get; private set; }
        public bool Refuses { get; set; }
        public Interactor LastInteractor { get; private set; }
        public IInteractorBehavior LastBehavior { get; private set; }

        public override bool Interact(InteractionContext context)
        {
            Count++;
            LastInteractor = context.Interactor;
            LastBehavior = context.Behavior;
            return Refuses == false;
        }
    }
}
