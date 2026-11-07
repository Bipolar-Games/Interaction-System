using UnityEngine;

namespace Bipolar.InteractionSystem
{
	/// <summary>
	/// Chooses the interactive object that a ray hits first, if the hit is within reach of this object.
	/// By default the ray starts at <see cref="rayOrigin"/> (the camera, a hand...) and goes forward, so it needs
	/// no input. Override <see cref="GetRay"/> to aim differently, e.g. from a mouse cursor.
	/// </summary>
	public class PointedInRangeObjectDetector : InteractiveObjectDetector
	{
		[Tooltip("Where the ray starts and where it points (its forward). Usually the camera. Empty = this object.")]
		[SerializeField]
		private Transform rayOrigin;

		[Tooltip("How far the player can reach, measured from this object to the point that was hit.")]
		[SerializeField]
		private float maxObjectDistance = 2;

		[SerializeField]
		private LayerMask detectedLayers = -1;

		[SerializeField]
		private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

		private Ray lastRay;

		protected override void Reset()
		{
			base.Reset();
			if (Camera.main)
				rayOrigin = Camera.main.transform;
		}

		protected override InteractiveObject Detect()
		{
			lastRay = GetRay();

			// Any point within reach of this object is at most this far from the ray's start.
			float rayLength = maxObjectDistance + Vector3.Distance(lastRay.origin, transform.position);
			if (Physics.Raycast(lastRay, out var hit, rayLength, detectedLayers, triggerInteraction) == false)
				return null;

			float sqrReach = (hit.point - transform.position).sqrMagnitude;
			if (sqrReach > maxObjectDistance * maxObjectDistance)
				return null;

			return GetInteractiveObject(hit.collider);
		}

		/// <summary>The ray to cast this frame.</summary>
		protected virtual Ray GetRay()
		{
			var origin = rayOrigin ? rayOrigin : transform;
			return new Ray(origin.position, origin.forward);
		}

		private void OnDrawGizmosSelected()
		{
			if (enabled == false)
				return;

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(lastRay.origin, lastRay.origin + lastRay.direction * (maxObjectDistance + 1));

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, maxObjectDistance);
		}
	}
}
