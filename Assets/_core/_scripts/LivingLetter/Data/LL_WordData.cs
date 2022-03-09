using Antura.Helpers;
using Antura.Core;
using Antura.Database;
using Antura.Language;

namespace Antura.LivingLetters
{
    /// <summary>
    /// View of a PhraseData shown as text on a LivingLetter.
    /// </summary>
    // TODO refactor: rename to better indicate that this is a view
    public class LL_WordData : ILivingLetterData
    {
        public Database.WordData Data;

        public LivingLetterDataType DataType
        {
            get { return LivingLetterDataType.Word; }
        }

        public string Id
        {
            get { return Data.Id; }
            set { Data = AppManager.I.DB.GetWordDataById(value); } // TODO refactor: inject the value, no reference to the DB
        }

        public LL_WordData(string _id) :
            this(AppManager.I.DB.GetWordDataById(_id)) // TODO refactor: inject the value, no reference to the DB
        {
        }

        public LL_WordData(string _id, Database.WordData _data) : this(_data)
        {
        }

        public LL_WordData(Database.WordData _data)
        {
            Data = _data;
        }

        /// <summary>
        /// Living Letter Text To Display.
        /// </summary>
        public string TextForLivingLetter
        {
            get
            {
                return LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(Data.Text);
            }
        }

        public string DrawingCharForLivingLetter
        {
            get { return AppManager.I.VocabularyHelper.GetWordDrawing(Data); } // TODO refactor: inject the value, no reference to the DB
        }

        public bool Equals(ILivingLetterData data)
        {
            LL_WordData other = data as LL_WordData;
            if (other == null)
            {
                return false;
            }

            return Data.IsSameAs(other.Data, WordEqualityStrictness.Spelling);
        }

        public override string ToString()
        {
            return "LL-" + Data.ToString();
        }
    }
}
