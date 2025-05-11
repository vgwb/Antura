using UnityEngine;
using System;

public class NpcAnimatorParameter : MonoBehaviour
{

    [Tooltip("The Animator component for this NPC.")]
    public Animator npcAnimator;

    [Tooltip("The name of the 'Speed' float parameter in your Animator Controller.")]
    public string speedParameterName = "speedChange"; // IMPORTANT: Change this to the EXACT name of your parameter

    [Tooltip("Set the desired animation speed multiplier for this NPC.")]
    [Range(0.1f, 5f)] // Optional: Adds a slider in the Inspector for easier adjustment
    public float animationSpeedMultiplier = 1f;

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

    // Optional: If you want to adjust it live in the editor during play mode
    // and see immediate changes, you can use OnValidate or Update.
    // OnValidate is called when the script is loaded or a value is changed in the Inspector.
    void OnValidate()
    {
        // Ensure this only runs if npcAnimator is already assigned,
        // or try to get it if in Play mode.
        if (npcAnimator == null && Application.isPlaying)
        {
            npcAnimator = GetComponent<Animator>();
        }
        else if (npcAnimator == null && !Application.isPlaying)
        {
            // Attempt to get it if not playing, but be mindful this runs in edit mode
            npcAnimator = GetComponent<Animator>();
        }


        if (npcAnimator != null && !string.IsNullOrEmpty(speedParameterName))
        {
            // To avoid errors if the parameter doesn't exist yet or animator isn't fully initialized
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

                if (parameterExists || Application.isPlaying) // Allow setting if playing, as parameter might be runtime generated or valid
                {
                    npcAnimator.SetFloat(speedParameterName, animationSpeedMultiplier);
                }
                else if (!parameterExists && !Application.isPlaying)
                {
                    // Debug.LogWarning($"Animator on {gameObject.name} does not have a float parameter named '{speedParameterName}'. Will try to set at runtime.");
                }
            }
            catch (System.Exception e)
            {
                // Debug.LogWarning($"Could not set animator speed parameter in OnValidate: {e.Message}");
            }
        }
    }
}

