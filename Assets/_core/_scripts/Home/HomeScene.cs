using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Dog;
using Antura.Keeper;
using Antura.Language;
using Antura.LivingLetters;
using Antura.Profile;
using Antura.UI;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

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
        public TextMeshProUGUI ClassroomName;

        protected override void Start()
        {
            base.Start();

            if (AppManager.PROFILE_INVERSION && AppManager.I.NavigationManager.NavData.CurrentContent != null)
            {
                if (AppManager.VERBOSE_INVERSION)
                    Debug.LogError("[INVERSION] Set Current Content to NULL as we are in Home");
                AppManager.I.NavigationManager.NavData.CurrentContent = null;
            }

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
                // Fix: [Home] a wrong audio is played #508
                //KeeperManager.I.PlayDialogue(AppManager.I.ContentEdition.LearnMethod.TitleLocID, false, true, TutorCreateProfile, KeeperMode.LearningNoSubtitles);
                PetSwitcher.AnimController.State = AnturaAnimation;
                LLAnimController.State = LLAnimation;
            }

            updateClassroomName();
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
                if (AppManager.VERBOSE_INVERSION)
                    Debug.LogError("[Inversion] Entering game with Player: " + AppManager.I.Player.Uuid);
                /* @note: Old profiles are ignored for now, always use the new logic instead!
                var playerAppVersion = AppManager.I.Player.AppVersion;
                var majorVersion = int.Parse(playerAppVersion.Split('.').First());
                if (majorVersion < 4)   // Older player profile, must be moved to the new separate content
                {
                    if (AppManager.VERBOSE_INVERSION) Debug.LogError("[Inversion] Player is OLD, must upgrade");
                    var contentID = AppManager.I.AppSettingsManager.Settings.ContentID;
                    if (AppManager.VERBOSE_INVERSION) Debug.LogError("[Inversion] ContentID selected is " + contentID);
                    // TODO: generate new content, and save it
                    AppManager.I.NavigationManager.GenerateContentData(AppManager.I.Player);
                }
                else*/
                {
                    if (AppManager.VERBOSE_INVERSION)
                        Debug.LogError("[Inversion] Player is new, let's get the content too");

                    var contentID = AppManager.I.AppSettingsManager.Settings.ContentID;
                    var contentProfile = AppManager.I.PlayerProfileManager.GetContentProfile(contentID);
                    AppManager.I.NavigationManager.NavData.CurrentContent = contentProfile;

                    if (AppManager.VERBOSE_INVERSION)
                        Debug.LogError("[Inversion] Current content: " + AppManager.I.NavigationManager.NavData.CurrentContent.ToString());
                }
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
            updateClassroomName();
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

        private void updateClassroomName()
        {
            var classroom = AppManager.I.AppSettingsManager.Settings.GetClassroom();
            if (classroom != "")
            {
                ClassroomName.text = "Class " + classroom;
            }
            else
            {
                ClassroomName.text = "";
            }
        }
    }
}
