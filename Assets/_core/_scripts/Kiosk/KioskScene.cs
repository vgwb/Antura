using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Dog;
using Antura.Keeper;
using Antura.Kiosk;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Antura.Scenes
{
    public enum KioskLanguages
    {
        English = 1,
        Italian = 2
    }

    /// <summary>
    /// Controls the _Start scene, providing an entry point for all users prior to having selected a player profile.
    /// </summary>
    public class KioskScene : SceneBase
    {
        public KioskLanguages KioskLanguage = KioskLanguages.English;

        public const string UrlKioskEng = "http://www.antura.org/triennale/";
        public const string UrlKioskIta = "http://www.antura.org/it/triennale_it/";

        [Header("Setup")]
        public AnturaAnimationStates AnturaAnimation = AnturaAnimationStates.sitting;
        public LLAnimationStates LLAnimation = LLAnimationStates.LL_dancing;

        [Header("References")]
        public AnturaAnimationController AnturaAnimController;
        public LivingLetterController LLAnimController;

        public WebPanel WebPanel;
        public TextMeshProUGUI ButtonTextDonate;
        public TextMeshProUGUI ButtonTextPlay;

        public GameObject ButtonItalian;
        public GameObject ButtonEnglish;


        protected override void Start()
        {
            base.Start();
            GlobalUI.ShowPauseMenu(false, PauseMenuType.StartScreen);
            KeeperManager.I.PlayDialogue(AppManager.I.ContentEdition.LearnMethod.TitleLocID, false, true, null, KeeperMode.LearningThenNativeNoSubtitles);

            AnturaAnimController.State = AnturaAnimation;
            LLAnimController.State = LLAnimation;

            LLAnimController.Init(AppManager.I.Teacher.GetRandomTestLetterLL(useMaxJourneyData: true));

            AppManager.I.AppSettings.KioskMode = true;
            AppManager.I.AppSettingsManager.NewSettings.ShareAnalyticsEnabled = true;
            AppManager.I.AppSettings.KeeperSubtitlesEnabled = true;
            updateUI();

            AppManager.I.Services.Analytics.TrackKioskEvent("kiosk_home");
        }

        public void OnBtnPlay()
        {
            AppManager.I.Services.Analytics.TrackKioskEvent("kiosk_play");

            AppManager.I.Player.CurrentJourneyPosition.SetPosition(6, 15, 1);
            var config = new MinigameLaunchConfiguration(0, 1, tutorialEnabled: true, directGame: true);
            AppManager.I.GameLauncher.LaunchGame(MiniGameCode.FastCrowd_buildword, config);
        }

        public void OnBtnDonate()
        {
            if (KioskLanguage == KioskLanguages.Italian)
            {
                WebPanel.Open(UrlKioskIta);
            }
            else
            {
                WebPanel.Open(UrlKioskEng);
            }
            AppManager.I.Services.Analytics.TrackKioskEvent("kiosk_donate");

        }

        public void OnBtnLanguageIta()
        {
            KioskLanguage = KioskLanguages.Italian;
            updateUI();
        }

        public void OnBtnLanguageEng()
        {
            KioskLanguage = KioskLanguages.English;
            updateUI();
        }

        private void updateUI()
        {
            switch (KioskLanguage)
            {
                case KioskLanguages.English:
                    ButtonTextDonate.text = "Help us!\nDonate";
                    ButtonTextPlay.text = "Play Demo";
                    ButtonItalian.transform.DOScale(1.0f, 0.3f);
                    ButtonEnglish.transform.DOScale(1.2f, 0.3f);
                    break;
                case KioskLanguages.Italian:
                    ButtonTextDonate.text = "Aiutaci!\nDonazione";
                    ButtonTextPlay.text = "Gioca Demo";
                    ButtonItalian.transform.DOScale(1.2f, 0.3f);
                    ButtonEnglish.transform.DOScale(1.0f, 0.3f);
                    break;
            }
        }
    }
}
