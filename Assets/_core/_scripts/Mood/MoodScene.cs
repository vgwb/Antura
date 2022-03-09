using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using Antura.UI;
using UnityEngine;

namespace Antura.Scenes
{
    /// <summary>
    /// Manager for the Mood scene.
    /// </summary>
    public class MoodScene : SceneBase
    {
        protected override void Start()
        {
            base.Start();
            GlobalUI.ShowPauseMenu(false);

            Invoke("PlayFeedback", 0.2f);
        }

        void PlayFeedback()
        {
            int rnd = Random.Range(1, 3);
            switch (rnd)
            {
                case 1:
                    KeeperManager.I.PlayDialogue(LocalizationDataId.Mood_Question_1);
                    break;
                case 3:
                    KeeperManager.I.PlayDialogue(LocalizationDataId.Mood_Question_3);
                    break;
                default:
                    KeeperManager.I.PlayDialogue(LocalizationDataId.Mood_Question_2);
                    break;
            }
        }

        private bool selectedAlready = false;

        /// <summary>
        /// Mood selected. [1,5]
        /// </summary>
        /// <param name="_mood"></param>
        public void MoodSelected(int _mood)
        {
            if (selectedAlready)
                return;
            selectedAlready = true;

            LogManager.I.LogMood(_mood);
            AudioManager.I.PlaySound(Sfx.UIButtonClick);

            LocalizationDataId locID = LocalizationDataId.Mood_Answer_SoSo;
            switch (_mood)
            {
                case 1:
                    locID = LocalizationDataId.Mood_Answer_VerySad;
                    break;
                case 2:
                    locID = LocalizationDataId.Mood_Answer_Sad;
                    break;
                case 3:
                    locID = LocalizationDataId.Mood_Answer_SoSo;
                    break;
                case 4:
                    locID = LocalizationDataId.Mood_Answer_Happy;
                    break;
                case 5:
                    locID = LocalizationDataId.Mood_Answer_VeryHappy;
                    break;
            }
            KeeperManager.I.PlayDialogue(locID, _callback: exitScene);
        }

        void exitScene()
        {
            AppManager.I.Player.Save();
            AppManager.I.NavigationManager.GoToNextScene();
        }
    }
}
