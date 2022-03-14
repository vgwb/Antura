using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public class LivingLetterRagdoll : MonoBehaviour, ICollidable
    {
        public Animator animator;

        public Transform headTransform;
        public Transform pelvisTransform;

        public GameObject puffPrefab;

        bool ragdolling = false;
        float puffCountdown = 0.0f;

        public float maxPoofCountdownAfterHit = 0.25f;

        const float MAX_RAGDOLL_TIME = 4.0f;
        public bool deleteOnRagdollHit = true;

        // For Test purposes
        public bool doRagdoll = false;

        bool isRagdoll = false;
        Rigidbody[] rigidBodies;

        public event System.Action onPoofed;

        void Awake()
        {
            if (rigidBodies == null)
            {
                rigidBodies = GetComponentsInChildren<Rigidbody>(true);

                var forwardTarget = new GameObject[] { gameObject };
                for (int i = 0; i < rigidBodies.Length; ++i)
                {
                    rigidBodies[i].isKinematic = true;
                    rigidBodies[i].useGravity = false;
                    rigidBodies[i].gameObject.AddComponent<CollisionForwarder>().forwardTo = forwardTarget;
                }
            }
        }

        void Update()
        {
            if (doRagdoll)
            {
                doRagdoll = false;
                ragdolling = true;

                SetRagdoll(true, 10 * Vector3.one);
            }

            if (ragdolling)
            {
                puffCountdown -= Time.deltaTime;

                if (puffCountdown < 0.0f)
                {
                    if (deleteOnRagdollHit)
                    {
                        Puff();
                    }
                }
            }
        }

        public void SetRagdoll(bool active, Vector3 initialVelocity)
        {
            ragdolling = true;
            puffCountdown = MAX_RAGDOLL_TIME * (0.7f + 0.3f * Random.value);
            animator.enabled = !active;

            if (active && !isRagdoll)
            {
                if (rigidBodies == null)
                    Awake();

                for (int i = 0; i < rigidBodies.Length; ++i)
                {
                    rigidBodies[i].isKinematic = false;
                    rigidBodies[i].useGravity = true;
                    rigidBodies[i].velocity = initialVelocity;
                }
            }

            isRagdoll = active;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                puffCountdown = Mathf.Min(puffCountdown, maxPoofCountdownAfterHit);
            }
        }

        public void Puff()
        {
            var puffGo = GameObject.Instantiate(puffPrefab);
            puffGo.AddComponent<AutoDestroy>().duration = 2;
            puffGo.SetActive(true);

            puffGo.transform.position = headTransform.position;

            if (onPoofed != null)
                onPoofed();

            Destroy(gameObject);
        }
    }
}
