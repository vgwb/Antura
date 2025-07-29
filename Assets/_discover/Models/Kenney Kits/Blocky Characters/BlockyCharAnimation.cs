using UnityEngine;
using System;

namespace Antura.Animation
{
    public class BlockyCharAnimation : MonoBehaviour
    {

        [Tooltip("The Animator component for this NPC.")]
        public Animator npcAnimator;

        [Tooltip("The name of the 'Speed' float parameter in your Animator Controller.")]
        public string speedParameterName = "speedChange"; // IMPORTANT: Change this to the EXACT name of your parameter

        [Tooltip("Set the desired animation speed multiplier for this NPC.")]
        [Range(0.1f, 5f)] // Optional: Adds a slider in the Inspector for easier adjustment
        public float animationSpeedMultiplier = 1f;

        public enum AnimationState
        {
            None,
            Idle,
            EmoteYes,
            EmoteNo,
            Sit,
            Walk
        }

        [Header("Animation Selector")]
        public AnimationState selectedAnimation = AnimationState.None;

        void Start()
        {
            // Get the Animator component if not assigned in the Inspector
            if (npcAnimator == null)
            {
                npcAnimator = GetComponent<Animator>();
            }

            // Check if the Animator and parameter name are set
            if (npcAnimator == null)
            {
                Debug.LogError("NPC Animator not found on " + gameObject.name);
                return;
            }

            if (string.IsNullOrEmpty(speedParameterName))
            {
                Debug.LogError("Speed Parameter Name is not set in the NpcAnimationSpeedController script on " + gameObject.name);
                return;
            }

            // Set the Animator's float parameter to the desired speed multiplier
            UpdateAnimatorSpeed();

            // Play the selected animation at start
            PlaySelectedAnimation(selectedAnimation);
        }

        // Call this method if you want to change the speed at runtime from another script
        public void SetAnimationSpeed(float newSpeed)
        {
            animationSpeedMultiplier = newSpeed;
            UpdateAnimatorSpeed();
        }

        private void UpdateAnimatorSpeed()
        {
            if (npcAnimator != null && !string.IsNullOrEmpty(speedParameterName))
            {
                npcAnimator.SetFloat(speedParameterName, animationSpeedMultiplier);
            }
        }

        /// <summary>
        /// Plays the given animation state by setting the corresponding trigger in the Animator.
        /// </summary>
        public void PlaySelectedAnimation(AnimationState state)
        {
            if (npcAnimator == null)
                return;

            string stateName = null;
            switch (state)
            {
                case AnimationState.None:
                    stateName = "static";
                    break;
                case AnimationState.Idle:
                    stateName = "idle";
                    break;
                case AnimationState.EmoteYes:
                    stateName = "emote-yes";
                    break;
                case AnimationState.EmoteNo:
                    stateName = "emote-no";
                    break;
                case AnimationState.Sit:
                    stateName = "sit";
                    break;
                case AnimationState.Walk:
                    stateName = "walk";
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(stateName))
            {
                npcAnimator.Play(stateName);
            }
        }

        // Overload for backwards compatibility
        public void PlaySelectedAnimation()
        {
            PlaySelectedAnimation(selectedAnimation);
        }

        // Optional: If you want to adjust it live in the editor during play mode
        // and see immediate changes, you can use OnValidate or Update.
        // OnValidate is called when the script is loaded or a value is changed in the Inspector.
        void OnValidate()
        {
            // Try to get the Animator if not assigned
            if (npcAnimator == null)
            {
                npcAnimator = GetComponent<Animator>();
            }

            // Only proceed if Animator exists and has a controller assigned
            if (npcAnimator != null && npcAnimator.runtimeAnimatorController != null && !string.IsNullOrEmpty(speedParameterName))
            {
                // Only set parameter and play animation if in play mode to avoid warnings in edit mode
                if (Application.isPlaying)
                {
                    try
                    {
                        // Check if the parameter exists before trying to set it
                        bool parameterExists = false;
                        foreach (AnimatorControllerParameter param in npcAnimator.parameters)
                        {
                            if (param.name == speedParameterName && param.type == AnimatorControllerParameterType.Float)
                            {
                                parameterExists = true;
                                break;
                            }
                        }

                        if (parameterExists)
                        {
                            npcAnimator.SetFloat(speedParameterName, animationSpeedMultiplier);
                        }

                        // Play the selected animation when changed in Inspector
                        PlaySelectedAnimation(selectedAnimation);
                    }
                    catch (System.Exception)
                    {
                        // Optionally log the exception if needed
                        // Debug.LogWarning($"Could not set animator speed parameter in OnValidate: {e.Message}");
                    }
                }
            }
        }
    }
}
