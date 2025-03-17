using UnityEngine;

namespace Bipolar.Interactions
{
	public class PointedInRangeObjectDetector : MonoBehaviour
	{
		[SerializeField]
		private DefaultInteractor interactor;

		[SerializeField]
		private float maxObjectDistance = 2;

		[SerializeField]
		private LayerMask detectedLayers = -1;

		private Ray ray;

		private void Update()
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			interactor.InteractiveObject = GetPointedInteractiveObject(ray);
		}

		public InteractiveObject GetPointedInteractiveObject(Ray ray)
		{
			if (Physics.Raycast(ray, out var hitInfo, 100, detectedLayers) == false)
				return null;

			var collider = hitInfo.collider;
			if (collider == null)
				return null;

			var interactiveObject = collider.GetComponentInParent<InteractiveObject>();
			if (interactiveObject == null)
				return null;

			float sqrDistance = (interactiveObject.transform.position - transform.position).sqrMagnitude;
			if (sqrDistance > maxObjectDistance)
				return null;
			
			return interactiveObject;
		}

		private void OnDrawGizmosSelected()
		{
			if (enabled == false)
				return;

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 100);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, maxObjectDistance);
		}
	}
}
