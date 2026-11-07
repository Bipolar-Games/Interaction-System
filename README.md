# Interaction System

A small Unity module for "the player does something to an object": opening a door, talking to a character, picking something up. It answers three questions and keeps them apart:

1. **What is the player looking at / standing next to?** (detectors)
2. **What can be done with it, and what can the player do?** (interaction types, matched on both sides)
3. **What happens when it is done?** (interactions, written by you, per object)

The module itself never decides what a door or a lamp does, and it does not read any input: you call `TryInteract` from whatever you use (a key, a gamepad, a voice command).

## Dependencies
- [Interface Serialization](https://github.com/Bipolar-Games/Interface-Serialization) (interfaces shown in the Inspector).
- Optional: [Naughty Attributes](https://github.com/dbrizov/NaughtyAttributes). It is only used behind `#if NAUGHTY_ATTRIBUTES`.
- `Bipolar.InteractionSystem.UI` (hint display) also needs TextMeshPro. The core assembly does not.

## Layout

| Folder | Assembly | Needs |
|---|---|---|
| `Scripts/Runtime` | `Bipolar.InteractionSystem` (core) | Interface Serialization |
| `Scripts/Runtime/UI` | `Bipolar.InteractionSystem.UI` | core, TextMeshPro |
| `Samples` | `Bipolar.InteractionSystem.Samples` | core |
| `Samples/Input` | `Bipolar.InteractionSystem.Samples.Input` | core, Input System (compiled only when it is enabled) |
| `Tests/PlayMode` | tests | |

## The idea in one picture

```
 Detector ──sets──▶ DefaultInteractor ──TryInteract(type)──▶ InteractiveObject ──▶ Interaction (your code)
 (who is near /        │   ▲                                      │  list of
  pointed at)          │   └── its Behaviors                      │  ObjectInteraction
                       │       (types the player CAN do)          └─ each lists the InteractionTypes it handles
                       └──▶ DefaultInteractorHintsManager ──▶ HintsUIManager ──▶ HintDisplay (UI)
```

An interaction happens only when **both** sides know the type. A door that handles `Open` does nothing for a player whose behaviors have no `Open`, and the other way round. This is how you give different characters different abilities without any `if`.

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

`GenericObjectInteraction` is meant for quick prototyping: the action is wired in the Inspector through a `UnityEvent`, no code needed. For anything that stays in the game, write a proper interaction class.

### Player side: `DefaultInteractor`
One component holds everything the player needs:

- `Behaviors`: what the player can do, a list of *interactor behaviors* (`IInteractorBehavior`), each declaring interaction types. A `SimpleInteraction` asset can be used directly as a behavior. The behavior is also passed to the object's interaction, so an interaction can tell *how* it was done. A behavior that needs scene references can be a component and still be listed here.
- `InteractiveObject`: the **current interactive object**, set by a detector. `OnInteractiveObjectChanged` is raised when it changes.
- `AvailableInteractions`: the types that both sides know.
- `TryInteract(InteractionType)` returns `true` if the interaction was carried out.

To use another selection logic, derive from `Interactor` (`TryInteract` is the only abstract method).

### Detectors
A detector decides which interactive object the interactor faces. All of them derive from `InteractiveObjectDetector`, which is the whole contract:

```csharp
public class LookedAtDetector : InteractiveObjectDetector
{
    protected override InteractiveObject Detect()
    {
        // return the object to interact with right now, or null when there is none
    }
}
```

The base class calls `Detect()` every frame, writes the answer to the interactor (so returning `null` clears it) and clears the interactor when the detector is switched off. It has a `GetInteractiveObject(Collider)` helper for the common "which interactive object owns this collider" step.

| Component | Chooses |
|---|---|
| `NearestInteractiveObjectDetector` | the closest `InteractiveObject` within `Detection Radius` (overlap sphere on `Detected Layers`) |
| `PointedInRangeObjectDetector` | the first thing hit by a ray from `Ray Origin` (the camera, a hand; its forward), if the hit point is within `Max Object Distance` of the detector. Anything in between blocks it. Override `GetRay()` to aim differently |

Keep the player's own colliders out of `Detected Layers`, or the ray will hit them first.

### Triggering interactions
The module does not read input. Call `interactor.TryInteract(type)` from your own code. `Samples/Input/InteractionInput` shows it with the Input System: a list of *interaction type ↔ `InputActionReference`* pairs. `Samples/Input/MouseInteractiveObjectDetector` is the pointing detector aimed from the mouse cursor.

### Hints (UI)
`DefaultInteractorHintsManager` (core) turns the current object and its available interactions into `IHint`s (name of the object, name of each interaction type) and raises `OnHintsUpdated`. In the UI assembly `HintsUIManager` pushes them into `HintDisplay` components. `HintDisplay` is abstract: derive from it and implement `Refresh(IHint)`. `TMPHintDisplay` is a ready one that writes the message into a TextMeshPro label.

## Quick start: a door the player opens with a key

1. Create a *Simple Interaction* asset named **Open**. It is the interaction type and, at the same time, the player's behavior for it.
2. Player: add `DefaultInteractor` and put the **Open** asset in its `Behaviors`. Add a detector (e.g. `PointedInRangeObjectDetector` with the camera as `Ray Origin`).
3. Door: add `InteractiveObject` with a collider on the same object or a child. Add your `DoorInteraction` component (as above) with the **Open** asset in its `Interaction Types`, then add it to the `InteractiveObject`'s `Interactions`.
4. Call `interactor.TryInteract(open)` from your input handling (or copy `Samples/Input/InteractionInput`).

## Extending
- **New action:** new `InteractionType` asset, plus an interaction (component or asset) on objects and a behavior in the `Behaviors` of whoever can do it.
- **New way to pick the object:** derive from `InteractiveObjectDetector`.
- **New way to trigger:** call `DefaultInteractor.TryInteract(type)`.

## Known limitations
- `Interact` returns nothing, so an object cannot refuse (locked door) or report why.
- Hint texts are the asset/object names; there is no localization hook.
- `HintsUIManager` throws if its object name display is not assigned.
