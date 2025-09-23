using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class DialoguePostcard : MonoBehaviour
    {
        public enum ViewMode
        {
            Fit,
            Crop
        }
        
        #region Events

        // public readonly ActionEvent<Sprite> OnClicked = new("DialoguePostcard.OnClicked");
        // public readonly ActionEvent OnFocusViewOpened = new("DialoguePostcard.OnFocusViewOpened");
        // public readonly ActionEvent OnFocusViewClosed = new("DialoguePostcard.OnFocusViewClosed");

        #endregion

        #region Serialized

        [SerializeField] ViewMode viewMode = ViewMode.Crop;

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] DialoguePostcardFocusView focusView;
        [DeEmptyAlert]
        [SerializeField] Image img;
        [DeEmptyAlert]
        [SerializeField] RectTransform titleContent;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;

        #endregion

        public bool IsActive { get; private set; }
        public bool IsMagnified => focusView.IsOpen;
        public Sprite CurrSprite { get; private set; }
        public bool CurrSpriteWasMagnifiedOnce { get; private set; }

        bool initialized;
        bool hasEntranceExitAnimations;
        string currTitle;
        Vector2 defImgSize;
        RectTransform imgRT;
        Button bt;
        Tween showTween, hideTween;

        #region Unity + INIT

        void Init()
        {
            if (initialized) return;

            initialized = true;

            imgRT = (RectTransform)img.transform;
            defImgSize = imgRT.sizeDelta;
            
            focusView.Hide(true);
        }

        void Start()
        {
            Init();
            
            bt = this.GetComponent<Button>();
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector3 defScale = this.transform.localScale;
            Vector3 defRot = this.transform.localEulerAngles;
            Vector2 defAnchoredP = rt.anchoredPosition;

            bt.onClick.AddListener(Magnify);

            const float inDuration = 0.35f;
            const float outDuration = 0.35f;
            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(rt.DOAnchorPos(defAnchoredP, inDuration).From(defAnchoredP + new Vector2(-380, -380)).SetEase(Ease.OutCubic))
                .Join(this.transform.DOScale(defScale, inDuration).From(0).SetEase(Ease.OutBack))
                .Join(this.transform.DOLocalRotate(defRot, inDuration, RotateMode.FastBeyond360).From(new Vector3(0, 0, 90)).SetEase(Ease.OutCubic))
                .Insert(inDuration * 0.5f, titleContent.DOAnchorPosY(200, 0.35f).From(true).SetEase(Ease.OutQuad));
            hideTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(this.transform.DOScale(0, outDuration).From(defScale).SetEase(Ease.InBack))
                .Join(rt.DOAnchorPos(defAnchoredP + new Vector2(290, 340), outDuration).From(defAnchoredP).SetEase(Ease.InQuad))
                .Join(this.transform.DOLocalRotate(new Vector3(0, 0, 60), outDuration).From(defRot).SetEase(Ease.InSine))
                .OnComplete(() => this.gameObject.SetActive(false));

            this.gameObject.SetActive(false);
            
            focusView.OnClicked.Subscribe(OnFocusViewClicked);
        }

        void OnDestroy()
        {
            showTween.Kill();
            hideTween.Kill();
            focusView.OnClicked.Unsubscribe(OnFocusViewClicked);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the postcard with an entrance animation
        /// (mainly to be used with normal dialogue postcards),
        /// with options for title, magnification and view mode
        /// </summary>
        public void Show(Sprite sprite, string title = null, bool magnified = false, ViewMode? customViewMode = null)
        {
            DoShow(sprite, title, magnified, customViewMode, false);
        }
        
        /// <summary>
        /// Shows the postcard immediately and without animations,
        /// with options for title, magnification and view mode
        /// </summary>
        public void ShowImmediate(Sprite sprite, string title = null, bool magnified = false, ViewMode? customViewMode = null)
        {
            DoShow(sprite, title, magnified, customViewMode, true);
        }

        /// <summary>
        /// Hides the postcard with or without animations depending if it was first shown via <see cref="Show"/> or <see cref="ShowImmediate"/>
        /// </summary>
        public void Hide()
        {
            Init();
            
            IsActive = false;
            CurrSprite = null;
            showTween.Complete();
            if (hasEntranceExitAnimations) hideTween.Restart();
            else hideTween.Complete();
            focusView.Hide();
        }

        /// <summary>
        /// Shows the fullscreen zoomed in version of the postcard
        /// </summary>
        public void Magnify()
        {
            Init();
            
            CurrSpriteWasMagnifiedOnce = true;
            focusView.Show(CurrSprite, currTitle);
        }

        /// <summary>
        /// Closes the magnified focus view of the postcard
        /// </summary>
        public void CloseMagnification()
        {
            Init();
            
            focusView.Hide();
        }

        #endregion

        #region Methods

        void DoShow(Sprite sprite, string title, bool magnified, ViewMode? customViewMode, bool immediate)
        {
            Init();
            
            IsActive = true;
            img.sprite = sprite;
            hasEntranceExitAnimations = !immediate;
            currTitle = title;
            if (CurrSprite != sprite)
            {
                CurrSpriteWasMagnifiedOnce = false;
                bool hasTitle = !string.IsNullOrEmpty(title);
                titleContent.gameObject.SetActive(hasTitle);
                if (hasTitle) tfTitle.text = title;
                hideTween.Complete();
                if (immediate || magnified) showTween.Complete();
                else showTween.Restart();
                ViewMode m = customViewMode == null ? viewMode : (ViewMode)customViewMode;
                switch (m)
                {
                    case ViewMode.Fit:
                        imgRT.sizeDelta = defImgSize;
                        break;
                    case ViewMode.Crop:
                        bool isLandscape = img.sprite.texture.width >= img.sprite.texture.height;
                        imgRT.sizeDelta = isLandscape ? new Vector2(3000, defImgSize.y) : new Vector2(defImgSize.x, 3000);
                        break;
                }
            }
            this.gameObject.SetActive(true);
            CurrSprite = sprite;
            if (magnified) Magnify();
        }

        #endregion

        #region Callbacks

        void OnFocusViewClicked()
        {
            CloseMagnification();
        }

        #endregion
    }
}