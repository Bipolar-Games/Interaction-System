namespace Bipolar.InteractionSystem.Tests
{
    /// <summary>An interactor written outside the module's own: it can still interact, through Perform.</summary>
    public class CustomInteractor : Interactor
    {
        public InteractiveObject Target { get; set; }
        public IInteractorBehavior Behavior { get; set; }
        public IObjectInteraction LastPerformed { get; private set; }

        public override bool TryInteract(InteractionType interaction)
        {
            LastPerformed = Perform(Target, interaction, Behavior);
            return LastPerformed != null;
        }
    }
}
