using Antura.Database;
using Antura.Helpers;
using Antura.UI;
using Antura.Core;
using Antura.LivingLetters;
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

        private BookDataManager myManager;
        private WordInfo myWordInfo;
        private UIButton uIButton;

        public void Init(BookDataManager _manager, WordInfo _wordInfo, bool _selected)
        {
            myWordInfo = _wordInfo;
            myManager = _manager;
            uIButton = GetComponent<UIButton>();

            if (myWordInfo.unlocked || AppManager.I.Player.IsDemoUser) {
                OkIcon.enabled = true;
            } else {
                OkIcon.enabled = false;
            }

            Title.text = myWordInfo.data.Text;
            SubTitle.text = AppManager.I.ParentEdition.ShowSubtitles ? myWordInfo.data.Id : "";

            if (myWordInfo.data.Drawing != "")
            {
                Drawing.SetLetterData(new LL_ImageData(myWordInfo.data));
            } else {
                Drawing.text = "";
            }

            highlight(_selected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            myManager.DetailWord(myWordInfo);
        }

        public void Select(string code)
        {
            if (myWordInfo != null) {
                highlight(code == myWordInfo.data.Id);
            }
        }

        private void highlight(bool _status)
        {
            uIButton.Toggle(_status);
        }
    }
}