using System.Collections.Generic;
using UnityEngine;
using PetanqueGame.Players;
using DG.DeInspektor.Attributes;
using Antura.Minigames.DiscoverCountry;

namespace PetanqueGame.Core
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerController> _players;
        [SerializeField] private List<Transform> _playerPositions;
        [SerializeField] private Transform _turnPosition;
        [SerializeField] private FocusCamera _focusCamera;

        private int _currentPlayerIndex = 0;
        private bool _gameStarted = false;

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

            if (_focusCamera != null)
                _focusCamera.SetTarget(currentPlayer.transform);

            HandlePlayerTurnStart(currentPlayer);
            currentPlayer.StartTurn(OnPlayerTurnEnded);
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

            player.transform.SetPositionAndRotation(targetPosition.position, targetPosition.rotation);
        }

        private void HandlePlayerTurnStart(PlayerController player)
        {
            if (_currentPlayerIndex == 0)
                StartCoroutine(DisableModelAndCameraNextFrame(player));
        }

        private System.Collections.IEnumerator DisableModelAndCameraNextFrame(PlayerController player)
        {
            yield return null;

            if (player.Model != null)
                player.Model.SetActive(false);

            //if (player.ScriptToDisable != null)
            //    player.ScriptToDisable.enabled = false;
        }

        private void HandlePlayerTurnEnd(PlayerController player)
        {
            if (player.Model != null)
                player.Model.SetActive(false);

            if (player.ScriptToDisable != null)
                player.ScriptToDisable.enabled = false;
        }

    }
}
