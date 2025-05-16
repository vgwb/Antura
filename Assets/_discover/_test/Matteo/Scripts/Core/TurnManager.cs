using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PetanqueGame.Players;
using DG.DeInspektor.Attributes;

namespace PetanqueGame.Core
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerController> _players;
        [SerializeField] private List<Transform> _playerPositions;
        [SerializeField] private Transform _turnPosition;
        [SerializeField] private Transform _gameCameraPoint;

        [Header("Jack Throw Settings")]
        [SerializeField] private GameObject _jackPrefab;
        [SerializeField] private Transform _jackThrowPoint;
        [SerializeField] private float _jackCurveHeight = 1.5f;
        [SerializeField] private float _jackTravelTime = 1f;

        private Transform _cameraFocusTarget;
        private int _currentPlayerIndex = 0;
        private bool _gameStarted = false;
        private bool _jackThrown = false;

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void StartGame()
        {
            if (_gameStarted)
                return;

            if (_players.Count != _playerPositions.Count)
            {
                Debug.LogError("Player list and position list must have the same number of elements.");
                return;
            }

            _gameStarted = true;
            _currentPlayerIndex = 0;

            PositionAllPlayers();
            StartCurrentPlayerTurn();
        }

        private void PositionAllPlayers()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                MovePlayerToPosition(_players[i], _playerPositions[i]);
            }
        }

        private void StartCurrentPlayerTurn()
        {
            var currentPlayer = _players[_currentPlayerIndex];
            MovePlayerToPosition(currentPlayer, _turnPosition);

            HandlePlayerTurnStart(currentPlayer);

            // Se è il primo turno del primo player, lancia il jack prima
            if (!_jackThrown && _currentPlayerIndex == 0 && currentPlayer is PlayerHumanController)
            {
                StartCoroutine(ThrowJackThenStartTurn(currentPlayer));
            }
            else
            {
                currentPlayer.StartTurn(OnPlayerTurnEnded);
            }
        }

        private IEnumerator ThrowJackThenStartTurn(PlayerController currentPlayer)
        {
            _jackThrown = true;

            GameObject jack = Instantiate(_jackPrefab, _jackThrowPoint.position, Quaternion.identity);

            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(2f, 4f));
            Vector3 targetPosition = jack.transform.position + randomOffset;

            yield return StartCoroutine(ThrowWithCurve(jack.transform, targetPosition, _jackCurveHeight, _jackTravelTime));

            currentPlayer.StartTurn(OnPlayerTurnEnded);
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
        }

        private void OnPlayerTurnEnded()
        {
            var currentPlayer = _players[_currentPlayerIndex];
            HandlePlayerTurnEnd(currentPlayer);
            MovePlayerToPosition(currentPlayer, _playerPositions[_currentPlayerIndex]);

            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            StartCurrentPlayerTurn();
        }

        private void MovePlayerToPosition(PlayerController player, Transform targetPosition)
        {
            if (targetPosition == null)
                return;

            Quaternion targetRotation = targetPosition.rotation;

            int playerIndex = _players.IndexOf(player);
            if (playerIndex != 0)
            {
                targetRotation *= Quaternion.Euler(0f, 180f, 0f);
            }

            player.transform.SetPositionAndRotation(targetPosition.position, targetRotation);
        }

        private void HandlePlayerTurnStart(PlayerController player)
        {
            if (_currentPlayerIndex == 0)
                StartCoroutine(DisableModelAndCameraNextFrame(player));
        }

        private IEnumerator DisableModelAndCameraNextFrame(PlayerController player)
        {
            yield return null;

            if (player.ScriptToDisable != null)
                player.ScriptToDisable.enabled = false;
        }

        private void HandlePlayerTurnEnd(PlayerController player)
        {
            if (player.ScriptToDisable != null)
                player.ScriptToDisable.enabled = false;
        }
    }
}
