using Antura.Core;
using TMPro;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// Gives feedback on the state of a play session.
    /// </summary>
    public class PlaySessionStateFeedback : MonoBehaviour
    {
        private JourneyPosition journeyPosition;
        private PlaySessionState playSessionState;

        [Header("Visuals")]
        public GameObject starsPivotGO;
        public PlaySessionStateStar[] stars;
        public TextMeshPro journeyPosTextUI;
        public GameObject surpriseGO;

        public void Initialise(JourneyPosition _journeyPosition, PlaySessionState _playSessionState)
        {
            journeyPosition = _journeyPosition;
            playSessionState = _playSessionState;

            HandleJourneyPosition(journeyPosition);
            HandlePlaySessionState(playSessionState);

            // @note: we do not show the JourneyPos text now
            journeyPosTextUI.gameObject.SetActive(false);

            if (journeyPosition.IsAssessment()
                || journeyPosition.IsEndGame()) {
                starsPivotGO.SetActive(false);
            }
        }

        private void HandlePlaySessionState(PlaySessionState playSessionState)
        {
            int score = 0;
            if (playSessionState != null && playSessionState.scoreData != null)
                score = (int)playSessionState.scoreData.GetScore();

            if (journeyPosition.IsAssessment())
            {
                //Debug.Log( playSessionState.psData.Id + " SCORE: " + score);
                // Show surprise if this is a not yet completed assessment

                var hasRewardsInSession = score == 0;//&& AppManager.I.RewardSystemManager.GetOrGenerateAllRewardPacksForJourneyPosition(journeyPosition) != null;
                surpriseGO.SetActive(hasRewardsInSession);
            }
            else
            {
                surpriseGO.SetActive(false);

                for (int i = 0; i < stars.Length; i++) {
                    stars[i].SetObtained(score >= i + 1);
                }
            }
        }

        private void HandleJourneyPosition(JourneyPosition journeyPosition)
        {
            journeyPosTextUI.text = journeyPosition.ToString();
        }

        #region Show / Hide

        public void Highlight(bool choice)
        {
            if (choice) {
                ShowHighlightedInfo();
            } else {
                ShowUnhighlightedInfo();
            }
        }

        public void HideAllInfo()
        {
            //journeyPosTextUI.gameObject.SetActive(false);
            foreach (var playSessionStateStar in stars) {
                playSessionStateStar.gameObject.SetActive(false);
            }
        }

        public void ShowHighlightedInfo()
        {
            //journeyPosTextUI.gameObject.SetActive(false);
            foreach (var playSessionStateStar in stars) {
                playSessionStateStar.gameObject.SetActive(true);
            }
        }

        public void ShowUnhighlightedInfo()
        {
            //journeyPosTextUI.gameObject.SetActive(true);
            foreach (var playSessionStateStar in stars) {
                playSessionStateStar.gameObject.SetActive(true);
            }
        }

        #endregion
    }
}
