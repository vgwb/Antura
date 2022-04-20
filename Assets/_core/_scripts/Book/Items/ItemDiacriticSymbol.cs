using Antura.Core;
using Antura.Database;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    public class ItemDiacriticSymbol : MonoBehaviour, IPointerClickHandler
    {
        public TextRender LetterText;
        public TextRender EnglishLetterText;
        public Image OkIcon;

        private LettersPage myManager;
        private LetterInfo myLetterInfo;
        private UIButton uIButton;

        public void Init(LettersPage _manager, LetterInfo letterInfo, bool _selected)
        {
            myLetterInfo = letterInfo;
            uIButton = GetComponent<UIButton>();
            myManager = _manager;

            if (myLetterInfo.unlocked || AppManager.I.Player.IsDemoUser)
            {
                OkIcon.enabled = true;
            }
            else
            {
                OkIcon.enabled = false;
            }

            if (myLetterInfo == null)
            {
                LetterText.SetTextUnfiltered("");
                EnglishLetterText.SetText("");
            }
            else
            {
                var isolatedChar = myLetterInfo.data.GetStringForDisplay(LetterForm.Isolated);

                LetterText.SetTextUnfiltered(isolatedChar);
                EnglishLetterText.text = AppManager.I.ContentEdition.LearnMethod.ShowHelpText ? myLetterInfo.data.Id : "";
            }

            hightlight(_selected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            myManager.ShowDiacriticCombo(myLetterInfo);
        }

        public void Select(string code)
        {
            if (myLetterInfo != null)
            {
                hightlight(code == myLetterInfo.data.Id);
            }
        }

        void hightlight(bool _status)
        {
            uIButton.Toggle(_status);
        }
    }
}
