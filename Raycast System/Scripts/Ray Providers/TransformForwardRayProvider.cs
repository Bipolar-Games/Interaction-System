using UnityEngine;

namespace Bipolar.RaycastSystem
{
	[AddComponentMenu(AddComponentPath.RayProviders + "Transform Forward Provider")]
	public class TransformForwardRayProvider : RayProvider
    {
		public override Ray GetRay() => 
			new Ray(transform.position, transform.forward);
	}
}
