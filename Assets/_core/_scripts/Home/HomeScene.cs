using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Dog;
using Antura.Keeper;
using Antura.Language;
using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;
using UnityEngine.Serialization;

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
        public EditionSelectionManager SelectionManager;
        public AnturaPetSwitcher PetSwitcher;
        public LivingLetterController LLAnimController;
        public GameObject DialogReservedArea;
        public GameObject ProfileSelectorUI;
        public GameObject PanelAppUpdate;
        public GameObject HomeLogo;

        protected override void Start()
        {
            base.Start();

            if (EditionSelectionManager.MustChooseContentEditions)
            {
                // First choose a learning edition
                PetSwitcher.gameObject.SetActive(false);
                LLAnimController.gameObject.SetActive(false);
                HomeLogo.SetActive(false);
                GlobalUI.ShowPauseMenu(false, PauseMenuType.StartScreen);
            }
            else
            {
                GlobalUI.ShowPauseMenu(true, PauseMenuType.StartScreen);
                KeeperManager.I.PlayDialogue(AppManager.I.ContentEdition.LearnMethod.TitleLocID, false, true, TutorCreateProfile, KeeperMode.LearningNoSubtitles);
                PetSwitcher.AnimController.State = AnturaAnimation;
                LLAnimController.State = LLAnimation;
            }
        }

        void TutorCreateProfile()
        {
            if (!EditionSelectionManager.MustChooseContentEditions && EditionSelectionManager.HasSelectedEdition && AppManager.I.PlayerProfileManager.GetPlayersIconData().Count < 1)
            {
                KeeperManager.I.PlayDialogue(LocalizationDataId.Action_Createprofile);
            }
        }

        /// <summary>
        /// Start the game using the currently selected player.
        /// </summary>
        public void Play()
        {
            if (AppManager.PROFILE_INVERSION)
            {
                // We need to choose the content instead, then we will resume the play session
                SelectionManager.ContentEditionSelection(true);
            }
            else
            {
                ResumeCurrentPlaySession();
            }
        }

        public static void ResumeCurrentPlaySession()
        {
            if (AppManager.PROFILE_INVERSION)
            {
                // We must load the play session data, or create it
                Debug.Log("Entering game with Player: " + AppManager.I.Player.Uuid);
                var playerAppVersion = AppManager.I.Player.AppVersion;
                var majorVersion = int.Parse(playerAppVersion.Split('.').First());
                if (majorVersion < 4)   // Older player profile, must be moved to the new separate content
                {
                    Debug.LogWarning("Player is OLD, must upgrade");
                    var contentID = AppManager.I.AppSettingsManager.Settings.ContentID;
                    Debug.LogWarning("ContentID selected is " + contentID);
                    // TODO: generate new content, and save it
                    AppManager.I.NavigationManager.GenerateContentData(AppManager.I.Player);
                }
                else
                {
                    Debug.Log("Player is new, let's get the content too");
                    // TODO: load the content
                }

                Debug.Log($"Content ID is: {AppManager.I.PlayerProfileManager.CurrentPlayer.ContentID}");
            }

            // Debug.Log("Play with Player: " + AppManager.I.Player);
            GlobalUI.ShowPauseMenu(true);
            AppManager.I.StartNewPlaySession();
            AppManager.I.NavigationManager.GoToNextScene();
        }

        #region Reserved Area

        private bool reservedAreaIsOpen = false;

        public void OnBtnReservedArea()
        {
            if (reservedAreaIsOpen)
            {
                CloseReservedAreaPanel();
            }
            else
            {
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

    }
}
