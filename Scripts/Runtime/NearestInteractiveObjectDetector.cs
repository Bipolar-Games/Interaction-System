using UnityEngine;

namespace Bipolar.InteractionSystem
{
	/// <summary>Chooses the closest interactive object within a radius around this object.</summary>
	public class NearestInteractiveObjectDetector : InteractiveObjectDetector
	{
		[SerializeField]
		private float detectionRadius = 2;

		[Tooltip("How many colliders are examined at most; closer ones may be missed in a crowd.")]
		[SerializeField]
		private int maxDetectedObjects = 16;

		[SerializeField]
		private LayerMask detectedLayers = -1;

		[SerializeField]
		private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

		private Collider[] detectedColliders = new Collider[0];

		protected override void OnEnable()
		{
			base.OnEnable();
			ResizeBuffer();
		}

		protected override InteractiveObject Detect()
		{
			int count = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, detectedColliders, detectedLayers, triggerInteraction);

			InteractiveObject nearest = null;
			float nearestSqrDistance = float.MaxValue;
			for (int i = 0; i < count; i++)
			{
				var interactiveObject = GetInteractiveObject(detectedColliders[i]);
				if (interactiveObject == null)
					continue;

				float sqrDistance = (interactiveObject.transform.position - transform.position).sqrMagnitude;
				if (sqrDistance < nearestSqrDistance)
				{
					nearest = interactiveObject;
					nearestSqrDistance = sqrDistance;
				}
			}
			return nearest;
		}

		private void ResizeBuffer()
		{
			int size = Mathf.Max(1, maxDetectedObjects);
			if (detectedColliders.Length != size)
				detectedColliders = new Collider[size];
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
				ResizeBuffer();
		}

		private void OnDrawGizmosSelected()
		{
			if (enabled == false)
				return;

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}
	}
}
