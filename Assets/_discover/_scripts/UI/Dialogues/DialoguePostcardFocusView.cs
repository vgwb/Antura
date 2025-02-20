using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialoguePostcardFocusView : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent OnClicked = new("DialoguePostcardFocusView.OnClicked");

        #endregion

        #region Serialized

        [DeEmptyAlert]
        [SerializeField] RectTransform bg;
        [DeEmptyAlert]
        [SerializeField] Image img;

        #endregion
        
        public bool IsOpen { get; private set; }

        bool initialized;
        Button bt;
        Tween showTween;

        #region Unity + INIT

        void Init()
        {
            if (initialized) return;

            initialized = true;
            
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

        public void Show(Sprite sprite)
        {
            Init();
            IsOpen = true;
            img.sprite = sprite;
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