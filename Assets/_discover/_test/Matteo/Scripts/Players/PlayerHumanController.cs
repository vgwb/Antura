
using UnityEngine;
using UnityEngine.EventSystems;
using PetanqueGame.Utils;

namespace PetanqueGame.Players
{
    public class PlayerHumanController : PlayerController
    {
        [SerializeField] private ObjectPooler _ballPool;
        [SerializeField] private Transform _throwPoint;

        private GameObject _currentBall;
        private Vector2 _startTouch, _endTouch;
        private bool _isDragging;

        public override void StartTurn(System.Action onEndTurn)
        {
            _endTurnCallback = onEndTurn;
            SpawnBall();
        }

        private void SpawnBall()
        {
            _currentBall = _ballPool.GetFromPool(_throwPoint.position, Quaternion.identity);
        }

        private void Update()
        {
            if (_currentBall == null || Input.touchCount == 0) return;

            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

            if (touch.phase == TouchPhase.Began)
            {
                _startTouch = touch.position;
                _isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended && _isDragging)
            {
                _endTouch = touch.position;
                Vector2 drag = _startTouch - _endTouch;
                Vector3 force = new Vector3(drag.x, drag.y * 1.5f, drag.y) * 0.02f;

                Rigidbody rb = _currentBall.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(force, ForceMode.Impulse);
                _currentBall = null;
                _isDragging = false;
                _endTurnCallback?.Invoke();
            }
        }
    }
}
