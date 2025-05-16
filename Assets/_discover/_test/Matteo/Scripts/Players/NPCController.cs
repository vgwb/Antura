using UnityEngine;
using PetanqueGame.Physics;

namespace PetanqueGame.Players
{
    public class NPCController : PlayerController
    {
        [SerializeField] private Transform _throwPoint;
        [SerializeField] private Transform _bouldsPlayedContainer;

        private JackIdentifier _jackIdentifier;
        private Transform _jack;

        public override void StartTurn(System.Action onEndTurn)
        {
            base.StartTurn(onEndTurn);

            if (_jackIdentifier == null)
            {
                _jackIdentifier = FindAnyObjectByType<JackIdentifier>();
                if (_jackIdentifier != null)
                    _jack = _jackIdentifier.transform;
                else
                {
                    Debug.LogWarning("JackIdentifier non trovato nella scena!");
                    return;
                }
            }

            Invoke(nameof(ThrowBall), Random.Range(1f, 2f));
        }

        private void ThrowBall()
        {
            foreach (Transform child in BouldsToPlayHolder)
            {
                if (child.TryGetComponent<BallPhysicsController>(out _))
                {
                    GameObject ball = child.gameObject;
                    ball.transform.SetParent(null);
                    ball.transform.position = _throwPoint.position;
                    ball.transform.rotation = Quaternion.identity;

                    Rigidbody rb = ball.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    Vector3 direction = (_jack.position - _throwPoint.position).normalized;
                    rb.AddForce(direction * Random.Range(4f, 7f), ForceMode.Impulse);

                    ball.transform.SetParent(_bouldsPlayedContainer);
                    break;
                }
            }

            _endTurnCallback?.Invoke();
        }
    }
}
