using PetanqueGame.Physics;
using UnityEngine;

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
            foreach (Transform boule in BouldsToPlayHolder)
            {
                if (boule.TryGetComponent<BallPhysicsController>(out _))
                {
                    GameObject ball = boule.gameObject;
                    ball.transform.SetParent(null);
                    ball.transform.position = _throwPoint.position;
                    ball.transform.rotation = Quaternion.identity;

                    Vector3 jackPos = _jack.position;
                    jackPos.y = _throwPoint.position.y;

                    Vector3 randomOffset = Random.insideUnitSphere * 2f;
                    if (randomOffset.y < 0)
                        randomOffset.y = 0f;

                    Vector3 targetPos = jackPos + randomOffset;

                    StartCoroutine(ThrowHelper.ThrowWithCurve(
                        ball.transform,
                        targetPos,
                        height: 2f,
                        duration: 1f,
                        parentAfterThrow: _bouldsPlayedContainer,
                        onComplete: () => _endTurnCallback?.Invoke()
                    ));

                    break;
                }
            }
        }

    }
}
