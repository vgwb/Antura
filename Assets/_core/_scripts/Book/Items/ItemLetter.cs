using Antura.Core;
using Antura.Database;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Displays an Letter item in the Dictionary page of the Player Book.
    /// </summary>
    public class ItemLetter : MonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        public TextRender Title;
        public TextRender SubTitle;
        public Image OkIcon;
        public Image BackgroundImage;

        public Color ColorVariation;
        public Color ColorSymbol;

        private LettersPage myManager;
        private LetterInfo myLetterInfo;
        //private UIButton uIButton;

        public void Init(LettersPage _manager, LetterInfo _info, bool _selected)
        {
            myLetterInfo = _info;
            myManager = _manager;
            //uIButton = GetComponent<UIButton>();

            if (myLetterInfo.unlocked || AppManager.I.Player.IsDemoUser)
            {
                OkIcon.enabled = true;
            }
            else
            {
                OkIcon.enabled = false;
            }

            Title.SetText(myLetterInfo.data.GetStringForDisplay(), Font2Use.Learning);
            SubTitle.text = AppManager.I.ContentEdition.LearnMethod.ShowHelpText ? myLetterInfo.data.Id : ""; //  + (myLetterInfo.data.Number > 0 ? " (" + myLetterInfo.data.Number + ")" : "");
            // + " " + myLetterInfo.data.Kind.ToString();
            highlight(_selected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            myManager.DetailLetter(myLetterInfo);
        }

        public void Select(string code)
        {

            highlight(code == myLetterInfo.data.Id);
        }

        private void highlight(bool _status)
        {
            if (_status)
            {
                BackgroundImage.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                ColorBackground();
            }
            //uIButton.Toggle(_status);
        }

        private void ColorBackground()
        {
            if (myLetterInfo.data.Kind == LetterDataKind.Letter)
            {
                BackgroundImage.color = Color.white;
            }
            else if (myLetterInfo.data.Kind == LetterDataKind.Symbol)
            {
                BackgroundImage.color = ColorSymbol;
            }
            else if (myLetterInfo.data.Kind == LetterDataKind.LetterVariation)
            {
                BackgroundImage.color = ColorVariation;
            }
            else
            {
                BackgroundImage.color = new Color(1f, 0.56f, 0.5f);
            }
        }
    }
}
