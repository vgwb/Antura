using Antura.Database;
using Antura.Helpers;
using Antura.UI;
using Antura.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Displays a Word item in the Dictionary page of the Player Book.
    /// </summary>
    public class ItemWord : MonoBehaviour, IPointerClickHandler
    {
        public TextRender Title;
        public TextRender SubTitle;
        public TextRender Drawing;
        public Image OkIcon;

        private WordsPage myManager;
        private WordInfo myWordInfo;
        private UIButton uIButton;

        public void Init(WordsPage _manager, WordInfo _wordInfo, bool _selected)
        {
            myWordInfo = _wordInfo;
            myManager = _manager;
            uIButton = GetComponent<UIButton>();

            if (myWordInfo.unlocked || AppManager.I.Player.IsDemoUser) {
                OkIcon.enabled = true;
            } else {
                OkIcon.enabled = false;
            }

            Title.text = myWordInfo.data.Arabic;
            SubTitle.text = myWordInfo.data.Id;

            if (myWordInfo.data.Drawing != "") {
                Drawing.text = AppManager.I.VocabularyHelper.GetWordDrawing(myWordInfo.data);
                if (myWordInfo.data.Category == WordDataCategory.Color) {
                    Drawing.SetColor(GenericHelper.GetColorFromString(myWordInfo.data.Value));
                }
            } else {
                Drawing.text = "";
            }

            hightlight(_selected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            myManager.DetailWord(myWordInfo);
        }

        public void Select(string code)
        {
            if (myWordInfo != null) {
                hightlight(code == myWordInfo.data.Id);
            }
        }

        private void hightlight(bool _status)
        {
            uIButton.Toggle(_status);
        }
    }
}