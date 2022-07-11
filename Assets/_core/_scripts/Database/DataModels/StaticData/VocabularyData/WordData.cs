using System;
using System.Linq;
using Antura.Core;
using Antura.LivingLetters;
using Antura.Helpers;
using SQLite;
using UnityEngine;

namespace Antura.Database
{

    /// <summary>
    /// Enumerator that defines if two words are identical.
    /// </summary>
    public enum WordEqualityStrictness
    {
        Word,               // words are the same if they have the same database ID (i.e. they are the actual same word)
        Spelling,             // words are the same if they are spelled the same
    }

    /// <summary>
    /// Data defining a Word.
    /// This is one of the fundamental dictionary (i.e. learning content) elements.
    /// <seealso cref="PhraseData"/>
    /// <seealso cref="LetterData"/>
    /// </summary>
    [Serializable]
    public class WordData : IVocabularyData, IConvertibleToLivingLetterData
    {
        [PrimaryKey]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [SerializeField]
        private string _Id;

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        [SerializeField]
        private bool _Active;

        public WordDataKind Kind
        {
            get { return _Kind; }
            set { _Kind = value; }
        }
        [SerializeField]
        private WordDataKind _Kind;

        public WordDataCategory Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        [SerializeField]
        private WordDataCategory _Category;

        public WordDataForm Form
        {
            get { return _Form; }
            set { _Form = value; }
        }
        [SerializeField]
        private WordDataForm _Form;

        public WordDataArticle Article
        {
            get { return _Article; }
            set { _Article = value; }
        }
        [SerializeField]
        private WordDataArticle _Article;

        public VocabularyDataGender Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        [SerializeField]
        private VocabularyDataGender _Gender;

        public string LinkedWord
        {
            get { return _LinkedWord; }
            set { _LinkedWord = value; }
        }
        [SerializeField]
        private string _LinkedWord;

        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }
        [SerializeField]
        private string _Text;

        /// <summary>
        /// the contextual value of this word (for example "green" for the green color
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        [SerializeField]
        private string _Value;

        public string SortValue
        {
            get { return _SortValue; }
            set { _SortValue = value; }
        }
        [SerializeField]
        private string _SortValue;

        /*
        [Ignore]
        public string[] Letters
        {
            get { return _Letters; }
            set { _Letters = value; }
        }
        [SerializeField]
        private string[] _Letters;

        public string Letters_list
        {
            get { return _Letters.ToJoinedString(); }
            set { }
        }
          */

        public string DrawingId
        {
            get { return _DrawingId; }
            set { _DrawingId = value; }
        }
        [SerializeField]
        private string _DrawingId;

        public string DrawingLabel
        {
            get { return _DrawingLabel; }
            set { _DrawingLabel = value; }
        }
        [SerializeField]
        private string _DrawingLabel;

        public float Complexity
        {
            get { return _Complexity; }
            set { _Complexity = value; }
        }
        [SerializeField]
        private float _Complexity;

        public string[] PlaySessionLinks
        {
            get { return _PlaySessionLinks; }
            set { _PlaySessionLinks = value; }
        }
        [SerializeField]
        private string[] _PlaySessionLinks;

        //public LetterSymbol[] Symbols; //TODO

        public string GetId()
        {
            return Id;
        }

        public float GetIntrinsicDifficulty()
        {
            return Complexity;
        }

        public override string ToString()
        {
            string s = Id + ": " + Text;
            return s;
        }

        public ILivingLetterData ConvertToLivingLetterData()
        {
            return new LL_WordData(GetId(), this);
        }

        public bool HasDrawing()
        {
            return DrawingId != "";
        }

        public bool IsSameAs(WordData other, WordEqualityStrictness strictness)
        {
            bool isEqual = false;
            switch (strictness)
            {
                case WordEqualityStrictness.Word:
                    isEqual = string.Equals(Id, other.Id);
                    break;
                case WordEqualityStrictness.Spelling:
                    isEqual = string.Equals(Text, other.Text);
                    break;
            }

            return isEqual;
        }

        public override bool Equals(object obj)
        {
            var other = obj as WordData;
            if (other == null)
            { return false; }
            return Equals(other);
        }

        private static WordEqualityStrictness DefaultStrictness = WordEqualityStrictness.Spelling;
        public override int GetHashCode()
        {
            switch (DefaultStrictness)
            {
                case WordEqualityStrictness.Word:
                    return (_Id != null ? _Id.GetHashCode() : 0);
                case WordEqualityStrictness.Spelling:
                    return (_Text != null ? _Text.GetHashCode() : 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool Equals(WordData other)
        {
            return IsSameAs(other, DefaultStrictness);
        }

        public DrawingData GetDrawingData()
        {
            return AppManager.I.AppEdition.DrawingsData.Drawings.Find(x => x.Id == DrawingId);
        }

        public string GetDrawingColor()
        {
            return GetDrawingData().Value;
        }

    }
}
