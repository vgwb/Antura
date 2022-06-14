using Antura.Audio;
using Antura.Core;
using Antura.Language;
using System;
using Antura.Keeper;
using UnityEngine;
using UnityEngine.UI;
using DG.DeExtensions;
using DG.Tweening;

namespace Antura.UI
{
    /// <summary>
    /// Shows an interactive YES/NO prompt panel.
    /// Uses callbacks to determine how to react to user input.
    /// </summary>
    public class PromptPanel : MonoBehaviour
    {
        public RectTransform Content;
        public TextRender TfMessage;
        public UIButton BtYes, BtNo;

        private Action onYes, onNo;
        private Action onCloseAction;
        private float defYesX;
        private RectTransform btYesRT;
        private Tween showTween;

        #region Unity

        void Awake()
        {
            btYesRT = BtYes.GetComponent<RectTransform>();
            defYesX = btYesRT.anchoredPosition.x;
            showTween = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause()
                .Append(this.GetComponent<Image>().DOFade(0, 0.35f).From())
                .Join(Content.DOScale(0.0001f, 0.35f).From().SetEase(Ease.OutBack))
                .OnRewind(() =>
                {
                    this.gameObject.SetActive(false);
                    OnClose();
                });

            BtYes.Bt.onClick.AddListener(() => OnClick(BtYes));
            BtNo.Bt.onClick.AddListener(() => OnClick(BtNo));

            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            BtYes.Bt.onClick.RemoveAllListeners();
            BtNo.Bt.onClick.RemoveAllListeners();
        }

        #endregion

        #region Public Methods

        public void Show(Database.LocalizationDataId id, LanguageUse _languageUse, Action _onYes, Action _onNo)
        {
            var localizationData = LocalizationManager.GetLocalizationData(id);

            if (_languageUse == LanguageUse.Native)
            {
                Show(localizationData.NativeText, _languageUse, _onYes, _onNo);
                KeeperManager.I.PlayDialogue(id, keeperMode: KeeperMode.NativeNoSubtitles);
            }
            else
            {
                Show(LocalizationManager.GetLearning(id), _languageUse, _onYes, _onNo);
                KeeperManager.I.PlayDialogue(id, keeperMode: KeeperMode.LearningThenNativeNoSubtitles);
            }
        }

        public void Show(string _message, Action _onYes, Action _onNo)
        {
            Show(_message, LanguageUse.Native, _onYes, _onNo);
        }

        public void Show(string _message, LanguageUse _languageUse, Action _onYes, Action _onNo)
        {
            onCloseAction = null;

            TfMessage.SetText(_message, _languageUse);

            onYes = _onYes;
            onNo = _onNo;
            btYesRT.SetAnchoredPosX(_onNo == null ? 0 : defYesX);
            BtNo.gameObject.SetActive(onNo != null);
            showTween.Restart();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            showTween.PlayBackwards();
        }

        #endregion

        #region Callbacks

        void OnClick(UIButton _bt)
        {
            if (showTween.IsBackwards())
            {
                return;
            }

            if (_bt == BtYes && onYes != null)
            {
                onCloseAction = onYes;
            }
            else if (_bt == BtNo && onNo != null)
            {
                onCloseAction = onNo;
            }
            Close();
        }

        void OnClose()
        {
            onCloseAction?.Invoke();
            onCloseAction = null;
        }

        #endregion
    }
}
