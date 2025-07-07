using DG.DeInspektor.Attributes;
using PetanqueGame.Physics;
using PetanqueGame.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        [Header("Round Settings")]
        [SerializeField] private int _bouleToThrow = 3;
        [SerializeField] private float _delayBeforeJackThrow = 2f;
        [SerializeField] private float _delayBeforeScoreCalculation = 2f;
        [SerializeField] private float _delayBeforeNextRound = 3f;

        [SerializeField] private ScoreManager _scoreManager;

        private HashSet<Transform> _expectedBoules = new();
        private HashSet<Transform> _boulesThrown = new();

        private Dictionary<Transform, Vector3> _initialPositions = new();
        private Dictionary<Transform, Quaternion> _initialRotations = new();
        private Dictionary<Transform, Transform> _initialParents = new();

        private Dictionary<PlayerController, int> _throwsPerPlayer = new();
        private int _currentPlayerIndex = 0;
        private bool _gameStarted = false;
        private bool _roundInProgress = false;

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
            _roundInProgress = false;

            foreach (var player in _players)
                _throwsPerPlayer[player] = 0;

            ThrowHelper.OnBouleThrown += OnBouleThrown;
            _scoreManager.OnGameOver += HandleGameOver;
            _scoreManager.OnRoundEnd += ResetRound;

            PositionAllPlayers();

            _expectedBoules.Clear();
            _boulesThrown.Clear();
            _initialPositions.Clear();
            _initialRotations.Clear();
            _initialParents.Clear();

            foreach (var player in _players)
            {
                foreach (var boule in player.Boules)
                {
                    _expectedBoules.Add(boule);

                    _initialPositions[boule] = boule.position;
                    _initialRotations[boule] = boule.rotation;
                    _initialParents[boule] = boule.parent;
                }
            }

            StartCoroutine(StartRoundWithJack());
        }

        private void OnDestroy()
        {
            ThrowHelper.OnBouleThrown -= OnBouleThrown;
            if (_scoreManager != null)
            {
                _scoreManager.OnGameOver -= HandleGameOver;
                _scoreManager.OnRoundEnd -= ResetRound;
            }
        }

        private IEnumerator StartRoundWithJack()
        {
            // Porta il primo giocatore a TurnPosition prima di lanciare il jack
            MovePlayerToPosition(_players[_currentPlayerIndex], _turnPosition);

            yield return new WaitForSeconds(_delayBeforeJackThrow);

            _roundInProgress = true;

            var currentPlayer = _players[_currentPlayerIndex];
            yield return ThrowJackThenStartTurn(currentPlayer);
        }

        private IEnumerator ThrowJackThenStartTurn(PlayerController currentPlayer)
        {
            GameObject jack = Instantiate(_jackPrefab, _jackThrowPoint.position, Quaternion.identity);
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(2f, 4f));
            Vector3 targetPosition = jack.transform.position + randomOffset;

            yield return StartCoroutine(ThrowHelper.ThrowWithCurve(jack.transform, targetPosition, _jackCurveHeight, _jackTravelTime, null));

            // Dopo il lancio del jack, fai partire il primo turno
            StartCurrentPlayerTurn();
        }

        private void OnBouleThrown(Transform boule)
        {
            if (!_roundInProgress)
                return;

            if (_expectedBoules.Contains(boule) && !_boulesThrown.Contains(boule))
            {
                _boulesThrown.Add(boule);

                PlayerController currentPlayer = _players[_currentPlayerIndex];
                _throwsPerPlayer[currentPlayer]++;

                // Dopo il lancio, sposta il giocatore alla sua posizione base
                MovePlayerToPosition(currentPlayer, _playerPositions[_currentPlayerIndex]);

                // Passa al prossimo giocatore
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

                // Controlla se tutti i giocatori hanno finito i lanci
                bool allDone = _players.All(p => _throwsPerPlayer[p] >= _bouleToThrow);

                if (allDone)
                {
                    _roundInProgress = false;
                    StartCoroutine(EndRoundSequence());
                }
                else
                {
                    // Sposta il prossimo giocatore a TurnPosition e avvia il suo turno
                    MovePlayerToPosition(_players[_currentPlayerIndex], _turnPosition);
                    StartCurrentPlayerTurn();
                }
            }
        }

        private void StartCurrentPlayerTurn()
        {
            var currentPlayer = _players[_currentPlayerIndex];
            currentPlayer.StartTurn(OnPlayerTurnEnded);
        }

        private void OnPlayerTurnEnded()
        {
            // Ora non serve perché il cambio turno è gestito da OnBouleThrown
        }

        private IEnumerator EndRoundSequence()
        {
            // Riporta i giocatori alle posizioni base
            for (int i = 0; i < _players.Count; i++)
            {
                MovePlayerToPosition(_players[i], _playerPositions[i]);
            }

            yield return new WaitForSeconds(_delayBeforeScoreCalculation);
            _scoreManager?.CalculateScores();

            yield return new WaitForSeconds(_delayBeforeNextRound);

            ResetForNextRound();
        }

        private void ResetForNextRound()
        {
            foreach (var player in _players)
                _throwsPerPlayer[player] = 0;

            _boulesThrown.Clear();
            _currentPlayerIndex = 0;

            // Rimuovi jack corrente
            var jack = FindAnyObjectByType<JackIdentifier>();
            if (jack != null)
                Destroy(jack.gameObject);

            // Reset boule posizioni e fisica
            foreach (var boule in _expectedBoules)
            {
                if (boule == null)
                    continue;

                Rigidbody rb = boule.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                boule.SetParent(_initialParents[boule]);
                boule.position = _initialPositions[boule];
                boule.rotation = _initialRotations[boule];

                //rb.isKinematic = false;
            }

            StartCoroutine(StartRoundWithJack());
        }

        private void PositionAllPlayers()
        {
            for (int i = 0; i < _players.Count; i++)
                MovePlayerToPosition(_players[i], _playerPositions[i]);
        }

        private void MovePlayerToPosition(PlayerController player, Transform targetPosition)
        {
            if (targetPosition == null)
                return;

            Quaternion targetRotation = targetPosition.rotation;
            int playerIndex = _players.IndexOf(player);
            if (playerIndex != 0)
                targetRotation *= Quaternion.Euler(0f, 180f, 0f);

            player.transform.SetPositionAndRotation(targetPosition.position, targetRotation);
        }

        private void HandleGameOver()
        {
            Debug.Log("Game Over!");

            if (_scoreManager.TeamRedWin)
                Debug.Log("TEAM RED WINS!");
            else if (_scoreManager.TeamBlueWin)
                Debug.Log("TEAM BLUE WINS!");

            _roundInProgress = false;
            _gameStarted = false;

            // Riporta i giocatori alle posizioni iniziali
            PositionAllPlayers();
            StopAllCoroutines();
        }

        private void ResetRound()
        {
            // Puoi lasciare vuoto o rimuoverlo se non usato
        }
    }
}
