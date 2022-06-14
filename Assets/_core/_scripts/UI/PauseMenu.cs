using Antura.Audio;
using Antura.Core;
using Antura.Keeper;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Shows and controls the Pause menu.
    /// Can be used throughout the application.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu I;

        [Header("Buttons")]
        public MenuButton BtPause;
        public MenuButton BtExit;
        public MenuButton BtMusic;
        public MenuButton BtFx;
        public MenuButton BtCredits;
        public MenuButton BtResume;
        public MenuButton BtSubtitles;

        [Header("Other")]
        public GameObject PauseMenuContainer;

        public Sprite AltPauseIconSprite;
        public Image MenuBg;
        public RectTransform SubButtonsContainer;
        public CreditsUI Credits;
        public RectTransform Logo;

        public bool IsMenuOpen { get; private set; }
        public bool typeSet;

        private Sprite defPauseIconSprite;
        private MenuButton[] menuBts;
        private Sequence openMenuTween;
        private Tween logoBobTween;

        void Awake()
        {
            I = this;
            defPauseIconSprite = BtPause.Bt.image.sprite;
            if (!Credits.HasAwoken)
            { Credits.gameObject.SetActive(true); }
        }

        void Start()
        {
            menuBts = PauseMenuContainer.GetComponentsInChildren<MenuButton>(true);

            // Tweens - Logo bobbing
            logoBobTween = Logo.DOAnchorPosY(16, 0.6f).SetRelative().SetUpdate(true).SetAutoKill(false).Pause()
                .SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
            logoBobTween.OnRewind(() => logoBobTween.SetEase(Ease.OutQuad))
                .OnStepComplete(() => logoBobTween.SetEase(Ease.InOutQuad));
            logoBobTween.ForceInit();

            // Tweens - menu
            CanvasGroup[] cgButtons = new CanvasGroup[menuBts.Length];
            for (int i = 0; i < menuBts.Length; i++)
            {
                cgButtons[i] = menuBts[i].GetComponent<CanvasGroup>();
            }
            openMenuTween = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause()
                .OnPlay(() => PauseMenuContainer.SetActive(true))
                .OnRewind(() =>
                {
                    PauseMenuContainer.SetActive(false);
                    logoBobTween.Rewind();
                });
            openMenuTween.Append(MenuBg.DOFade(0, 0.5f).From())
                .Join(Logo.DOAnchorPosY(750f, 0.4f).From().SetEase(Ease.OutQuad).OnComplete(() => logoBobTween.Play()))
                .Join(SubButtonsContainer.DORotate(new Vector3(0, 0, 180), 0.4f).From());

            const float btDuration = 0.3f;
            for (int i = 0; i < menuBts.Length; ++i)
            {
                CanvasGroup cgButton = cgButtons[i];
                RectTransform rtButton = cgButton.GetComponent<RectTransform>();
                openMenuTween.Insert(i * 0.05f, rtButton.DOScale(0.0001f, btDuration).From().SetEase(Ease.OutBack));
            }

            // Deactivate pause menu
            PauseMenuContainer.SetActive(false);

            if (!typeSet)
            { SetType(PauseMenuType.GameScreen); }

            // Listeners
            BtPause.Bt.onClick.AddListener(() => OnClick(BtPause));
            foreach (MenuButton bt in menuBts)
            {
                MenuButton b = bt; // Redeclare to fix Unity's foreach issue with delegates
                b.Bt.onClick.AddListener(() => OnClick(b));
            }
        }

        void OnDestroy()
        {
            openMenuTween.Kill();
            logoBobTween.Kill();
            BtPause.Bt.onClick.RemoveAllListeners();
            foreach (MenuButton bt in menuBts)
            {
                bt.Bt.onClick.RemoveAllListeners();
            }
        }

        void Update()
        {
            if (BtMusic.IsToggled != AudioManager.I.MusicEnabled)
            {
                BtMusic.Toggle(AudioManager.I.MusicEnabled);
            }
        }

        /// <summary>
        /// Opens or closes the pause menu
        /// </summary>
        /// <param name="_open">If TRUE opens, otherwise closes</param>
        public void OpenMenu(bool _open)
        {
            IsMenuOpen = _open;

            if (_open)
                AudioManager.I.RefreshMusicEnabled();

            // Set toggles
            BtMusic.Toggle(AudioManager.I.MusicEnabled);
            BtFx.Toggle(AppManager.I.AppSettings.HighQualityGfx);
            BtSubtitles.Toggle(AppManager.I.AppSettings.KeeperSubtitlesEnabled);
            BtSubtitles.gameObject.SetActive(AppManager.I.ContentEdition.LearnMethod.CanUseSubtitles && AppManager.I.ContentEdition.LearnMethod.EnableSubtitlesToggle);

            if (_open)
            {
                //timeScaleAtMenuOpen = Time.timeScale;
                Time.timeScale = 0;
                openMenuTween.timeScale = 1;
                openMenuTween.PlayForward();
                AudioManager.I.PlaySound(Sfx.UIPauseIn);
            }
            else
            {
                //Time.timeScale = timeScaleAtMenuOpen;
                Time.timeScale = 1;
                logoBobTween.Pause();
                openMenuTween.timeScale = 2; // Speed up tween when going backwards
                openMenuTween.PlayBackwards();
                AudioManager.I.PlaySound(Sfx.UIPauseOut);
            }
        }

        public void SetType(PauseMenuType _type)
        {
            typeSet = true;
            BtPause.Bt.image.sprite = _type == PauseMenuType.GameScreen ? defPauseIconSprite : AltPauseIconSprite;
            BtCredits.gameObject.SetActive(_type == PauseMenuType.StartScreen);
            BtExit.gameObject.SetActive(_type != PauseMenuType.StartScreen);
        }

        /// <summary>
        /// Callback for button clicks
        /// </summary>
        void OnClick(MenuButton _bt)
        {
            if (SceneTransitioner.IsPlaying)
            { return; }

            if (_bt == BtPause)
            {
                OpenMenu(!IsMenuOpen);
            }
            else if (!openMenuTween.IsPlaying())
            {
                // Ignores pause menu clicks when opening/closing menu
                switch (_bt.Type)
                {
                    case MenuButtonType.Back: // Exit
                        if (AppManager.I.NavigationManager.NavData.CurrentScene == AppScene.MiniGame)
                        {
                            // Prompt
                            GlobalUI.ShowPrompt(Database.LocalizationDataId.UI_AreYouSure, () =>
                            {
                                OpenMenu(false);
                                AppManager.I.NavigationManager.ExitToMainMenu();
                            },
                            () => { },
                            Language.LanguageUse.Learning
                            );
                        }
                        else
                        {
                            // No prompt
                            OpenMenu(false);
                            AppManager.I.NavigationManager.ExitToMainMenu();
                        }
                        break;
                    case MenuButtonType.MusicToggle: // Music on/off
                        AudioManager.I.ToggleMusic();
                        BtMusic.Toggle(AudioManager.I.MusicEnabled);
                        break;
                    case MenuButtonType.FxToggle: // FX on/off
                        AppManager.I.AppSettingsManager.ToggleQualitygfx();
                        BtFx.Toggle(AppManager.I.AppSettings.HighQualityGfx);
                        break;
                    case MenuButtonType.SubtitlesToggle:
                        AppManager.I.AppSettingsManager.ToggleKeeperSubtitles();
                        BtSubtitles.Toggle(AppManager.I.AppSettings.KeeperSubtitlesEnabled);
                        break;
                    case MenuButtonType.Credits:
                        Credits.Show(true);
                        break;
                    case MenuButtonType.Continue: // Resume
                        OpenMenu(false);
                        break;
                }
            }
        }
    }
}
