using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class LetterAllForms : MonoBehaviour
    {
        public TextRender LetterTextIsolated;
        public TextRender LetterTextInitial;
        public TextRender LetterTextMedial;
        public TextRender LetterTextFinal;

        private LetterData currentLetter;
        private bool lastTestShaddah;

        public void Init(LetterData letterData)
        {
            currentLetter = letterData;
            var isolatedChar = letterData.GetStringForDisplay(LetterForm.Isolated);
            var InitialChar = letterData.GetStringForDisplay(LetterForm.Initial);
            var MedialChar = letterData.GetStringForDisplay(LetterForm.Medial);
            var FinalChar = letterData.GetStringForDisplay(LetterForm.Final);

            if (Book.Book.I.TestShaddah)
            {
                isolatedChar = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(isolatedChar + "\u0651");
                InitialChar = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(InitialChar + "\u0651");
                MedialChar = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(MedialChar + "\u0651");
                FinalChar = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(FinalChar + "\u0651");
            }

            LetterTextIsolated.SetTextUnfiltered(isolatedChar);
            LetterTextInitial.SetTextUnfiltered(InitialChar);
            LetterTextMedial.SetTextUnfiltered(MedialChar);
            LetterTextFinal.SetTextUnfiltered(FinalChar);

            if (AppManager.I.ParentEdition.BookShowRelatedWords) {
                LetterTextIsolated.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                LetterTextInitial.gameObject.SetActive(false);
                LetterTextMedial.gameObject.SetActive(false);
                LetterTextFinal.gameObject.SetActive(false);
            } else {
                LetterTextIsolated.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 130);
                LetterTextInitial.gameObject.SetActive(true);
                LetterTextMedial.gameObject.SetActive(true);
                LetterTextFinal.gameObject.SetActive(true);
            }
        }

        void Update()
        {
            if (lastTestShaddah != Book.Book.I.TestShaddah)
            {
                lastTestShaddah = Book.Book.I.TestShaddah;
                Init(currentLetter);
            }
        }
    }
}