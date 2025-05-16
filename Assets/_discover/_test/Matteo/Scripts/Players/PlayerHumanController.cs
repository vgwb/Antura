using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using PetanqueGame.Physics;

namespace PetanqueGame.Players
{
    public class PlayerHumanController : PlayerController
    {
        [SerializeField] private Transform _throwPoint;
        [SerializeField] private Transform _bouldsPlayedContainer;
        [SerializeField] private float _throwStrength = 0.02f;
        [SerializeField] private float _curveHeight = 2f;
        [SerializeField] private float _travelTime = 1.0f;

        private GameObject _currentBall;
        private Vector2 _startTouch, _endTouch;
        private bool _isDragging;
        private bool _usingMouse;

        public override void StartTurn(System.Action onEndTurn)
        {
            base.StartTurn(onEndTurn);
            TakeTheBall();
        }

        private void TakeTheBall()
        {
            foreach (Transform child in BouldsToPlayHolder)
            {
                if (child.TryGetComponent<BallPhysicsController>(out _))
                {
                    _currentBall = child.gameObject;
                    _currentBall.transform.SetParent(null);
                    _currentBall.transform.position = _throwPoint.position;
                    _currentBall.transform.rotation = Quaternion.identity;
                    break;
                }
            }

            if (_currentBall == null)
            {
                Debug.LogWarning("Nessuna palla disponibile nel bouldsToPlayHolder!");
            }
        }

        private void Update()
        {
            if (_currentBall == null)
                return;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (touch.phase == TouchPhase.Began)
                {
                    _startTouch = touch.position;
                    _isDragging = true;
                    _usingMouse = false;
                }
                else if (touch.phase == TouchPhase.Ended && _isDragging && !_usingMouse)
                {
                    _endTouch = touch.position;
                    HandleSwipe();
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                _startTouch = Input.mousePosition;
                _isDragging = true;
                _usingMouse = true;
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging && _usingMouse)
            {
                _endTouch = Input.mousePosition;
                HandleSwipe();
            }
        }

        private void HandleSwipe()
        {
            Vector2 swipe = (_endTouch - _startTouch) * _throwStrength;
            Vector3 targetOffset = new Vector3(swipe.x, 0, swipe.y);
            Vector3 targetPosition = _currentBall.transform.position + targetOffset;

            StartCoroutine(ThrowWithCurve(_currentBall.transform, targetPosition, _curveHeight, _travelTime));
            _isDragging = false;
            _usingMouse = false;
        }

        private IEnumerator ThrowWithCurve(Transform obj, Vector3 target, float height, float duration)
        {
            Vector3 startPos = obj.position;
            float elapsed = 0f;
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float curvedY = Mathf.Sin(Mathf.PI * t) * height;
                obj.position = Vector3.Lerp(startPos, target, t) + Vector3.up * curvedY;
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.position = target;
            rb.isKinematic = false;

            obj.SetParent(_bouldsPlayedContainer);
            _currentBall = null;
            _endTurnCallback?.Invoke();
        }
    }
}
