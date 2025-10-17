using System.Collections;
using UnityEngine;

namespace Bipolar.InteractionSystem.Samples
{ 
	public interface IOpenable
	{
		bool IsOpen { get; set; }
	}

	public class OpenableDoor : MonoBehaviour, IOpenable
	{
		[SerializeField]
		private Transform openedObject;

		[SerializeField]
		private float closedAngle = 0;

		[SerializeField]
		private float openAngle = 90;

		[SerializeField]
		private float rotationSpeed = 300;

		[Header("States")]
		[SerializeField]
		private bool isOpen = false;
		public bool IsOpen
		{
			get => isOpen;
			set
			{
				isOpen = value;
				StopAllCoroutines();
				StartCoroutine(RotatingCo());
			}
		}

		public void Toggle()
		{
			IsOpen = !IsOpen;
		}

		private void Awake()
		{
			RefreshRotation();
		}

		private void RefreshRotation()
		{
			float angle = isOpen ? openAngle : closedAngle;
			openedObject.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
		}

		private IEnumerator RotatingCo()
		{
			float target = isOpen ? openAngle : closedAngle;
			float angle = openedObject.localEulerAngles.y;
			while (angle != target)
			{
				angle = Mathf.MoveTowardsAngle(angle, target, rotationSpeed * Time.deltaTime);
				openedObject.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
				yield return null;
			}
			openedObject.localRotation = Quaternion.AngleAxis(target, Vector3.up);
		}

		private void OnValidate()
		{
			RefreshRotation();
		}
	}
}