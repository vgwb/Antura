using Antura.Helpers;
using Antura.UI;
using UnityEngine;

namespace Antura.Test
{
    /// <summary>
    /// Helper class to test the GlobalUI
    /// <seealso cref="GlobalUI"/>
    /// </summary>
    public class Tester_GlobalUI : MonoBehaviour
    {
        #region ActionFeedback

        public void ActionFeedback_Show(bool _feedback)
        {
            GlobalUI.I.ActionFeedback.Show(_feedback);
        }

        #endregion

        #region ContinueScreen

        public void ContinueScreen_ShowButton()
        {
            ContinueScreen.Show(null, ContinueScreenMode.Button);
        }

        public void ContinueScreen_ShowButtonWithBg()
        {
            ContinueScreen.Show(null, ContinueScreenMode.ButtonWithBg);
        }

        public void ContinueScreen_ShowButtonWithBgFullscreen()
        {
            ContinueScreen.Show(null, ContinueScreenMode.ButtonWithBgFullscreen);
        }

        public void ContinueScreen_ShowButtonFullscreen()
        {
            ContinueScreen.Show(null, ContinueScreenMode.ButtonFullscreen);
        }

        public void ContinueScreen_ShowFullscreenBg()
        {
            ContinueScreen.Show(null, ContinueScreenMode.FullscreenBg);
        }

        #endregion

        #region PauseMenu

        public void PauseMenu_ShowStart()
        {
            GlobalUI.ShowPauseMenu(true, PauseMenuType.StartScreen);
        }

        public void PauseMenu_ShowUtility()
        {
            GlobalUI.ShowPauseMenu(true, PauseMenuType.UtilityScreen);
        }

        public void PauseMenu_ShowGame()
        {
            GlobalUI.ShowPauseMenu(true, PauseMenuType.GameScreen);
        }

        #endregion

        #region Prompt

        public void Prompt_Show(bool yesOnly)
        {
            if (yesOnly)
                GlobalUI.ShowPrompt("لعربية", () => Debug.Log("YES"));
            else
                GlobalUI.ShowPrompt("لعربية", () => Debug.Log("YES"), () => Debug.Log("NO"));
        }

        #endregion

        #region WidgetPopup

        public void Popup_ShowSentence()
        {
            GlobalUI.WidgetPopupWindow.ShowSentence(() => GlobalUI.WidgetPopupWindow.Close(), Database.LocalizationDataId.Assessment_Start_2);
        }

        public void Popup_ShowImage()
        {
            //var answerData = AppManager.I.DB.GetWordDataByRandom();
            //LL_WordData randomWord = new LL_WordData(answerData.Id, answerData);
            //GlobalUI.WidgetPopupWindow.ShowStringAndWord(()=> GlobalUI.WidgetPopupWindow.Close(), "Title", randomWord);
        }

        #endregion

        #region WidgetSubtitles

        public void Subtitles_DisplaySentence(bool _isKeeper)
        {
            GlobalUI.WidgetSubtitles.DisplayDialogue(RandomTextId(), 3f, _isKeeper);
        }

        public void Subtitles_Close()
        {
            GlobalUI.WidgetSubtitles.Close();
        }

        Database.LocalizationDataId RandomTextId()
        {
            return RandomHelper.GetRandomParams(Database.LocalizationDataId.Assessment_Start_1,
                Database.LocalizationDataId.Assessment_Start_2,
                Database.LocalizationDataId.Assessment_Start_3,
                Database.LocalizationDataId.Assessment_Complete_1,
                Database.LocalizationDataId.Assessment_Classify_Letters);
        }

        #endregion
    }
}
