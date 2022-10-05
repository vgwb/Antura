using System;
using Antura.Audio;
using Antura.Database;
using DG.Tweening;
using Antura.Core;
using Antura.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Controls the transition animation between two scenes.
    /// </summary>
    public class SceneTransitioner : MonoBehaviour
    {
        [Header("Options")]
        public float AnimationDuration = 0.75f;

        [Header("References")]
        public Image MaskCover;

        public Image Icon, Logo;
        public UIMinigameVariationBadge Badge;

        public static bool IsShown { get; private set; }
        public static bool IsPlaying { get; private set; }

        Sprite defIcon;
        Action onCompleteCallback, onRewindCallback;
        Sequence tween;

        #region Unity

        private void OnEnable()
        {
            if (EditionSelectionManager.MustChooseContentEditions)
            {
                Logo.enabled = false;
            }
            else
            {
                Logo.enabled = true;
                Logo.sprite = AppManager.I.ContentEdition.TransitionLogo;
            }
        }

        void Awake()
        {
            defIcon = Icon.sprite;

            if (AppManager.I.RootConfig.DebugConfig.SpeedUpAnimations) AnimationDuration = 0.01f;

            tween = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause()
                .Append(MaskCover.DOFillAmount(0, AnimationDuration).From())
                .Join(Icon.transform.DOScale(0.01f, AnimationDuration * 0.6f).From())
                .Join(Icon.transform.DOPunchRotation(new Vector3(0, 0, 90), AnimationDuration * 0.9f, 6))
                .Insert(AnimationDuration * 0.4f, Logo.transform.DOScale(0.01f, AnimationDuration * 0.5f).From().SetEase(Ease.OutBack))
                .Join(Badge.transform.DOScale(0.01f, AnimationDuration * 0.5f).From().SetEase(Ease.OutBack))
                .OnPlay(() => this.gameObject.SetActive(true))
                .OnRewind(OnRewind)
                .OnComplete(OnComplete);

            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            tween.Kill();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Call this to show/hide the scene transitioner.
        /// </summary>
        /// <param name="_doShow">If TRUE animates the transition IN and stops when the screen is covered, otherwise animates OUT</param>
        /// <param name="_onComplete">Eventual callback to call when the transition IN/OUT completes</param>
        public static void Show(bool _doShow, Action _onComplete = null)
        {
            GlobalUI.Init();

            GlobalUI.SceneTransitioner.SetContent();
            GlobalUI.SceneTransitioner.DoShow(_doShow, _onComplete);
        }

        void DoShow(bool _doShow, Action _onComplete = null)
        {
            IsShown = _doShow;
            if (_doShow)
            {
                MaskCover.fillClockwise = true;
                onRewindCallback = null;
                onCompleteCallback = _onComplete;
                tween.Restart();
                this.gameObject.SetActive(true);
                AudioManager.I.PlaySound(Sfx.Transition);
                IsPlaying = true;
            }
            else
            {
                MaskCover.fillClockwise = false;
                onCompleteCallback = null;
                onRewindCallback = _onComplete;
                if (tween.Elapsed() <= 0)
                {
                    tween.Pause();
                    OnRewind();
                }
                else
                {
                    tween.PlayBackwards();
                }
            }
        }

        public void CloseImmediate()
        {
            onRewindCallback = null;
            tween.Rewind();
        }

        #endregion

        #region Methods

        void SetContent()
        {
            //if (AppConstants.VerboseLogging) Debug.Log(AppManager.I.NavigationManager.IsLoadingMinigame + " > " + AppManager.I.NavigationManager.CurrentMiniGameData);
            bool isLoadingMinigame = AppManager.I.NavigationManager.IsLoadingMinigame;
            Logo.gameObject.SetActive(!isLoadingMinigame);
            if (isLoadingMinigame)
            {
                MiniGameData mgData = AppManager.I.NavigationManager.CurrentMiniGameData;
                Icon.sprite = AppManager.I.AssetManager.GetMainIcon(mgData);
                Badge.Assign(mgData);
                Badge.gameObject.SetActive(mgData.HasBadge);
            }
            else
            {
                Badge.gameObject.SetActive(isLoadingMinigame);
                Icon.sprite = defIcon;
            }
        }

        void OnRewind()
        {
            IsPlaying = false;
            this.gameObject.SetActive(false);
            if (onRewindCallback != null)
            {
                onRewindCallback();
            }
        }

        void OnComplete()
        {
            Time.timeScale = 1;
            GlobalUI.Clear(false);
            if (onCompleteCallback != null)
            {
                onCompleteCallback();
            }
        }

        #endregion
    }
}
