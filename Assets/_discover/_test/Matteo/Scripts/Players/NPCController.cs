using UnityEngine;
using PetanqueGame.Utils;

namespace PetanqueGame.Players
{
    public class NPCController : PlayerController
    {
        [SerializeField] private Transform _throwPoint;
        [SerializeField] private ObjectPooler _ballPool;

        private JackIdentifier _jackIdentifier;
        private Transform _jack;

        public override void StartTurn(System.Action onEndTurn)
        {
            _endTurnCallback = onEndTurn;

            // Cerca JackIdentifier se non � gi� trovato
            if (_jackIdentifier == null)
            {
                _jackIdentifier = FindAnyObjectByType<JackIdentifier>();

                if (_jackIdentifier != null)
                {
                    _jack = _jackIdentifier.transform;
                }
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
            Vector3 dir = (_jack.position - _throwPoint.position).normalized;
            GameObject ball = _ballPool.GetFromPool(_throwPoint.position, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(dir * Random.Range(4f, 7f), ForceMode.Impulse);
            _endTurnCallback?.Invoke();
        }
    }
}
