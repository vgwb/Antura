
using System;
using Antura.Core;
using Antura.Rewards;
using UnityEngine;
using System.Collections.Generic;
using Antura.Dog;
using Antura.AnturaSpace.UI;

namespace Antura.Discover
{
    public class PlayerActions : MonoBehaviour
    {
        public PlayerController player;

        void Start()
        {
            // Subscribe to fall damage event
            player.OnFallDamage += HandleFallDamage;
        }

        void HandleFallDamage(float fallHeight, float damage, GameObject hitObject)
        {
            Debug.Log($"PlayerActions: HandleFallDamage - Fall Height: {fallHeight}, Damage: {damage}, Hit Object: {hitObject?.name}");

            // Show message
            // messageUI.ShowMessage($"Ouch! Fell {fallHeight:F1} meters!");

            // Apply damage to player health
            //PlayerHealth.TakeDamage(damage);

            // Check what we landed on
            if (hitObject != null)
            {
                Debug.Log($"Landed on: {hitObject.name}");

                // If the object is a breakable crate, apply impact damage
                var crate = hitObject.GetComponent<BreakableCrate>();
                if (crate)
                {
                    crate.OnImpact(damage, isFallDamage: true);
                }
            }
        }
    }
}
