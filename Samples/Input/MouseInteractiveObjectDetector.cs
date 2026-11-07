using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.InteractionSystem.Samples
{
    /// <summary>
    /// Sample: the pointed-at detector aimed from the mouse cursor instead of straight ahead.
    /// For a view with a locked cursor use <see cref="PointedInRangeObjectDetector"/> from the camera.
    /// </summary>
    public class MouseInteractiveObjectDetector : PointedInRangeObjectDetector
    {
        [SerializeField]
        private Camera pointerCamera;

        protected override void Reset()
        {
            base.Reset();
            pointerCamera = Camera.main;
        }

        protected override Ray GetRay()
        {
            if (pointerCamera == null || Mouse.current == null)
                return base.GetRay();

            return pointerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
    }
}
