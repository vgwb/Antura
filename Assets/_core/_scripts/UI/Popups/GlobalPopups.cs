using System;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class GlobalPopups : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image bgBlocker;
        [DeEmptyAlert]
        [SerializeField] SelectorPopup selectorPopup;
        [DeEmptyAlert]
        [SerializeField] TextInputPopup textInputPopup;

        #endregion

        public const float ShowTime = 0.35f;
        
        static GlobalPopups I;
        const string ResourcesPath = "Prefabs/UI/GlobalPopupsUI";
        Tween showBlockerTween;

        #region Unity + INIT

        static void Init()
        {
            if (I != null) return;

            I = Instantiate(Resources.Load<GlobalPopups>(ResourcesPath));
            DontDestroyOnLoad(I.gameObject);

            I.showBlockerTween = I.bgBlocker.DOFade(0, ShowTime).From().SetAutoKill(false)
                .SetEase(Ease.Linear)
                .OnRewind(() => I.gameObject.SetActive(false));
            
            I.selectorPopup.OnClosing.Subscribe(I.OnClosingPopup);
            I.textInputPopup.OnClosing.Subscribe(I.OnClosingPopup);
        }

        void Start()
        {
            if (I == null) this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
            showBlockerTween.Kill();
            selectorPopup.OnClosing.Unsubscribe(OnClosingPopup);
            textInputPopup.OnClosing.Unsubscribe(OnClosingPopup);
        }

        #endregion

        #region Public Methods

        public static void OpenSelector(string title, List<string> values, Action<int> onSelected, bool hasCloseButton = true, params int[] specialItemsIndexes)
        {
            Init();
            I.Show();
            I.selectorPopup.Open(title, values, onSelected, hasCloseButton, specialItemsIndexes);
        }

        [DeMethodButton]
        public static void OpenTextInput(string title, string existingText = "Enter text", Action<string> onSubmit = null)
        {
            Init();
            I.Show();
            I.textInputPopup.Open(title, existingText, onSubmit);
        }

        #endregion

        #region Methods

        void Show()
        {
            showBlockerTween.Restart();
            this.gameObject.SetActive(true);
        }

        void Hide()
        {
            showBlockerTween.PlayBackwards();
        }

        #endregion

        #region Callbacks

        void OnClosingPopup()
        {
            Hide();
        }

        #endregion
    }
}