using System;
using Antura.Audio;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// A general-purpose button
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour
    {
        #region Serialized

        [DeComment("If Target Bt Img is NULL automatically finds image in same gameObject", style = DeCommentStyle.WrapNextLine, marginBottom = -1)]
        [SerializeField] Image targetBtImg;
        public Color BtToggleOffColor = Color.white;
        public Color BtLockedColor = Color.red;
        public bool ToggleIconAlpha = true;

        [Tooltip("If this is TRUE and a CanvasGroup is not found, it is automatically added")]
        public bool ToggleCanvasGroupAlpha = false;

        [DeToggleButton]
        [DeComment("Button will become non-interactable after the first click (interactivity will be re-enabled next time it's activated)", "OneShot", true, style = DeCommentStyle.WrapNextLine)]
        [DeComment("Button will allow multiple clicks", "OneShot", false, style = DeCommentStyle.WrapNextLine)]
        public bool OneShot = false;
        public bool AutoAnimateClick = true;
        public bool AutoPlayButtonFx = false;

        #endregion

        /// <summary>Default color of the button image</summary>
        [NonSerialized]
        public Color DefaultColor;

        public bool IsToggled { get; private set; }
        public bool IsLocked { get; private set; }

        public Button Bt
        {
            get
            {
                if (fooBt == null)
                    fooBt = this.GetComponent<Button>();
                return fooBt;
            }
        }

        public RectTransform RectT
        {
            get
            {
                if (fooRectT == null)
                    fooRectT = this.GetComponent<RectTransform>();
                return fooRectT;
            }
        }

        public Image BtImg
        {
            get
            {
                if (targetBtImg == null)
                {
                    targetBtImg = this.GetComponent<Image>();
                    DefaultColor = targetBtImg.color;
                }
                return targetBtImg;
            }
        }

        public Image Ico
        {
            get
            {
                if (!fooIcoSearched)
                {
                    fooIcoSearched = true;
                    Image[] icos = this.GetOnlyComponentsInChildren<Image>(true);
                    if (icos.Length > 0)
                        fooIco = icos[0];
                }
                return fooIco;
            }
            set
            {
                fooIcoSearched = true;
                fooIco = value;
            }
        }

        public CanvasGroup CGroup
        {
            get
            {
                if (fooCGroup == null)
                {
                    fooCGroup = this.GetComponent<CanvasGroup>();
                    if (fooCGroup == null)
                        fooCGroup = this.gameObject.AddComponent<CanvasGroup>();
                }
                return fooCGroup;
            }
        }

        private Button fooBt;
        private Image fooIco;
        private bool fooIcoSearched;
        private RectTransform fooRectT;
        private CanvasGroup fooCGroup;
        private bool tweensInitialized;
        private Tween clickTween, pulseTween;
        private bool interactivityChangedByOneShot;

        #region Unity + INIT

        void OnEnable()
        {
            if (Application.isPlaying)
            {
                UIDirector.Add(this);
                if (interactivityChangedByOneShot)
                {
                    interactivityChangedByOneShot = false;
                    Bt.interactable = true;
                }
            }
        }

        void OnDisable()
        {
            if (Application.isPlaying)
            { UIDirector.Remove(this); }
        }

        protected virtual void Awake()
        {
            clickTween = this.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.35f).SetAutoKill(false).SetUpdate(true).Pause();
            pulseTween = this.transform.DOScale(this.transform.localScale * 1.1f, 0.3f).SetAutoKill(false).SetUpdate(true).Pause()
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

            Bt.onClick.AddListener(OnInternalClick);
        }

        protected virtual void OnDestroy()
        {
            clickTween.Kill();
            pulseTween.Kill();
            Bt.onClick.RemoveAllListeners();
        }

        #endregion

        #region Public Methods

        public virtual void Toggle(bool _activate, bool _animateClick = false)
        {
            IsToggled = _activate;

            if (pulseTween != null && pulseTween.Elapsed() > 0)
            { pulseTween.Rewind(); }
            BtImg.color = _activate ? DefaultColor : IsLocked ? BtLockedColor : BtToggleOffColor;
            if (ToggleIconAlpha && Ico != null)
            { Ico.SetAlpha(_activate ? 1 : 0.4f); }
            if (ToggleCanvasGroupAlpha)
            { CGroup.alpha = _activate ? 1 : 0.4f; }

            if (_animateClick)
            { AnimateClick(true); }
        }

        public virtual void Lock(bool _doLock)
        {
            IsLocked = _doLock;
            BtImg.color = _doLock ? BtLockedColor : IsToggled ? DefaultColor : BtToggleOffColor;
            Bt.interactable = !_doLock;
        }

        public void ChangeDefaultColors(Color _defaultColor, Color? _toggleOffColor = null)
        {
            BtImg.color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, BtImg.color.a);
            DefaultColor = _defaultColor;
            if (_toggleOffColor != null)
            { BtToggleOffColor = (Color)_toggleOffColor; }
        }

        /// <summary>
        /// Pulsing stops automatically when the button is toggled or clicked (via <see cref="AnimateClick"/>)
        /// </summary>
        public void Pulse()
        {
            InitTweens();
            pulseTween.PlayForward();
        }

        public void StopPulsing()
        {
            InitTweens();
            pulseTween.Rewind();
        }

        public void AnimateClick(bool _force = false)
        {
            InitTweens();
            pulseTween.Rewind();
            if (AutoAnimateClick || _force)
            { clickTween.Restart(); }
        }

        public void PlayClickFx()
        {
            AudioManager.I.PlaySound(Sfx.UIButtonClick);
        }

        #endregion

        #region Methods

        void InitTweens()
        {
            if (tweensInitialized)
            { return; }

            tweensInitialized = true;
            clickTween.ForceInit();
            pulseTween.ForceInit();
        }

        #endregion

        #region Callbacks

        void OnInternalClick()
        {
            if (AutoAnimateClick)
            { AnimateClick(); }
            if (AutoPlayButtonFx)
            { PlayClickFx(); }
            if (OneShot)
            {
                interactivityChangedByOneShot = true;
                Bt.interactable = false;
            }
        }

        #endregion
    }
}
