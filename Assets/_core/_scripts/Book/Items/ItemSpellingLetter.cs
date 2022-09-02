using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
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

        public void Init(LetterData _letterData)
        {
            myLetterData = _letterData;

            if (myLetterData == null)
            {
                LetterText.SetTextUnfiltered("");
                SubtitleText.SetText("");
            }
            else
            {
                var isolatedChar = myLetterData.GetStringForDisplay(LetterForm.Isolated);
                LetterText.SetText(isolatedChar, Font2Use.Learning);
                Debug.Log("Spelling " + isolatedChar);

                if (AppManager.I.ContentEdition.LearnMethod.ShowHelpText)
                    SubtitleText.SetText(myLetterData.Id);
                else
                    SubtitleText.SetText("");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
            AudioManager.I.PlayLetter(myLetterData, true, soundType);
        }

    }
}
