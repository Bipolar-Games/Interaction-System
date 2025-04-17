namespace Bipolar.Interactions
{
	public interface IObjectInteraction : IInteraction
	{
		void Interact(Interactor interactor, IInteractionHandler interactorInteraction);
	}

	[System.Serializable]
	public class ObjectInteraction : Serialized<IObjectInteraction>, IObjectInteraction
	{
		public void Interact(Interactor interactor, IInteractionHandler interactorInteraction) => Value.Interact(interactor, interactorInteraction);
	}
}
