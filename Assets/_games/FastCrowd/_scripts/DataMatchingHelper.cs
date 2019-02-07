using Antura.Database;
using Antura.LivingLetters;

namespace Antura.Database
{
    public static class DataMatchingHelper
    {
        public static bool IsDataMatching(ILivingLetterData ll1, ILivingLetterData ll2, LetterEqualityStrictness letterEqualityStrictness)
        {
            if (ll1 is LL_LetterData && ll2 is LL_LetterData)
            {
                var letter1 = ll1 as LL_LetterData;
                var letter2 = ll2 as LL_LetterData;

                //UnityEngine.Debug.Log("Matching letters " + letter1.Data + " and " + letter2.Data);
                return letter1.Data.IsSameLetterAs(letter2.Data, strictness: letterEqualityStrictness);
            }
            else if ((ll1 is LL_WordData || ll1 is LL_ImageData) && (ll2 is LL_WordData || ll2 is LL_ImageData))
            {
                return ll1.Id == ll2.Id;
            }
            else if (ll1 is LL_PhraseData && ll2 is LL_PhraseData)
            {
                var phrase1 = ll1 as LL_PhraseData;
                var phrase2 = ll2 as LL_PhraseData;
                return phrase1.Data.Id == phrase2.Data.Id;
            }
            return false;
        }
    }

}
