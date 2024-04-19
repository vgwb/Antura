using System;
using System.Collections;
using System.Collections.Generic;
using Demigiant.DemiTools;
using Homer;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class ChoicesLayout : MonoBehaviour
    {
        public enum ChoicesType
        {
            Unset,
            Text,
            Image
        }

        #region EVENTS

        public readonly ActionEvent<int> OnChoiceConfirmed = new("ChoicesLayout.OnChoiceConfirmed");

        #endregion

        #region Serialized

        public ChoicesType Type;

        AbstractChoiceBox[] choiceBoxes;

        #endregion

        public bool Interactable {
            get { return allButtons[0].interactable; }
            set { SetInteractable(value); }
        }
        public bool IsShowingOrHidingElements {
            get {
                foreach (AbstractChoiceBox choiceBox in choiceBoxes)
                {
                    if (choiceBox.IsShowingOrHiding) return true;
                }
                return false;
            }
        }

        Button[] allButtons;
        Coroutine coShow, coHide;

        #region Unity

        void Awake()
        {
            choiceBoxes = this.GetComponentsInChildren<AbstractChoiceBox>();
            allButtons = this.GetComponentsInChildren<Button>();
            for (int i = 0; i < choiceBoxes.Length; i++)
            {
                choiceBoxes[i].SetIndex(i);
                choiceBoxes[i].OnSelect.Subscribe(OnChoiceBoxSelected);
                choiceBoxes[i].OnConfirm.Subscribe(OnChoiceBoxConfirmed);
            }
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            for (int i = 0; i < choiceBoxes.Length; i++) choiceBoxes[i].OnSelect.Unsubscribe(OnChoiceBoxSelected);
        }

        #endregion

        #region Public Methods

        public void Show(List<HomerElement> choiceElements)
        {
            this.gameObject.SetActive(true);
            this.RestartCoroutine(ref coShow, CO_Show(choiceElements));
        }
        IEnumerator CO_Show(List<HomerElement> choiceElements)
        {
            SetInteractable(false);
            int totChoices = choiceElements.Count;
            for (int i = 0; i < choiceBoxes.Length; i++)
            {
                if (i >= totChoices) choiceBoxes[i].gameObject.SetActive(false);
                else
                {
                    choiceBoxes[i].gameObject.SetActive(true);
                    choiceBoxes[i].Show(choiceElements[i]._localizedContents[0]._text);
                    yield return new WaitForSeconds(i * 0.15f);
                }
            }
            while (IsShowingOrHidingElements) yield return null;
            SetInteractable(true);
            coShow = null;
        }
        

        public void Hide(int confirmedChoiceIndex)
        {
            this.RestartCoroutine(ref coHide, CO_Hide(confirmedChoiceIndex));
        }
        IEnumerator CO_Hide(int confirmedChoiceIndex)
        {
            SetInteractable(false);
            int unconfirmedTimeIndex = -1;
            for (int i = 0; i < choiceBoxes.Length; i++)
            {
                if (i == confirmedChoiceIndex) continue;
                unconfirmedTimeIndex++;
                choiceBoxes[i].Hide();
                yield return new WaitForSeconds(unconfirmedTimeIndex * 0.1f);
            }
            if (confirmedChoiceIndex >= 0)
            {
                choiceBoxes[confirmedChoiceIndex].Hide();
            }
            while (IsShowingOrHidingElements) yield return null;
            this.gameObject.SetActive(false);
            coHide = null;
        }

        #endregion

        #region Methods

        void SetInteractable(bool interactable)
        {
            foreach (AbstractChoiceBox choiceBox in choiceBoxes) choiceBox.SetInteractable(interactable);
        }
        
        #endregion

        #region Callbacks

        void OnChoiceBoxSelected(AbstractChoiceBox selectedChoiceBox)
        {
            foreach (AbstractChoiceBox choiceBox in choiceBoxes)
            {
                if (choiceBox != selectedChoiceBox) choiceBox.Deselect();
            }
        }
        
        void OnChoiceBoxConfirmed(AbstractChoiceBox confirmedChoiceBox)
        {
            OnChoiceConfirmed.Dispatch(confirmedChoiceBox.Index);
        }

        #endregion
    }
}