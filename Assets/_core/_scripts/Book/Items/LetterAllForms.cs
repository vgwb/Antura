using Antura.Core;
using Antura.Database;
using Antura.Language;

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
                isolatedChar = ReplaceWithShaddah(isolatedChar);
                InitialChar = ReplaceWithShaddah(InitialChar);
                MedialChar = ReplaceWithShaddah(MedialChar);
                FinalChar = ReplaceWithShaddah(FinalChar);
            }

            if (AppManager.I.ContentEdition.LearnMethod.ShowLinkedWordsInBook)
            {
                LetterTextIsolated.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                LetterTextInitial.gameObject.SetActive(false);
                LetterTextMedial.gameObject.SetActive(false);
                LetterTextFinal.gameObject.SetActive(false);
            }
            else
            {
                LetterTextIsolated.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 130);
                LetterTextInitial.gameObject.SetActive(true);
                LetterTextMedial.gameObject.SetActive(true);
                LetterTextFinal.gameObject.SetActive(true);
            }

            LetterTextIsolated.SetText(isolatedChar, Font2Use.Learning);
            LetterTextInitial.SetTextUnfiltered(InitialChar, Font2Use.Learning);
            LetterTextMedial.SetTextUnfiltered(MedialChar, Font2Use.Learning);
            LetterTextFinal.SetTextUnfiltered(FinalChar, Font2Use.Learning);

            if (Book.Book.I.EditDiacritics)
            {
                IsolatedSubtitle.gameObject.SetActive(true);
                IsolatedSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Isolated)}</color>");
                InitialSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Initial)}</color>");
                MedialSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Medial)}</color>");
                FinalSubtitle.text = ($"<color=black>{letterData.GetUnicode(LetterForm.Final)}</color>");
            }
        }

        private string ReplaceWithShaddah(string str)
        {
            var shaddah = "\u0651";
            bool hasSign = str.EndsWith("\u0640");
            if (hasSign)
                str = str.Remove(str.Length - 1, 1);
            str = $"{str}{shaddah}";
            if (hasSign)
                str += "\u0640";
            return str;
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
