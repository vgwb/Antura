using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class DialoguePostcardFocusView : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent OnClicked = new("DialoguePostcardFocusView.OnClicked");

        #endregion

        #region Serialized

        [Header("References")]

        [DeEmptyAlert]
        [SerializeField] RectTransform bg;
        [DeEmptyAlert]
        [SerializeField] Image img;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;

        #endregion

        public bool IsOpen { get; private set; }

        bool initialized;
        float titleBottomOffset;
        RectTransform imgRT;
        Button bt;
        Tween showTween;

        #region Unity + INIT

        void Init()
        {
            if (initialized)
                return;

            initialized = true;

            imgRT = (RectTransform)img.transform;
            titleBottomOffset = imgRT.offsetMin.y;

            bt = this.GetComponent<Button>();

            bt.onClick.AddListener(OnClicked.Dispatch);

            const float tweenDuration = 0.6f;
            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(bg.DOAnchorPosY(1440, tweenDuration).From(true).SetEase(Ease.OutQuint))
                .OnRewind(() => this.gameObject.SetActive(false));
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(Sprite sprite, string title = null)
        {
            Init();
            
            IsOpen = true;
            img.sprite = sprite;
            bool hasTitle = !string.IsNullOrEmpty(title);
            tfTitle.gameObject.SetActive(hasTitle);
            if (hasTitle) tfTitle.text = title;
            imgRT.offsetMin = new Vector2(imgRT.offsetMin.x, hasTitle ? titleBottomOffset : 0);
            showTween.timeScale = 1;
            showTween.Restart();
            this.gameObject.SetActive(true);
        }

        public void Hide(bool immediate = false)
        {
            Init();
            
            IsOpen = false;
            if (immediate)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                showTween.timeScale = 2;
                showTween.PlayBackwards();
            }
        }

        #endregion
    }
}