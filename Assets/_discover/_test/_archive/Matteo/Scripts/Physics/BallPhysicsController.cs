using UnityEngine;

namespace PetanqueGame.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallPhysicsController : MonoBehaviour
    {
        private Rigidbody _rb;

        [SerializeField] private bool _isBlueTeam;
        [SerializeField] private bool _isRedTeam;

        public bool IsBlueTeam => _isBlueTeam;
        public bool IsRedTeam => _isRedTeam;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.maxAngularVelocity = 20f;
        }

        private void FixedUpdate()
        {
            //if (_rb.linearVelocity.magnitude < 0.05f)
            //    _rb.linearVelocity = Vector3.zero;
        }
    }
}
