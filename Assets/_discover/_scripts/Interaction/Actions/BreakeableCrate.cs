using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Antura.Discover
{
    public class BreakableCrate : MonoBehaviour
    {
        [Header("Crate Settings")]
        [Tooltip("Total health before the crate breaks")]
        public float health = 50f;
        [Tooltip("Prefab spawned when crate is destroyed (broken pieces)")]
        public GameObject brokenPrefab;

        [Header("Impact Feedback")]
        [Tooltip("Particle prefab spawned on impact (optional)")]
        public GameObject hitParticlePrefab;
        [Tooltip("Seconds before auto-destroying the hit particle (0 = don't auto destroy)")]
        public float particleLifetime = 2f;
        [Tooltip("Optional sound played when crate breaks")]
        public AudioClip breakSound;
        [Range(0f, 1f)] public float breakSoundVolume = 0.9f;
        [Tooltip("Minimum damage required to spawn impact particle if crate not yet broken")]
        public float minParticleDamage = 1f;

        [Header("Fall Impact Bonus")]
        [Tooltip("Multiplier applied to fall damage coming from player (set via PlayerActions)")]
        public float fallDamageMultiplier = 1f;

        private bool _isBroken;

        /// <summary>
        /// Apply damage to the crate. If health reaches zero it breaks.
        /// </summary>
        /// <param name="damage">Raw damage value (already scaled).</param>
        /// <param name="isFallDamage">If true, applies fallDamageMultiplier.</param>
        public void OnImpact(float damage, bool isFallDamage = false)
        {
            if (_isBroken)
                return;
            if (isFallDamage && fallDamageMultiplier != 1f)
            {
                damage *= fallDamageMultiplier;
            }

            // Spawn hit particle for sufficiently strong hits even if not breaking
            if (hitParticlePrefab && damage >= minParticleDamage)
            {
                SpawnHitParticle();
            }

            health -= damage;
            if (health <= 0f)
            {
                Break();
            }
        }

        private void Break()
        {
            if (_isBroken)
                return;
            _isBroken = true;

            if (brokenPrefab)
            {
                Instantiate(brokenPrefab, transform.position, transform.rotation);
            }

            if (breakSound)
            {
                // Use AudioSource at position (safer if no global audio manager reference here)
                AudioSource.PlayClipAtPoint(breakSound, transform.position, breakSoundVolume);
            }

            // Final particle burst (even if already spawned one for impact)
            if (hitParticlePrefab)
            {
                SpawnHitParticle();
            }

            Destroy(gameObject);
        }

        private void SpawnHitParticle()
        {
            var go = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            if (particleLifetime > 0f)
            {
                Destroy(go, particleLifetime);
            }
        }

        private void OnValidate()
        {
            health = Mathf.Max(1f, health);
            particleLifetime = Mathf.Max(0f, particleLifetime);
            minParticleDamage = Mathf.Max(0f, minParticleDamage);
            fallDamageMultiplier = Mathf.Max(0.01f, fallDamageMultiplier);
        }
    }
}
