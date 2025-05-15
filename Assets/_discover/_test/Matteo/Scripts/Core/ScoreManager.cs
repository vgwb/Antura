using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PetanqueGame.UI;

namespace PetanqueGame.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _balls;
        [SerializeField] private ScoreUI _scoreUI;

        private JackIdentifier _jackIdentifier;

        public void CalculateScores()
        {
            if (_jackIdentifier == null)
            {
                _jackIdentifier = FindAnyObjectByType<JackIdentifier>();
            }

            if (_jackIdentifier == null)
            {
                Debug.LogWarning("Jack non trovato! Assicurati che abbia lo script JackIdentifier.");
                return;
            }

            GameObject jack = _jackIdentifier.gameObject;

            var distances = _balls
                .Where(b => b != null)
                .Select(b => new
                {
                    Ball = b,
                    Distance = Vector3.Distance(jack.transform.position, b.transform.position)
                })
                .OrderBy(d => d.Distance)
                .ToList();

            if (distances.Count == 0)
            {
                Debug.LogWarning("Nessuna palla valida trovata per il calcolo del punteggio.");
                return;
            }

            string leadingTeam = distances[0].Ball.tag;
            int score = distances.TakeWhile(d => d.Ball.tag == leadingTeam).Count();

            _scoreUI?.UpdateScore(leadingTeam, score);
        }
    }
}
