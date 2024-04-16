using System;
using System.Collections.Generic;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class Legacy_DialogueChoices : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent<int> OnChoiceSelected = new("DialogueChoices.OnChoiceSelected");

        #endregion
        
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Legacy_DialogueChoice[] choices;

        #endregion

        public bool IsOpen { get; private set; }
        
        Sequence showTween, hideTween;

        #region Unity

        void Start()
        {
            for (int i = 0; i < choices.Length; i++)
            {
                int index = i;
                choices[i].Button.onClick.AddListener(() => OnChoiceClicked(index));
            }
            SetInteractable(false);
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            hideTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(List<HomerElement> choiceElements)
        {
            IsOpen = true;
            showTween.Kill();
            hideTween.Kill();
            showTween = DOTween.Sequence()
                .OnComplete(() => SetInteractable(true));
            int totChoices = choiceElements.Count;
            for (int i = 0; i < choices.Length; i++)
            {
                Legacy_DialogueChoice choice = choices[i];
                choice.gameObject.SetActive(i < totChoices);
                if (i >= totChoices) continue;
                choice.SetText(choiceElements[i]._localizedContents[0]._text);
                float interval = 0.1f * i;
                showTween
                    .Insert(interval, choice.RectT.DOAnchorPosX(choice.DefAnchoredP.x, 0.5f).From(new Vector2(choice.DefAnchoredP.x + 500, 0)).SetEase(Ease.OutBack))
                    .Join(choice.CanvasGroup.DOFade(1, 0.2f).From(0).SetEase(Ease.Linear));
            }
            this.gameObject.SetActive(true);
        }
        
        public void Hide(int selectedChoiceIndex = -1)
        {
            IsOpen = false;
            SetInteractable(false);
            showTween.Kill();
            hideTween.Kill();
            hideTween = DOTween.Sequence()
                .OnComplete(() => this.gameObject.SetActive(false));
            int unselectedIndex = -1;
            for (int i = 0; i < choices.Length; i++)
            {
                Legacy_DialogueChoice choice = choices[i];
                if (!choice.gameObject.activeSelf) break;
                if (i == selectedChoiceIndex)
                {
                    hideTween.Insert(0f, choice.transform.DOPunchScale(Vector3.one * 0.35f, 0.35f, 6))
                        .Insert(0.2f, choice.CanvasGroup.DOFade(0, 0.15f).SetEase(Ease.InSine));
                }
                else
                {
                    unselectedIndex++;
                    hideTween.Insert(unselectedIndex * 0.05f + 0.15f, choice.CanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Linear))
                        .Insert(unselectedIndex * 0.05f, choice.RectT.DOAnchorPosX(100, 0.35f).SetEase(Ease.InSine));
                }
            }
        }

        #endregion

        #region Public Methods

        void SetInteractable(bool interactable)
        {
            foreach (Legacy_DialogueChoice choice in choices) choice.SetInteractable(interactable);
        }

        #endregion

        #region Callbacks

        void OnChoiceClicked(int index)
        {
            OnChoiceSelected.Dispatch(index);
        }

        #endregion
    }
}