using UnityEngine;

namespace Bipolar.RaycastSystem
{
	[AddComponentMenu(AddComponentPath.RayProviders + "Main Camera Forward Provider")]
	public class MainCameraForwardRayProvider : RayProvider
    {
        private Transform mainCameraTransform;
        private Transform MainCameraTransform
        {
            get 
            {
                if (mainCameraTransform == null)
                    mainCameraTransform = Camera.main.transform;
                return mainCameraTransform; 
            }
        }

		public override Ray GetRay() => 
            new Ray(MainCameraTransform.position, MainCameraTransform.forward);
	}
}
