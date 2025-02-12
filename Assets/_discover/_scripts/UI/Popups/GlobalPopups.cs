using System;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry.Popups
{
    public class GlobalPopups : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image bgBlocker;
        [DeEmptyAlert]
        [SerializeField] SelectorPopup selectorPopup;

        #endregion

        public const float ShowTime = 0.35f;
        
        static GlobalPopups I;
        const string ResourcesPath = "UI/UI Global Popups";
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
            
            I.selectorPopup.OnClosing.Subscribe(I.OnClosingSelector);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
            showBlockerTween.Kill();
            selectorPopup.OnClosing.Unsubscribe(OnClosingSelector);
        }

        #endregion

        #region Public Methods

        public static void OpenSelector(string title, List<string> values, Action<int> onSelected)
        {
            Init();
            I.Show();
            I.selectorPopup.Open(title, values, onSelected);
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

        void OnClosingSelector()
        {
            Hide();
        }

        #endregion
    }
}