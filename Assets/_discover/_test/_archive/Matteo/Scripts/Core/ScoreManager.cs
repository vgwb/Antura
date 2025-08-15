using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PetanqueGame.UI;
using PetanqueGame.Physics;
using System;

namespace PetanqueGame.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private ScoreUI _scoreUI;

        [Header("Winning Settings")]
        [SerializeField] private int _winningScore = 5;
        [SerializeField] private GameObject _winPrefab;
        [SerializeField] private Transform _winSpawnPoint;

        private JackIdentifier _jackIdentifier;
        private int _redTotalScore = 0;
        private int _blueTotalScore = 0;

        public bool TeamRedWin { get; private set; } = false;
        public bool TeamBlueWin { get; private set; } = false;

        public System.Action OnGameOver;
        public System.Action OnRoundEnd;

        private void Start()
        {
            //_scoreUI?.UpdateScore(_redTotalScore, _blueTotalScore);
        }

        public void CalculateScores()
        {
            if (_jackIdentifier == null)
                _jackIdentifier = FindAnyObjectByType<JackIdentifier>();

            if (_jackIdentifier == null)
            {
                Debug.LogWarning("Jack non trovato! Assicurati che abbia lo script JackIdentifier.");
                return;
            }

            GameObject jack = _jackIdentifier.gameObject;
            List<(GameObject ball, BallPhysicsController controller)> balls = new();

            foreach (Transform child in transform)
            {
                BallPhysicsController controller = child.GetComponent<BallPhysicsController>();
                if (controller != null && (controller.IsBlueTeam || controller.IsRedTeam))
                {
                    balls.Add((child.gameObject, controller));
                }
            }

            if (balls.Count == 0)
            {
                Debug.LogWarning("Nessuna palla valida trovata per il calcolo del punteggio.");
                return;
            }

            var distances = balls
                .Select(b => new
                {
                    Ball = b.ball,
                    Controller = b.controller,
                    Distance = Vector3.SqrMagnitude(jack.transform.position - b.ball.transform.position)
                })
                .OrderBy(d => d.Distance)
                .ToList();

            var leadingController = distances[0].Controller;
            bool isRed = leadingController.IsRedTeam;
            int score = distances.TakeWhile(d =>
                d.Controller.IsRedTeam == isRed &&
                d.Controller.IsBlueTeam == !isRed
            ).Count();

            int redScore = isRed ? score : 0;
            int blueScore = !isRed ? score : 0;

            _redTotalScore += redScore;
            _blueTotalScore += blueScore;

            _scoreUI?.UpdateScore(_redTotalScore, _blueTotalScore);

            if (_redTotalScore >= _winningScore)
            {
                TeamRedWin = true;
                OnGameOver?.Invoke();
            }
            else if (_blueTotalScore >= _winningScore)
            {
                TeamBlueWin = true;

                if (_winPrefab != null && _winSpawnPoint != null)
                {
                    Instantiate(_winPrefab, _winSpawnPoint.position, Quaternion.identity);
                }

                OnGameOver?.Invoke();
            }
            else
            {
                OnRoundEnd?.Invoke();
            }
        }
    }
}
