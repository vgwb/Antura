using System.Collections;
using System.Collections.Generic;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Discover
{
    public class DialogueChoices : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent<int> OnChoiceConfirmed = new("DialogueChoices.OnChoiceConfirmed");

        #endregion

        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] ChoicesLayout textChoicesLayout;
        [DeEmptyAlert]
        [SerializeField] ChoicesLayout imageChoicesLayout;

        #endregion

        public bool IsOpen { get; private set; }
        public bool IsOpening { get; private set; }
        public bool IsHiding { get; private set; }

        ChoicesLayout currLayout;
        ChoicesLayout[] allLayouts;
        Coroutine coShow, coHide;
        Sequence showTween, hideTween;

        #region Unity

        void Awake()
        {
            allLayouts = this.GetComponentsInChildren<ChoicesLayout>(true);
            foreach (ChoicesLayout layout in allLayouts)
                layout.OnChoiceConfirmed.Subscribe(OnLayoutChoiceConfirmed);
        }

        void Start()
        {
            SetInteractable(false);
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            showTween.Kill();
            hideTween.Kill();
            foreach (ChoicesLayout layout in allLayouts)
            {
                if (layout != null)
                    layout.OnChoiceConfirmed.Unsubscribe(OnLayoutChoiceConfirmed);
            }
        }

        #endregion

        #region Public Methods

        public void Show(List<NodeChoice> choiceElements, bool isQuiz, bool UseLearningLanguage)
        {
            this.gameObject.SetActive(true);
            this.RestartCoroutine(ref coShow, CO_Show(choiceElements, isQuiz, UseLearningLanguage));
        }

        IEnumerator CO_Show(List<NodeChoice> choiceElements, bool isQuiz, bool UseLearningLanguage)
        {
            IsOpen = IsOpening = true;
            IsHiding = false;
            currLayout = textChoicesLayout; // TODO > use correct layout when we have a system to distinguish it
            currLayout.Show(choiceElements, isQuiz, UseLearningLanguage);
            while (currLayout.IsShowingOrHidingElements)
                yield return null;
            IsOpening = false;
            coShow = null;
        }

        public void Hide(int confirmedChoiceIndex = -1)
        {
            this.RestartCoroutine(ref coHide, CO_Hide(confirmedChoiceIndex));
        }
        IEnumerator CO_Hide(int confirmedChoiceIndex = -1)
        {
            IsOpen = IsOpening = false;
            IsHiding = true;
            currLayout.Hide(confirmedChoiceIndex);
            while (currLayout.IsShowingOrHidingElements)
                yield return null;
            this.gameObject.SetActive(false);
            IsHiding = false;
        }

        #endregion

        #region Public Methods

        void SetInteractable(bool interactable)
        {
            foreach (ChoicesLayout layout in allLayouts)
                layout.Interactable = interactable;
        }

        #endregion

        #region Callbacks

        void OnLayoutChoiceConfirmed(int index)
        {
            OnChoiceConfirmed.Dispatch(index);
        }

        #endregion
    }
}
