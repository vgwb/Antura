using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace Antura.Discover
{
    public class TaskCounter : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TMP_Text tfCurrent;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTarget;

        #endregion

        bool isOpen;
        int totItemsCollected;
        int targetItems;
        Tween showTween, changeTween;

        #region Unity + INIT

        public void Awake()
        {
            showTween = ((RectTransform)this.transform).DOAnchorPosX(-4, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutQuart)
                .OnRewind(() => this.gameObject.SetActive(false));
            changeTween = tfCurrent.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f).SetAutoKill(false).Pause();
            
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            changeTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show()
        {
            if (isOpen) return;

            isOpen = true;
            
            showTween.Restart();
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (!isOpen) return;

            isOpen = false;
            
            showTween.PlayBackwards();
        }

        public void Setup(int pTotItemsCollected, int pTargetItems)
        {
            totItemsCollected = pTotItemsCollected;
            targetItems = pTargetItems;
            if (targetItems <= 0) Hide();
            else
            {
                Show();
                Refresh(true);
            }
        }

        public void SetTargetItemsTo(int value)
        {
            targetItems = value;
            totItemsCollected = ValidateAsTotItemsCollected(totItemsCollected);
        }

        public void SetTotItemsCollectedTo(int value)
        {
            int prev = totItemsCollected;
            totItemsCollected = ValidateAsTotItemsCollected(value);
            if (targetItems != prev) Refresh();
        }

        #endregion

        #region Methods

        void Refresh(bool immediate = false)
        {
            tfCurrent.text = totItemsCollected.ToString();
            tfTarget.text = targetItems.ToString();
            if (immediate) changeTween.Complete();
            else changeTween.Restart();
        }

        int ValidateAsTotItemsCollected(int value)
        {
            if (value < 0)
            {
                Debug.LogWarning($"TaskCounter : Can't set totItemsCollected to less than 0 ({value}), forcing it to 0");
                value = 0;
            }
            else if (value > targetItems)
            {
                Debug.LogWarning($"TaskCounter : Can't set totItemsCollected to more than current targetItems ({value}/{targetItems}), forcing it to {targetItems}");
                value = targetItems;
            }
            return value;
        }

        #endregion
    }
}