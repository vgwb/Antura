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
        public AnturaPetSwitcher Antura;
        public AnturaPetSwitcher Cat;
        public LivingLetterController LL_letter;
        public LivingLetterController LL_word;
        public LivingLetterController LL_image;
        public GameObject DialogReservedArea;
        public GameObject ProfileSelectorUI;
        public GameObject PanelAppUpdate;
        public GameObject HomeLogo;
        public TextMeshProUGUI ClassroomName;

        protected override void Start()
        {
            base.Start();

            if (AppManager.I.NavigationManager.NavData.CurrentContent != null)
            {
                if (AppManager.VERBOSE_INVERSION)
                    Debug.LogError("[INVERSION] Set Current Content to NULL as we are in Home");
                AppManager.I.NavigationManager.NavData.CurrentContent = null;
            }

            if (EditionSelectionManager.MustChooseContentEditions)
            {
                // First choose a learning edition
                Antura.gameObject.SetActive(false);
                Cat.gameObject.SetActive(false);
                LL_letter.gameObject.SetActive(false);
                LL_word.gameObject.SetActive(false);
                LL_image.gameObject.SetActive(false);
                HomeLogo.SetActive(false);
                GlobalUI.ShowPauseMenu(false, PauseMenuType.StartScreen);
            }
            else
            {
                GlobalUI.ShowPauseMenu(true, PauseMenuType.StartScreen);
                // Fix: [Home] a wrong audio is played #508
                //KeeperManager.I.PlayDialogue(AppManager.I.ContentEdition.LearnMethod.TitleLocID, false, true, TutorCreateProfile, KeeperMode.LearningNoSubtitles);
                Antura.AnimController.State = AnturaAnimation;
                Cat.AnimController.State = AnturaAnimation;
                LL_letter.GetComponent<HomeSceneLetter>().ChangeLetter();
                LL_letter.State = LLAnimation;
                LL_word.GetComponent<HomeSceneLetter>().ChangeWord();
                LL_word.State = LLAnimation;
                LL_image.GetComponent<HomeSceneLetter>().ChangeWord();
                LL_image.State = LLAnimation;
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
            // We need to choose the content , then we will resume the play session
            SelectionManager.ContentEditionSelection(true);
        }

        public static void ResumeCurrentPlaySession()
        {

            // We must load the play session data, or create it
            if (AppManager.VERBOSE_INVERSION)
                Debug.LogError("[Inversion] Entering game with Player: " + AppManager.I.Player.Uuid);

            var contentID = AppManager.I.AppSettingsManager.Settings.ContentID;
            var contentProfile = AppManager.I.PlayerProfileManager.GetContentProfile(contentID);
            AppManager.I.NavigationManager.NavData.CurrentContent = contentProfile;

            if (AppManager.VERBOSE_INVERSION)
                Debug.LogError("[Inversion] Current content: " + AppManager.I.NavigationManager.NavData.CurrentContent.ToString());

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
            var classroomName = ClassroomHelper.GetClassroomName(AppManager.I.AppSettingsManager.Settings.ClassRoomMode);
            if (classroomName != "")
            {
                ClassroomName.text = "Class " + classroomName;
            }
            else
            {
                ClassroomName.text = "";
            }
        }
    }
}
