---
title: Antura and Living Letters
nav_order: 0
---
# Antura and Living Letters

_work in progress_

## Antura

The contents of the AnturaSpace folder handle interactions with Antura in the AnturaSpace scene, used for reward and customization purposes.

The Antura classes are used to control Antura's behaviours, its animations, and define the appearance of rewards.
**AnturaAnimationController** and **AnturaWalkBehaviour** control the animation state of Antura.

In the **_app/Resources/Prefabs/Actors/** directory you will find a prefab named **Antura**.

That is the prefab for the animated living letter that should be used by all the MiniGames.

For Antura, you must use the original prefab **without breaking the prefab reference**.
If you need a custom prefab, instantiate it in the scene, add your components on it (this will not break the reference to the original prefab), disable it in the scene, and use that as prefab (e.g. dragging it in the inspector of the scene’s components). Remember to reactivate it upon instantiation.

The prefab has a **AnturaAnimationController** component that let you change animation and set the arabic word/letter on it. It is pretty similar to the LL view.

You can switch state by using the following property:
`AnturaAnimationStates State`

The supported states are:

```C#
idle,  // Antura is standing
walking, // Antura walking/running,
sitting, // Antura is sitting
sleeping, // Antura is sleeping
sheeping, // Antura is jumping in place
sucking // Antura is inhaling
```

Properties:

Such property:
**_bool IsAngry_**
is used when Antura is sitting, or running to select a special sitting/running animation.

Such properties are used when Antura is idle to select a special idle animation.

**_bool isExcited;_**

**_bool isSad;_**

To switch between Walking and running use:

**_void SetWalkingSpeed(speed);_**

*the animation will blend between walk (speed = 0) and run (speed = 1).*

```C#
void DoBark();

void DoSniff();

void DoShout();

void DoBurp();

DoSpit(bool openMouth);
```

The following methods can be used to perform a jump. Animations are in place, so you have to move transform when performing jump and just notify the animator with the following events.
Such events must be called in this order:

```C#
void OnJumpStart();

void OnJumpMaximumHeightReached();

void OnJumpGrab();

void OnJumpEnded();
```

This method:

`void DoCharge(System.Action onChargeEnded);`
makes Antura do an angry charge.
The Dog makes an angry charging animation (it must stay in the same position during this animation);
IsAngry is set to true automatically (needed to use the angry run).

After such animation ends, **_onChargeEnded_** will be called to inform you, and passes automatically into running state.
You should use **_onChargeEnded_** to understand when you should begin to move the antura's transform.

## LivingLetters

In the **_app/Resources/Prefabs/Actors/** directory you will find a prefab named **LivingLetter**.

That is the prefab for the animated living letter that should be used by all the MiniGames.

For the LLs, you must use the LL prefab in `_app/Resources/Prefabs/Actors/LLPrefab` **without breaking the prefab reference**.
If you need a custom prefab, instantiate it in the scene, add your components on it (this will not break the reference to the original prefab), disable it in the scene, and use that as prefab (e.g. dragging it in the inspector of the scene’s components). Remember to reactivate it upon instantiation.

The prefab has a **LetterObjectView** component that let you change animation and set the arabic word/letter on it.

To set the current vocabulary data, use one of the following methods:

```C#
void Initialize(ILivingLetterData _data);
void Initialize(ILivingLetterData data, string customText, float scale);
```

by passing the data that you want to see displayed on the LL.

Use `letterObjectView.Data` to get the current data.

Then, you can drive the animations using the following interface.

```C#
bool Crouching; // the LL is crouching
bool Falling; // the LL is falling*
bool Horraying; // continous horray
```

You can switch state by using the following method:
`void SetState(LLAnimationStates newState);`

The supported states are:
```C#
LL_idle, // when the LL is standing
LL_walking, // when the LL is walking or running
LL_dragging, // when the player is dragging the LL
LL_hanging, // special state for Baloons game (still waiting for animation in the fbx)
LL_dancing, // Dance!
LL_rocketing, // LL on the Rocket (use DoHorray/{set horraying} for rocket hooray)
LL_tickling, // LL is tickling
LL_limbless // when the LL has no arms and legs
```

To switch between Walking and running use:

`void SetWalkingSpeed(speed);`

*the animation will blend between walk (speed = 0) and run (speed = 1).*

Special animation triggers (it will perform an animation and go back to idle).

```C#
void DoHorray(); // triggers a single horray
void DoAngry();
void DoHighFive();
void DoDancingWin();
void DoDancingLose();
void ToggleDance(); // Switch dance between dancing1 and dancing2
void DoTwirl(System.Action onLetterShowingBack);
```

The DoTwirl animation will trigger your callback when the letter is showing its back to the camera (so you can change letter in that moment).

The following methods can be used to perform a jump. Animations are in place, so you have to move transform when performing jump and just notify the animator with the following events.

```C#
void OnJumpStart();
void OnJumpMaximumHeightReached();
void OnJumpEnded();
```

The Living Letter View has a **Poof()** method that let you create a "poof" particle animation in the current position of the letter. You can use it when you want to make the LL disappear and re-appear on another position, or simply destroy it;
