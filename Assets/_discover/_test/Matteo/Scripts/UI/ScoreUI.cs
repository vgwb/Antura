using TMPro;
using UnityEngine;

namespace PetanqueGame.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void UpdateScore(int redScore, int blueScore)
        {
            _scoreText.text = $"Team Rosso: {redScore} - Team Blu: {blueScore}";
        }
    }
}
