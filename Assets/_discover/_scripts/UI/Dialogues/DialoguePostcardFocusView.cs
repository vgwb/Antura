using Antura.UI;
using Antura.Discover.Audio;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using System;
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
        [SerializeField] GameObject infoPanel;
        [DeEmptyAlert]
        [SerializeField] TextRender tfTitle;
        [DeEmptyAlert]
        [SerializeField] TextRender tfDescription;
        [DeEmptyAlert]
        [SerializeField] protected Button btTranslate;

        #endregion

        private CardData currCardData;
        private bool usingLearningLanguage;

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
            btTranslate.onClick.AddListener(ToggleTranslation);

            const float tweenDuration = 0.6f;
            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(bg.DOAnchorPosY(1440, tweenDuration).From(true).SetEase(Ease.OutQuint))
                .OnRewind(() => this.gameObject.SetActive(false));
        }

        void OnDestroy()
        {
            showTween.Kill();
            btTranslate.onClick.RemoveListener(ToggleTranslation);
        }

        #endregion

        #region Public Methods

        public void Show(Sprite sprite, CardData cardData = null)
        {
            currCardData = cardData;
            Init();

            IsOpen = true;
            img.sprite = sprite;
            bool hasTitle = cardData != null;
            infoPanel.SetActive(hasTitle);
            if (hasTitle)
            {
                DisplayText(QuestManager.I.LearningLangFirst);
            }
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

        private void ToggleTranslation()
        {
            if (QuestManager.I.HasTranslation)
            {
                usingLearningLanguage = !usingLearningLanguage;
            }
            DisplayText(usingLearningLanguage);
        }

        private void DisplayText(bool useLearningLanguage)
        {
            usingLearningLanguage = useLearningLanguage;
            if (usingLearningLanguage)
            {
                tfTitle.text = DiscoverDataManager.I.GetCardTitle(currCardData);
                tfDescription.text = DiscoverDataManager.I.GetCardDescription(currCardData);
                DiscoverDataManager.I.PlayCardTitle(currCardData, usingLearningLanguage);
            }
            else
            {
                tfTitle.text = currCardData.Title.GetLocalizedString();
                tfDescription.text = currCardData.Description.GetLocalizedString();
                DiscoverDataManager.I.PlayCardTitle(currCardData, usingLearningLanguage);
            }
        }

        #endregion
    }
}
