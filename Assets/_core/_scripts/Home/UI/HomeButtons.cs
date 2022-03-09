using Antura.Audio;
using Antura.Core;
using Antura.Scenes;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// Control buttons for the (_Start) scene.
    /// </summary>
    public class HomeButtons : MonoBehaviour
    {
        public HomeScene HomeMngr;
        public Credits CreditsWindow;
        public MenuButton BtPlay, BtMusic, BtFx, BtCredits;

        MenuButton[] menuBts;

        void Start()
        {
            menuBts = this.GetComponentsInChildren<MenuButton>(true);
            foreach (MenuButton bt in menuBts)
            {
                MenuButton b = bt;
                b.Bt.onClick.AddListener(() => OnClick(b));
            }

            BtMusic.Toggle(AudioManager.I.MusicEnabled);
            BtFx.Toggle(AppManager.I.AppSettings.HighQualityGfx);
        }

        void OnDestroy()
        {
            foreach (MenuButton bt in menuBts)
            {
                bt.Bt.onClick.RemoveAllListeners();
            }
        }

        void OnClick(MenuButton bt)
        {
            switch (bt.Type)
            {
                case MenuButtonType.MusicToggle: // Music on/off
                    AudioManager.I.ToggleMusic();
                    BtMusic.Toggle(AudioManager.I.MusicEnabled);
                    break;
                case MenuButtonType.FxToggle: // FX on/off
                    AppManager.I.AppSettingsManager.ToggleQualitygfx();
                    BtFx.Toggle(AppManager.I.AppSettings.HighQualityGfx);
                    break;
                case MenuButtonType.Continue:
                    HomeMngr.Play();
                    break;
                case MenuButtonType.Credits:
                    CreditsWindow.Open();
                    break;
            }
        }
    }
}
