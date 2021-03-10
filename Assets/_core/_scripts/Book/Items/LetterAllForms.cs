using Antura.Core;
using Antura.Database;
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

        public void Init(LetterData letterData)
        {
            var isolatedChar = letterData.GetStringForDisplay(LetterForm.Isolated);
            var InitialChar = letterData.GetStringForDisplay(LetterForm.Initial);
            var MedialChar = letterData.GetStringForDisplay(LetterForm.Medial);
            var FinalChar = letterData.GetStringForDisplay(LetterForm.Final);

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
    }
}