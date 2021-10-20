using Antura.Core;
using Antura.Database;
using TMPro;
using UnityEngine;

namespace Antura.UI
{
    public class LetterAllForms : MonoBehaviour
    {
        public TextRender LetterTextIsolated;
        public TextRender LetterTextInitial;
        public TextRender LetterTextMedial;
        public TextRender LetterTextFinal;

        public TextMeshProUGUI IsolatedSubtitle;
        public TextMeshProUGUI InitialSubtitle;
        public TextMeshProUGUI MedialSubtitle;
        public TextMeshProUGUI FinalSubtitle;

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
                var shaddah = "\u0651";
                isolatedChar = isolatedChar + shaddah;
                InitialChar = InitialChar + shaddah;
                MedialChar = MedialChar + shaddah;
                FinalChar = FinalChar + shaddah;
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


            if (Book.Book.I.EditDiacritics)
            {
                IsolatedSubtitle.gameObject.SetActive(true);
                IsolatedSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Isolated)}</color>");
                InitialSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Initial)}</color>");
                MedialSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Medial)}</color>");
                FinalSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Final)}</color>");
            }
        }

        void Update()
        {
            if (lastTestShaddah != Book.Book.I.TestShaddah)
            {
                lastTestShaddah = Book.Book.I.TestShaddah;
                Init(currentLetter);
            }

            if (Book.Book.I.EditDiacritics)
            {
                // Clear so it will be re-rendered again
                LetterTextIsolated.SetTextUnfiltered("");
                LetterTextInitial.SetTextUnfiltered("");
                LetterTextMedial.SetTextUnfiltered("");
                LetterTextFinal.SetTextUnfiltered("");

                Init(currentLetter);
            }
        }
    }
}