using System.Collections;
using UnityEngine;

namespace Antura.Discover.Interaction
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySound : ActableAbstract
    {
        [Header("--- PlaySound Settings")]

        public AudioClip SoundFx;
        public AudioSource audioSource;

        [Header("Sound Options")]
        public bool is3D = false;
        public bool loop = false;

        [Header("Trigger Options")]
        public bool useColliderTrigger = false;

        [Header("State")]
        public bool IsActivated = false;

        [Header("Visual Effect")]
        public bool scaleEffect = false;
        public float scaleAmount = 1.2f;
        public float scaleSpeed = 4f;
        public Transform scaleTarget; // The GameObject to scale

        private Vector3 originalScale;
        private GameObject playerObj;

        void Start()
        {
            // Use scaleTarget or fallback to this GameObject
            if (scaleTarget == null)
                scaleTarget = transform;
            originalScale = scaleTarget.localScale;

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (SoundFx != null)
            {
                audioSource.clip = SoundFx;
            }
            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            audioSource.spatialBlend = is3D ? 1f : 0f; // 0 = 2D, 1 = 3D

            playerObj = GameObject.FindGameObjectWithTag("Player");

            // Set maxDistance to collider radius if 3D
            if (is3D)
            {
                var sphere = GetComponent<SphereCollider>();
                if (sphere != null)
                {
                    audioSource.maxDistance = sphere.radius;
                }
            }
        }

        public override void OnTrigger()
        {
            Play();
        }

        void Update()
        {
            // Scale effect
            if (scaleEffect && scaleTarget != null)
            {
                if (IsActivated)
                {
                    // Oscillate between 0.99 and 1.01
                    float s = Mathf.Lerp(0.99f, 1.01f, (Mathf.Sin(Time.time * scaleSpeed) + 1f) * 0.5f);
                    scaleTarget.localScale = originalScale * s;
                }
                else
                {
                    scaleTarget.localScale = originalScale;
                }
            }

            // Distance check for activation
            if (is3D)
            {
                if (playerObj != null)
                {
                    float dist = Vector3.Distance(transform.position, playerObj.transform.position);
                    var sphere = GetComponent<SphereCollider>();
                    float radius = sphere != null ? sphere.radius : audioSource.maxDistance;
                    IsActivated = dist <= radius;
                }
                else
                {
                    IsActivated = false;
                }
            }
        }

        private void Play()
        {
            if (audioSource != null && SoundFx != null && !audioSource.isPlaying)
            {
                audioSource.Play();
                IsActivated = true;
                Debug.Log("Playing SFX: " + SoundFx.name);
                StartCoroutine(CheckIfStopped());
            }
        }

        private IEnumerator CheckIfStopped()
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }
            IsActivated = false;
        }

        // Optional: Play sound when entering 3D collider
        private void OnTriggerEnter(Collider other)
        {
            if (useColliderTrigger && is3D && other.CompareTag("Player"))
            {
                Play();
            }
        }
    }
}
