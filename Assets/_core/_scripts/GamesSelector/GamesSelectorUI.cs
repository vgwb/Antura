using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Keeper;
using Antura.Language;
using Antura.Rewards;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using DG.DeExtensions;

namespace Antura.GamesSelector
{
    /// <summary>
    /// User interface of the GamesSelector.
    /// </summary>
    public class GamesSelectorUI : MonoBehaviour
    {
        public GameObject[] Stars;
        public TextRender TitleCode;
        public TextRender TitleNativeLang;
        public TextRender TitleLearningLang;

        void Start()
        {
            // Fill with data
            var journeyPos = AppManager.I.Player.CurrentJourneyPosition;
            var playSession = AppManager.I.DB.GetPlaySessionDataById(journeyPos.Id);
            var learningBlock = AppManager.I.DB.GetLearningBlockDataById($"{playSession.Stage}.{playSession.LearningBlock}");
            TitleCode.text = journeyPos.GetShortTitle();
            TitleLearningLang.SetText(learningBlock.Title_LearningLang, LanguageUse.Learning);

            if (AppManager.I.ContentEdition.LearnMethod.ShowHelpText)
            {
                TitleNativeLang.SetText(learningBlock.Title_NativeLang, LanguageUse.Help);
            }
            else
            {
                TitleNativeLang.text = "";
            }

            // play the tutorial only if in LB 1.1
            PlayTutorialAudio();

            if (!journeyPos.IsMinor(AppManager.I.Player.MaxJourneyPosition))
            {
                // First time playing this session: 0 stars
                SetStars(0);
            }
            else
            {
                int score = (int)AppManager.I.ScoreHelper.GetCurrentScoreForJourneyPosition(AppManager.I.Player.CurrentJourneyPosition);
                SetStars(score);
            }
        }

        void PlayTutorialAudio()
        {
            AudioManager.I.ClearCallbacks();
            KeeperManager.I.PlayDialogue(
                new[]
                {
                    LocalizationDataId.SelectGame_Tuto_1,
                    LocalizationDataId.SelectGame_Tuto_2,
                    LocalizationDataId.SelectGame_Tuto_3,
                }.GetRandom()
            );
        }

        void SetStars(int _tot)
        {
            if (_tot > 3)
            {
                _tot = 3;
            }
            for (int i = 0; i < Stars.Length; ++i)
            {
                GameObject star = Stars[i];
                star.SetActive(i < _tot);
                star.transform.parent.GetComponent<Image>().SetAlpha(i < _tot ? 1f : 0.3f);
            }
        }
    }
}
