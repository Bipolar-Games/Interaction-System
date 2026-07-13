using UnityEngine;

namespace Bipolar.InteractionSystem
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
			Debug.DrawRay(ray.origin, ray.direction, Color.orange, 0.01f); 
			if (Physics.Raycast(ray, out var hitInfo, 2 * maxObjectDistance, detectedLayers) == false)
				return null;

			var collider = hitInfo.collider;
			if (collider == null)
				return null;

			var interactiveObject = collider.GetComponentInParent<InteractiveObject>();
			if (interactiveObject == null)
				return null;

			float sqrDistance = (interactiveObject.transform.position - transform.position).sqrMagnitude;
			if (sqrDistance > maxObjectDistance * maxObjectDistance)
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
