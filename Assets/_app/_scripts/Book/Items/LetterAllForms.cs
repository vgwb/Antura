using Antura.Database;
using UnityEngine;

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
        }

    }
}