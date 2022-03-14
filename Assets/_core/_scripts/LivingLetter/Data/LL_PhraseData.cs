using Antura.Helpers;
using Antura.Core;
using Antura.Language;

namespace Antura.LivingLetters
{
    /// <summary>
    /// View of a PhraseData shown as text on a LivingLetter.
    /// </summary>
    // TODO refactor: rename to better indicate that this is a view
    public class LL_PhraseData : ILivingLetterData
    {
        public Database.PhraseData Data;

        public LivingLetterDataType DataType
        {
            get { return LivingLetterDataType.Phrase; }
        }

        public string Id
        {
            get { return Data.Id; }
            set { Data = AppManager.I.DB.GetPhraseDataById(value); } // TODO refactor: inject the value, no reference to the DB
        }

        public LL_PhraseData(string _id) :
            this(_id, AppManager.I.DB.GetPhraseDataById(_id)) // TODO refactor: inject the value, no reference to the DB
        {
        }

        public LL_PhraseData(string _id, Database.PhraseData _data) : this(_data)
        {
        }

        public LL_PhraseData(Database.PhraseData _data)
        {
            Data = _data;
        }

        /// <summary>
        /// @note Not ready yet!
        /// Living Letter Phrase Text To Display.
        /// </summary>
        public string TextForLivingLetter
        {
            get { return LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(Data.Text); }
        }

        public string DrawingCharForLivingLetter
        {
            get { return null; }
        }

        public bool Equals(ILivingLetterData data)
        {
            LL_PhraseData other = data as LL_PhraseData;
            if (other == null)
            {
                return false;
            }

            return other.Data.Id == Data.Id;
        }

        public override string ToString()
        {
            return "LL-" + Data.ToString();
        }
    }
}
