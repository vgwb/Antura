using TMPro;
using UnityEngine;
using System.Collections;

namespace PetanqueGame.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private float displayDuration = 2f;

        private Coroutine _hideRoutine;

        public void UpdateScore(int redScore, int blueScore)
        {
            _scoreText.text = $"Red Team: {redScore} - Blue Team: {blueScore}";
            ShowTemporarily();
        }

        private void ShowTemporarily()
        {
            _scoreText.gameObject.SetActive(true);

            if (_hideRoutine != null)
                StopCoroutine(_hideRoutine);

            _hideRoutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(displayDuration);
            _scoreText.gameObject.SetActive(false);
        }
    }
}
