using Antura.UI;
using UnityEngine;

namespace Antura
{
    /// <summary>
    /// Created by Daniele (demigiant) for internal testing. Please don't remove.
    /// In the build it will be just an empty MonoBehaviour so no strings attached.
    /// </summary>
    public class DemiTester : MonoBehaviour
    {
#if UNITY_EDITOR
        void Update()
        {
            // SceneTransitioner - SPACE to show/hide
            if (InputCompat.GetKeyDown(KeyCode.Space))
            {
                SceneTransitioner.Show(!SceneTransitioner.IsShown);
            }

            // Subtitles - T to show text, SHIFT+T to show keeper text, CTRL/CMD+T to close
            if (WidgetSubtitles.I != null)
            {
                if (InputCompat.GetKeyDown(KeyCode.T))
                {
                    if (InputCompat.GetKey(KeyCode.LeftControl) || InputCompat.GetKey(KeyCode.RightControl) || InputCompat.GetKey(KeyCode.LeftCommand) || InputCompat.GetKey(KeyCode.RightCommand))
                    {
                        WidgetSubtitles.I.Close();
                    }
                    else
                    {
                        var testData = new Database.LocalizationData();
                        WidgetSubtitles.I.DisplayDialogue(testData, 2, InputCompat.GetKey(KeyCode.LeftShift) || InputCompat.GetKey(KeyCode.RightShift));
                    }
                }
            }

            // Continue button - C to show, SHIFT+C to show fullscreen-button on the side
            if (WidgetSubtitles.I != null)
            {
                if (InputCompat.GetKeyDown(KeyCode.C))
                {
                    ContinueScreenMode continueScreenMode = InputCompat.GetKey(KeyCode.LeftShift) || InputCompat.GetKey(KeyCode.RightShift)
                        //                        ? ContinueScreenMode.FullscreenBg : ContinueScreenMode.ButtonWithBgFullscreen;
                        ? ContinueScreenMode.ButtonFullscreen : ContinueScreenMode.ButtonWithBg;
                    ContinueScreen.Show(null, continueScreenMode);
                }
            }

            // Popup - P to show/hide
            if (InputCompat.GetKeyDown(KeyCode.P))
            {
                WidgetPopupWindow.I.Show(!WidgetPopupWindow.IsShown);
            }
        }
#endif
    }
}
