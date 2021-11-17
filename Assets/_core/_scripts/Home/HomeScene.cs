using System.Collections;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Dog;
using Antura.Keeper;
using Antura.Language;
using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;

namespace Antura.Scenes
{
    /// <summary>
    /// Controls the _Start scene, providing an entry point for all users prior to having selected a player profile.
    /// </summary>
    public class HomeScene : SceneBase
    {
        [Header("Setup")]
        public AnturaAnimationStates AnturaAnimation = AnturaAnimationStates.sitting;

        public LLAnimationStates LLAnimation = LLAnimationStates.LL_dancing;

        [Header("References")]
        public AnturaAnimationController AnturaAnimController;

        public LivingLetterController LLAnimController;
        public GameObject DialogReservedArea;
        public GameObject ProfileSelectorUI;

        public GameObject PanelAppUpdate;

        public GameObject HomeLogo;

        public static bool HasSelectedLearningEdition;
        public static bool MustChooseLearningEdition => !HasSelectedLearningEdition && AppManager.I.AppEdition.HasMultipleLearningEditions && AppManager.I.Player == null;

        protected override void Start()
        {
            base.Start();

            if (MustChooseLearningEdition)
            {
                // First choose a learning edition
                AnturaAnimController.gameObject.SetActive(false);
                LLAnimController.gameObject.SetActive(false);
                HomeLogo.SetActive(false);
                GlobalUI.ShowPauseMenu(false, PauseMenuType.StartScreen);
            }
            else
            {
                GlobalUI.ShowPauseMenu(true, PauseMenuType.StartScreen);
                KeeperManager.I.PlayDialogue(LocalizationDataId.Game_Title_2, false, true, TutorCreateProfile, KeeperMode.LearningThenNativeNoSubtitles);
                AnturaAnimController.State = AnturaAnimation;
                LLAnimController.State = LLAnimation;
            }
        }

        void TutorCreateProfile()
        {
            if (!MustChooseLearningEdition && AppManager.I.PlayerProfileManager.GetPlayersIconData().Count < 1) {
                KeeperManager.I.PlayDialogue(LocalizationDataId.Action_Createprofile);
            }
        }

        /// <summary>
        /// Start the game using the currently selected player.
        /// </summary>
        public void Play()
        {
            // Debug.Log("Play with Player: " + AppManager.I.Player);
            GlobalUI.ShowPauseMenu(true);

            AppManager.I.StartNewPlaySession();
            AppManager.I.NavigationManager.GoToNextScene();
        }

        #region Reserved Area

        private bool reservedAreaIsOpen = false;

        public void OnBtnReservedArea()
        {
            if (reservedAreaIsOpen) {
                CloseReservedAreaPanel();
            } else {
                OpenReservedAreaPanel();
            }
        }

        public void OnBtnQuit()
        {
            AppManager.I.QuitApplication();
        }

        public void OpenReservedAreaPanel()
        {
            AudioManager.I.PlaySound(Sfx.UIButtonClick);
            DialogReservedArea.SetActive(true);
            ProfileSelectorUI.SetActive(false);
            GlobalUI.ShowPauseMenu(false);
            reservedAreaIsOpen = true;
        }

        public void CloseReservedAreaPanel()
        {
            AudioManager.I.PlaySound(Sfx.UIButtonClick);
            DialogReservedArea.SetActive(false);
            ProfileSelectorUI.SetActive(true);
            GlobalUI.ShowPauseMenu(true, PauseMenuType.StartScreen);
            reservedAreaIsOpen = false;
        }
        #endregion

        #region Language

        public void OnBtnSwitchLanguage()
        {
            ChangeLearningLanguage(AppManager.I.LearningEdition.NativeLanguage == LanguageCode.spanish
                ? LanguageCode.italian
                : LanguageCode.spanish);
        }

        public void ChangeLearningLanguage(LanguageCode langCode)
        {
            StartCoroutine(ChangeLearningLanguageCO(langCode));
        }
        private IEnumerator ChangeLearningLanguageCO(LanguageCode langCode)
        {
            yield return AppManager.I.ResetLanguageSetup(langCode);
            AppManager.I.NavigationManager.GoToHome(true);
        }

        #endregion
    }
}