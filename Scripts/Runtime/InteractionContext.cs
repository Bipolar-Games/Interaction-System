namespace Bipolar.InteractionSystem
{
    /// <summary>
    /// What an object interaction receives: who interacts and how. Only the module can create it
    /// (through an <see cref="Interactor"/>), so an interaction cannot be triggered by calling it directly.
    /// </summary>
    public readonly struct InteractionContext
    {
        internal InteractionContext(Interactor interactor, IInteractorBehavior behavior)
        {
            Interactor = interactor;
            Behavior = behavior;
        }

        public Interactor Interactor { get; }

        /// <summary>The interactor's way of doing it; <c>null</c> when it has none.</summary>
        public IInteractorBehavior Behavior { get; }
    }
}
