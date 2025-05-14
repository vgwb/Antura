
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PetanqueGame.UI;

namespace PetanqueGame.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private GameObject _jack;
        [SerializeField] private List<GameObject> _balls;
        [SerializeField] private ScoreUI _scoreUI;

        public void CalculateScores()
        {
            var distances = _balls
                .Where(b => b != null)
                .Select(b => new { Ball = b, Distance = Vector3.Distance(_jack.transform.position, b.transform.position) })
                .OrderBy(d => d.Distance)
                .ToList();

            if (distances.Count == 0) return;

            string leadingTeam = distances[0].Ball.tag;
            int score = distances.TakeWhile(d => d.Ball.tag == leadingTeam).Count();

            _scoreUI?.UpdateScore(leadingTeam, score);
        }
    }
}
