using Antura.Dog;
using UnityEngine;

namespace Antura.Minigames.MixedLetters
{
    public class AnturaController : MonoBehaviour
    {
        public static AnturaController instance;

        public CapsuleCollider capsuleCollider;

        private Vector3 velocity;
        private System.Action NotifyFightBeganCallback;
        private System.Action NotifyAnturaExitedScene;

        private AnturaAnimationController animator;

        private bool _lastEnteredFromTheLeft;
        public bool LastEnteredFromTheLeft
        {
            get
            {
                return _lastEnteredFromTheLeft;
            }
        }

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            capsuleCollider.enabled = true;

            animator = GetComponent<AnturaAnimationController>();

            Reset();
        }

        void Update()
        {
            Vector3 position = transform.position;
            position += velocity * Time.deltaTime;
            transform.position = position;

            if (IsOffScreen() && velocity.x * transform.position.x > 0)
            {
                NotifyAnturaExitedScene();
                Disable();
            }
        }

        public void PrepareToEnterScene()
        {
            Vector3 victimLLPosition = VictimLLController.instance.transform.position;
            Vector3 anturaPosition = victimLLPosition;

            float frustumHeight = 2.0f * Mathf.Abs(anturaPosition.z - Camera.main.transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Camera.main.aspect;

            anturaPosition.x = frustumWidth / 2 + 12f;

            if (Random.Range(0f, 1f) <= 0.5f)
            {
                anturaPosition.x *= -1;
            }

            _lastEnteredFromTheLeft = anturaPosition.x < 0f;

            transform.position = anturaPosition;
        }

        public void EnterScene(System.Action notifyFightBeganCallback, System.Action notifyAnturaExitedScene)
        {
            velocity.x = VictimLLController.instance.transform.position.x - transform.position.x;

            if (velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 270, 0);
            }

            else
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            NotifyFightBeganCallback = notifyFightBeganCallback;
            NotifyAnturaExitedScene = notifyAnturaExitedScene;
        }

        private bool IsOffScreen()
        {
            float frustumHeight = 2.0f * Mathf.Abs(transform.position.z - Camera.main.transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Camera.main.aspect;
            float halfFrustumWidth = frustumWidth / 2;

            if (Mathf.Abs(transform.position.x) - 12f > halfFrustumWidth)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Destination")
            {
                NotifyFightBeganCallback.Invoke();
            }
        }

        public void SetPosition(Vector3 position)
        {
            SetPositionWithOffset(position, Vector3.zero);
        }

        public void SetPositionWithOffset(Vector3 position, Vector3 offset)
        {
            transform.position = position + offset;
        }

        public void Reset()
        {
            velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            animator.State = AnturaAnimationStates.walking;
            animator.SetWalkingSpeed(1f);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
