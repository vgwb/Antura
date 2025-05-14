using System.Collections.Generic;
using UnityEngine;
using PetanqueGame.Players;

namespace PetanqueGame.Core
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerController> _players;

        private int _currentPlayerIndex = 0;
        private bool _gameStarted = false;

        public void StartGame()
        {
            if (_gameStarted)
                return;

            _gameStarted = true;
            _currentPlayerIndex = 0;
            StartCurrentPlayerTurn();
        }

        private void StartCurrentPlayerTurn()
        {
            _players[_currentPlayerIndex].StartTurn(OnPlayerTurnEnded);
        }

        private void OnPlayerTurnEnded()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            StartCurrentPlayerTurn();
        }
    }
}
