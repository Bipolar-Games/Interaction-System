#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.RaycastSystem
{
	[AddComponentMenu(AddComponentPath.RayProviders + "Input System Ray From Mouse Provider")]
    public class InputSystemFromMouseRayProvider : RayFromMouseProvider
    {
		protected override Vector2 GetScreenPosition() => 
			Mouse.current.position.ReadValue();
	}
}
#endif
