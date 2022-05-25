using Antura.Core;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    // DEPRECATED
    public class SwitchLanguagePanel : MonoBehaviour
    {
        public CurrentLanguageIcon currentLanguageIcon;

        public SwitchLanguageButton prefabButton;
        private List<SwitchLanguageButton> buttons = new List<SwitchLanguageButton>();

        //private Tween moveTween;
        public void Open()
        {
            /*
            if (moveTween != null && moveTween.IsPlaying()) moveTween.Kill();
            moveTween = DOTween.Sequence().SetAutoKill(false)
                .OnPlay(() => gameObject.SetActive(true))
                .Append(GetComponent<RectTransform>().DOAnchorPosY(0, 0.2f));
                */
            GlobalUI.ShowPauseMenu(false);
            gameObject.SetActive(true);
            //moveTween.Play();
        }

        public void Close()
        {
            /*
            if (moveTween != null && moveTween.IsPlaying()) moveTween.Kill();
            moveTween = DOTween.Sequence().SetAutoKill(false)
                .Append(GetComponent<RectTransform>().DOAnchorPosY(-500, 0.2f))
                .OnComplete(() => gameObject.SetActive(false));
                */
            //moveTween.Play();
            GlobalUI.ShowPauseMenu(true);
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            //moveTween?.Kill();
        }

        public void Awake()
        {
            foreach (var lang in AppManager.I.ContentEdition.OverridenNativeLanguages)
            {
                var buttonGO = Instantiate(prefabButton.gameObject);
                buttonGO.transform.SetParent(prefabButton.transform.parent);
                buttonGO.transform.localScale = Vector3.one;
                buttonGO.SetActive(true);
                var button = buttonGO.GetComponent<SwitchLanguageButton>();
                button.Setup(lang);
                buttons.Add(button);
            }
            prefabButton.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            RefreshSelection();
        }

        public void RefreshSelection()
        {
            foreach (var button in buttons)
            {
                button.SetUnselected();
                if (button.Language == AppManager.I.AppSettings.NativeLanguage)
                    button.SetSelected();
            }
            currentLanguageIcon.OnEnable();
        }
    }
}
