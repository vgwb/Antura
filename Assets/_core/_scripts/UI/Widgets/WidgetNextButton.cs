using Antura.Audio;
using UnityEngine;
using System;

namespace Antura.UI
{
    /// <summary>
    /// A general-purpose *Next* button.
    /// </summary>
    public class WidgetNextButton : MonoBehaviour
    {
        public static WidgetNextButton I;

        public GameObject WidgetPanel;

        private Action currentCallback;

        void Awake()
        {
            I = this;
        }

        public void Show(Action callback)
        {
            currentCallback = callback;

            AudioManager.I.PlaySound(Sfx.UIPopup);
            WidgetPanel.SetActive(true);
        }

        public void Close()
        {
            AudioManager.I.PlaySound(Sfx.UIButtonClick);
            WidgetPanel.SetActive(false);
        }

        public void OnPressButton()
        {
            Close();
            if (currentCallback != null)
            {
                currentCallback();
            }
        }
    }
}
