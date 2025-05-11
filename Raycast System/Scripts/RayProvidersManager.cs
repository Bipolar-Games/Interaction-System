namespace Bipolar.RaycastSystem
{
    public static class RayProvidersExtension
    {
		public static void SetRayProvider<T>(this RaycastController controller) where T : RayProvider
		{
			if (controller.TryGetComponent<T>(out var rayProvider) == false)
				rayProvider = controller.gameObject.AddComponent<T>();

			controller.RayProvider = rayProvider;
		}
	}
}
