using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
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

        public readonly ActionEvent<Sprite> OnClicked = new("DialoguePostcard.OnClicked");

        #endregion

        #region Serialized

        [SerializeField] ViewMode viewMode = ViewMode.Crop;
        
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Image img;

        #endregion

        public bool IsActive { get; private set; }
        public Sprite CurrSprite { get; private set; }

        bool initialized;
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
        }

        void Start()
        {
            Init();
            
            bt = this.GetComponent<Button>();
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector3 defScale = this.transform.localScale;
            Vector3 defRot = this.transform.localEulerAngles;
            Vector2 defAnchoredP = rt.anchoredPosition;

            bt.onClick.AddListener(() => OnClicked.Dispatch(CurrSprite));

            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(rt.DOAnchorPos(defAnchoredP, 0.5f).From(defAnchoredP + new Vector2(-380, -960)).SetEase(Ease.OutCubic))
                .Join(this.transform.DOScale(defScale, 0.5f).From(0).SetEase(Ease.OutBack))
                .Join(this.transform.DOLocalRotate(defRot, 0.5f, RotateMode.FastBeyond360).From(new Vector3(0, 0, 960)).SetEase(Ease.OutCubic));
            hideTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(this.transform.DOScale(0, 0.5f).From(defScale).SetEase(Ease.InBack))
                .Join(rt.DOAnchorPos(defAnchoredP + new Vector2(290, 340), 0.5f).From(defAnchoredP).SetEase(Ease.InQuad))
                .Join(this.transform.DOLocalRotate(new Vector3(0, 0, 60), 0.5f).From(defRot).SetEase(Ease.InSine))
                .OnComplete(() => this.gameObject.SetActive(false));

            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            hideTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(Sprite sprite, ViewMode? customViewMode = null)
        {
            Init();
            
            IsActive = true;
            img.sprite = sprite;
            if (CurrSprite != sprite)
            {
                hideTween.Complete();
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
        }

        public void Hide()
        {
            IsActive = false;
            CurrSprite = null;
            showTween.Complete();
            hideTween.Restart();
        }

        #endregion
    }
}