using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    public class ItemSpellingLetter : MonoBehaviour, IPointerClickHandler
    {
        public TextRender LetterText;
        public TextRender SubtitleText;

        private LetterData myLetterData;
        //        private UIButton uIButton;

        public void Init(LetterData _letterData)
        {
            myLetterData = _letterData;
            //            uIButton = GetComponent<UIButton>();

            if (myLetterData == null) {
                LetterText.SetTextUnfiltered("");
                SubtitleText.SetText("");
            } else {
                var isolatedChar = myLetterData.GetStringForDisplay(LetterForm.Isolated);
                LetterText.SetTextUnfiltered(isolatedChar);
                if (AppManager.I.ParentEdition.ShowNativeTooltips)
                    SubtitleText.SetText(myLetterData.Id);
                else
                    SubtitleText.SetText("");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.I.PlayLetter(myLetterData, true, LetterDataSoundType.Phoneme);
        }

    }
}