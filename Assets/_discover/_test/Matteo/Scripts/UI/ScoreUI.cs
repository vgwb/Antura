
using TMPro;
using UnityEngine;

namespace PetanqueGame.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void UpdateScore(string team, int score)
        {
            _scoreText.text = $"Punti {team}: {score}";
        }
    }
}
