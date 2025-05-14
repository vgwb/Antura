
using UnityEngine;
using PetanqueGame.Utils;

namespace PetanqueGame.Players
{
    public class NPCController : PlayerController
    {
        [SerializeField] private Transform _throwPoint;
        [SerializeField] private Transform _jack;
        [SerializeField] private ObjectPooler _ballPool;

        public override void StartTurn(System.Action onEndTurn)
        {
            _endTurnCallback = onEndTurn;
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
