using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.Interactions
{
	public class NearestInteractiveObjectDetector : MonoBehaviour
	{
		[SerializeField]
		private SingleObjectInteractor interactor;

		[SerializeField]
		private float detectionRadius;

		[SerializeField]
		private int maxDetectedObjects;

		[SerializeField]
		private LayerMask detectedLayers;

		[Header("States")]
		[SerializeField]
		private Collider[] detectedColliders;

		[SerializeField]
		private List<InteractiveObject> detectedObjects;

		private IComparer<InteractiveObject> distanceComparer;

		private void OnEnable()
		{
			distanceComparer = new InteractiveObjectsDistanceComparer(transform);
			detectedColliders = new Collider[maxDetectedObjects];
		}

		private void Update() => DetectInteractiveObject();

		private void DetectInteractiveObject()
		{
			detectedObjects.Clear();
			int count = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, detectedColliders, detectedLayers);
			if (count <= 0)
				return;
			
			for (int i = 0; i < count; i++)
			{
				var collider = detectedColliders[i];
				var interactiveObject = collider.GetComponentInParent<InteractiveObject>();
				if (interactiveObject == null)
					continue;

				detectedObjects.Add(interactiveObject);
			}
			detectedObjects.Sort(distanceComparer);

			var nearestInteractiveObject = detectedObjects.Count > 0 ? detectedObjects[0] : null;
			interactor.InteractiveObject = nearestInteractiveObject;
		}

		private void OnValidate()
		{
			if (Application.isPlaying == false)
				return;

			if (detectedColliders == null)
				return;

			int oldCount = detectedColliders.Length;
			if (maxDetectedObjects > oldCount)
			{
				var old = detectedColliders;
				detectedColliders = new Collider[maxDetectedObjects];
				System.Array.Copy(old, detectedColliders, oldCount);
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}

		internal struct InteractiveObjectsDistanceComparer : IComparer<InteractiveObject>
		{
			private readonly Transform _detectorTransform;
			private int frameNumber;

			private Vector3 position;

			public InteractiveObjectsDistanceComparer(Transform detectorTransform)
			{
				_detectorTransform = detectorTransform;
				frameNumber = Time.frameCount;
				position = detectorTransform.position;
			}

			public int Compare(InteractiveObject lhs, InteractiveObject rhs)
			{
				int currentFrame = Time.frameCount;
				if (frameNumber != currentFrame)
				{
					frameNumber = currentFrame;
					position = _detectorTransform.position;
				}

				float leftSqrDistance = (lhs.transform.position - position).sqrMagnitude;
				float rightSqrDistance = (rhs.transform.position - position).sqrMagnitude;
				return leftSqrDistance.CompareTo(rightSqrDistance);
			}
		}

	}
}
