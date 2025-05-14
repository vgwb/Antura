
using UnityEngine;

namespace PetanqueGame.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallPhysicsController : MonoBehaviour
    {
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.maxAngularVelocity = 20f;
        }

        private void FixedUpdate()
        {
            if (_rb.linearVelocity.magnitude < 0.05f)
                _rb.linearVelocity = Vector3.zero;
        }
    }
}
