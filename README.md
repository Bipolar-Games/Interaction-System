# Interaction System

A small Unity module for "the player does something to an object": opening a door, talking to a character, picking something up. It answers three questions and keeps them apart:

1. **What is the player looking at / standing next to?** (detectors)
2. **What can be done with it, and what can the player do?** (interaction types, matched on both sides)
3. **What happens when it is done?** (interactions, written by you, per object)

The module itself never decides what a door or a lamp does. It only connects the player to your code.

## Dependencies
- [Interface Serialization](https://github.com/Bipolar-Games/Interface-Serialization) (interfaces shown in the Inspector)
- TextMeshPro and Naughty Attributes: the `Bipolar.InteractionSystem` assembly definition references `Unity.TextMeshPro` and `NaughtyAttributes.Core`, so both must be in the project for it to compile.
- The Raycast System's `InputSystemFromMouseRayProvider` is compiled only when the Input System package is enabled (`ENABLE_INPUT_SYSTEM`).

## The idea in one picture

```
 Detector ──sets──▶ DefaultInteractor ──TryInteract(type)──▶ InteractiveObject ──▶ Interaction (your code)
 (who is near /        │   ▲                                      │  list of
  pointed at)          │   └── InteractorDescriptor               │  ObjectInteraction
                       │       (types the player CAN do)          └─ each lists the InteractionTypes it handles
                       └──▶ DefaultInteractorHintsManager ──▶ HintsUIManager ──▶ HintDisplay (UI)
```

An interaction happens only when **both** sides know the type. A door that handles `Open` does nothing for a player whose descriptor has no `Open`, and the other way round. This is how you give different characters different abilities without any `if`.

## Concepts

### `InteractionType`
A ScriptableObject that is only a name/identity ("Open", "Talk", "Pick Up"). Create it with *Create ▸ Bipolar ▸ Interaction System ▸ Interaction Type*. Types are compared by reference, so adding a new one means adding a new asset, not editing an enum.

### `SimpleInteraction`
A ready-made asset that is **an interaction type, an object interaction and an interactor behavior at once**. It just logs `Interacted with <name>`. Derive from it and override `Interact` for something that needs no per-object setup. *Create ▸ Bipolar ▸ Interaction System ▸ Simple Interaction*.

### Object side: `InteractiveObject` and interactions
Put `InteractiveObject` on the object (it is looked up with `GetComponentInParent` from the hit collider, so it can be on a parent). Its `Interactions` list holds `IObjectInteraction`s. An interaction can be

- a **component**: derive from `SceneObjectInteraction` and set its `Interaction Types` in the Inspector (use this when it needs scene references, e.g. a door), or
- an **asset**: derive from `ObjectInteractionAsset` (use this when it is shared and needs no scene references).

```csharp
using Bipolar.InteractionSystem;
using UnityEngine;

// Lives next to a plain Door component; the Door knows nothing about interaction.
public class DoorInteraction : SceneObjectInteraction
{
    [SerializeField]
    private Door door;

    public override void Interact(Interactor interactor, IInteractorBehavior behavior)
    {
        door.Open();
    }
}
```

`GenericObjectInteraction` calls a `UnityEvent` instead, for quick prototyping without code.

### Player side: `Interactor`, `DefaultInteractor`, `InteractorDescriptor`
- `InteractorDescriptor` lists what the player can do: a list of *interactor behaviors* (`IInteractorBehavior`), each declaring interaction types. A `SimpleInteraction` asset can be used directly as a behavior. A behavior is also passed to the object's interaction, so an interaction can tell *how* it was done.
- `DefaultInteractor` holds the **current interactive object** (set by a detector) and `AvailableInteractions` (types both sides know). `TryInteract(InteractionType)` returns `true` if the interaction was carried out. It raises `OnInteractiveObjectChanged` when the current object changes.
- To use another selection logic, derive from `Interactor` (`TryInteract` is the only abstract method).

### Detectors
They only choose the current object and write it to `DefaultInteractor.InteractiveObject`.

| Component | Chooses |
|---|---|
| `NearestInteractiveObjectDetector` | the closest `InteractiveObject` within `Detection Radius` (overlap sphere on `Detected Layers`; set `Max Detected Objects` above 0) |
| `PointedInRangeObjectDetector` | the object under the **mouse cursor** if it is within `Max Object Distance` |

### Triggering interactions
`PlayerInteractorController` maps keys to interaction types (`Controls` list) and calls `TryInteract` when a key is pressed. It reads the legacy `UnityEngine.Input` class, so it works only when the legacy Input Manager is enabled. With the new Input System write your own small component that calls `interactor.TryInteract(type)` from an input action.

### Hints (UI)
`DefaultInteractorHintsManager` turns the current object and its available interactions into `IHint`s (name of the object, name of each interaction type) and raises `OnHintsUpdated`. `UI/HintsUIManager` pushes them into `HintDisplay` components. `HintDisplay` is abstract: derive from it and implement `Refresh(IHint)` to show the text (TextMeshPro label, icon...).

## Quick start: a door the player opens with a key

1. Create a *Simple Interaction* asset named **Open**. It is the interaction type and, at the same time, the player's behavior for it.
2. Player: add `DefaultInteractor`, `InteractorDescriptor` (assign it to the interactor) and a detector. Add the **Open** asset to the descriptor's `Behaviors`.
3. Door: add `InteractiveObject` with a collider on the same object or a child. Add your `DoorInteraction` component (as above) with the **Open** asset in its `Interaction Types`, then add it to the `InteractiveObject`'s `Interactions`.
4. Call `interactor.TryInteract(open)` from your input handling (or add `PlayerInteractorController` if you use the legacy Input Manager).

## Raycast System

A separate assembly (`Bipolar.RaycastSystem`) that tells you which object a ray hits, with enter/exit events. It does not depend on the interaction system.

- `RaycastController` casts one ray per frame (`Raycasted Layers`, `Raycast Distance`, trigger handling) and raises `OnRayEntered` / `OnRayExited`. `Target` is the current `RaycastTarget`.
- `RaycastCollider` goes on the collider that is hit and points to the `RaycastTarget`(s) it belongs to (filled by `Reset` from the parents). Only colliders with this component are detected.
- `RaycastTarget` exposes `OnRayEntered` / `OnRayExited`.
- `RaycastControllerEvents` forwards the controller's events to `UnityEvent`s.
- The ray comes from a `RayProvider` on the controller. Included: `MainCameraForwardRayProvider` (screen centre, typical for first person), `TransformForwardRayProvider`, `InputSystemFromMouseRayProvider` and `InputManagerRayFromMouseProvider` (mouse). Derive from `RayProvider` and return a `Ray` for your own. `controller.SetRayProvider<T>()` adds and assigns one in code.

## Extending
- **New action:** new `InteractionType` asset, plus an interaction (component or asset) on objects and a behavior in the descriptor of whoever can do it.
- **New way to pick the object:** a component that sets `DefaultInteractor.InteractiveObject` (to `null` when nothing is picked).
- **New way to trigger:** call `DefaultInteractor.TryInteract(type)`.

## Known limitations
- The module is not wired to the Raycast System. The only pointing detector aims from the mouse cursor, which does not suit a first-person view with a locked cursor.
- `PlayerInteractorController` and `PointedInRangeObjectDetector` read the legacy `Input` class and throw in projects that use the new Input System only.
- `NearestInteractiveObjectDetector` keeps the last object when nothing is in range anymore.
- `Interact` returns nothing, so an object cannot refuse (locked door) or report why.
- Hint texts are the asset/object names; there is no localization hook.
