using System.Collections;
using UnityEngine;

namespace Bipolar.InteractionSystem.Samples
{
    public class OpenDoorInteraction : SceneObjectInteraction
    {
        [SerializeField]
        private Transform openedObject;

        [SerializeField]
        private float closedAngle = 0;

        [SerializeField]
        private float openAngle = 90;

        [SerializeField]
        private float rotationSpeed = 300;

        [field: Header("States")]
        [field: SerializeField]
        public bool IsOpen { get; private set; } = false;

        private void OnEnable()
        {
            RefreshRotation();
        }

        private void RefreshRotation()
        {
            float angle = IsOpen ? openAngle : closedAngle;
            openedObject.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        public override void Interact(Interactor interactor, IInteractorBehavior behavior)
        {
            Toggle();
        }

        public void Open()
        {
            Rotate(openAngle);
            IsOpen = true;
        }

        public void Close()
        {
            Rotate(closedAngle);
            IsOpen = false;
        }

        private void Rotate(float target)
        {
            StopAllCoroutines();
            StartCoroutine(RotatingCo(target));
        }

        public void Toggle()
        {
            if (IsOpen)
                Close();
            else
                Open();
        }

        private IEnumerator RotatingCo(float target)
        {
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
