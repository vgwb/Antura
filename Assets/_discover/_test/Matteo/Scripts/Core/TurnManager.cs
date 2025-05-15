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
            MovePlayerToPosition(_players[_currentPlayerIndex], _turnPosition);
            _players[_currentPlayerIndex].StartTurn(OnPlayerTurnEnded);

            if (_currentPlayerIndex == 0)
            {
                CameraManager.I.FocusCamOn(_players[_currentPlayerIndex].transform);
                CameraManager.I.ChangeCameraMode(CameraMode.Focus);
            }
            else
            {
                CameraManager.I.ChangeCameraMode(CameraMode.Player);
            }
        }

        private void OnPlayerTurnEnded()
        {
            MovePlayerToPosition(_players[_currentPlayerIndex], _playerPositions[_currentPlayerIndex]);

            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            StartCurrentPlayerTurn();
        }

        private void MovePlayerToPosition(PlayerController player, Transform targetPosition)
        {
            if (targetPosition != null)
            {
                player.transform.position = targetPosition.position;
                player.transform.rotation = targetPosition.rotation;
            }
            else
            {
                Debug.LogWarning("Target position is not set for a player in TurnManager.");
            }
        }
    }
}
