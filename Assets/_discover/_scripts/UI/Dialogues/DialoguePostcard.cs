using Antura.UI;
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
        [SerializeField] TextRender tfTitle;
        [DeEmptyAlert]
        [SerializeField] protected Button btTranslate;

        #endregion

        public bool IsActive { get; private set; }
        public bool IsMagnified => focusView.IsOpen;
        public Sprite CurrSprite { get; private set; }
        public bool CurrSpriteWasMagnifiedOnce { get; private set; }

        private CardData currCardData;
        private bool usingLearningLanguage;

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
            if (initialized)
                return;

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

            bt.onClick.AddListener(OpenZoomView);
            btTranslate.onClick.AddListener(ToggleTranslation);

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

            bt.onClick.RemoveListener(OpenZoomView);
            btTranslate.onClick.RemoveListener(ToggleTranslation);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the postcard with an entrance animation
        /// </summary>
        public void Show(CardData card, bool magnified = false, bool silent = false)
        {
            if (card == null)
                return;
            if (card.ImageAsset == null)
                return;

            currCardData = card;
            DoShow(card.ImageAsset.Image, silent, magnified, null, false);
        }

        public void Show(AssetData asset, bool magnified = false)
        {
            if (asset == null)
                return;

            currCardData = null;
            DoShow(asset.Image, true, magnified, null, false);
        }

        /// <summary>
        /// Shows the postcard immediately and without animations,
        /// with options for title, magnification and view mode
        /// </summary>
        public void ShowImmediate(Sprite sprite, string title = null, bool magnified = false, ViewMode? customViewMode = null)
        {
            currCardData = null;
            DoShow(sprite, true, magnified, customViewMode, true);
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
            if (hasEntranceExitAnimations)
                hideTween.Restart();
            else
                hideTween.Complete();
            focusView.Hide();
        }

        /// <summary>
        /// Shows the fullscreen zoomed in version of the postcard
        /// </summary>
        public void OpenZoomView()
        {
            Init();

            CurrSpriteWasMagnifiedOnce = true;
            focusView.Show(CurrSprite, currCardData);
        }

        /// <summary>
        /// Closes the magnified focus view of the postcard
        /// </summary>
        public void CloseZoomView()
        {
            Init();

            focusView.Hide();
        }

        #endregion

        #region Methods

        void DoShow(Sprite sprite, bool silent, bool magnified, ViewMode? customViewMode, bool immediate)
        {
            Init();

            IsActive = true;
            img.sprite = sprite;
            hasEntranceExitAnimations = !immediate;

            if (silent)
            {
                titleContent.gameObject.SetActive(false);
            }
            else
            {
                titleContent.gameObject.SetActive(true);
                DisplayTitle(QuestManager.I.LearningLangFirst);
            }

            if (CurrSprite != sprite)
            {
                CurrSpriteWasMagnifiedOnce = false;
                hideTween.Complete();
                if (immediate || magnified)
                    showTween.Complete();
                else
                    showTween.Restart();

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
            if (magnified)
                OpenZoomView();
        }

        private void ToggleTranslation()
        {
            if (QuestManager.I.HasTranslation)
            {
                usingLearningLanguage = !usingLearningLanguage;
                DisplayTitle(usingLearningLanguage);
            }
        }

        private void DisplayTitle(bool useLearningLanguage)
        {
            usingLearningLanguage = useLearningLanguage;
            if (usingLearningLanguage)
            {
                tfTitle.text = DiscoverDataManager.I.GetCardTitle(currCardData);
            }
            else
            {
                tfTitle.text = currCardData.Title.GetLocalizedString();
            }
        }
        #endregion

        #region Callbacks

        void OnFocusViewClicked()
        {
            CloseZoomView();
        }

        #endregion
    }
}
