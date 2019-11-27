using Antura.Core;
using Antura.Database;

namespace Antura.LivingLetters
{
    /// <summary>
    /// View of a LetterData shown as a single letter on a LivingLetter.
    /// </summary>
    // TODO refactor: rename to better indicate that this is a view
    public class LL_LetterData : ILivingLetterData
    {
        public LetterData Data;
        public LetterForm Form = LetterForm.Isolated; // TODO refactor: this is tied to the Arabic language

        public LivingLetterDataType DataType
        {
            get { return LivingLetterDataType.Letter; }
        }

        public string Id
        {
            get { return Data.Id; }
            set { Data = AppManager.I.DB.GetLetterDataById(value); }
        }

        // @note: this should be the only constructor for LL_LetterData
        public LL_LetterData(LetterData _data)
        {
            Data = _data;
            if (_data.ForcedLetterForm != LetterForm.None) Form = _data.ForcedLetterForm;
        }

        #region API

        public string TextForLivingLetter
        {
            get { return Data.GetStringForDisplay(Form); }
        }

        public string DrawingCharForLivingLetter
        {
            get { return null; }
        }

        public bool Equals(ILivingLetterData data)
        {
            LL_LetterData other = data as LL_LetterData;
            if (other == null) {
                return false;
            }

            return Data.IsSameLetterAs(other.Data, LetterEqualityStrictness.LetterOnly);
        }

        public override string ToString()
        {
            return "LL-" + Data;
        }

        #endregion
    }
}