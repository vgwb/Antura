using Antura.LivingLetters;
using System;
using System.Collections.Generic;
using System.Globalization;
using Antura.Core;
using SQLite;
using UnityEngine;
using Antura.Language;

namespace Antura.Database
{
    public enum LetterKindCategory
    {
        Real = 0, // default: Base + Combo
        DiacriticCombo,
        Base,
        LetterVariation,
        Symbol,
        BaseAndVariations,
        AccentedLetter,
        SpecialChar
    }

    [Flags]
    public enum LetterForm
    {
        None = 0,
        Isolated = 1,
        Initial = 2,
        Medial = 4,
        Final = 8
    }

    /// <summary>
    /// Enumerator that defines how to treat equality for letters.
    /// </summary>
    public enum LetterEqualityStrictness
    {
        LetterBase,          // the same letter, regardless of whether it has accents or not
        Letter,             // the same letter, regardless of the form
        WithVisualForm,     // the same letter with the same form, or with different forms but with the same visual appearance
        WithActualForm,      // the same letter with the same form
    }


    /// <summary>
    ///     Data defining a Letter.
    ///     This is one of the fundamental dictionary (i.e. learning content) elements.
    ///     <seealso cref="PhraseData" />
    ///     <seealso cref="WordData" />
    /// </summary>
    // TODO refactor: we could make this general in respect to the language
    [Serializable]
    public class LetterData : IVocabularyData, IConvertibleToLivingLetterData
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

        public bool InBook
        {
            get { return _InBook; }
            set { _InBook = value; }
        }
        [SerializeField]
        private bool _InBook;

        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }
        [SerializeField]
        private int _Number;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        [SerializeField]
        private string _Title;

        public LetterDataKind Kind
        {
            get { return _Kind; }
            set { _Kind = value; }
        }
        [SerializeField]
        private LetterDataKind _Kind;

        public string BaseLetter
        {
            get { return _BaseLetter; }
            set { _BaseLetter = value; }
        }
        [SerializeField]
        private string _BaseLetter;

        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }
        [SerializeField]
        private string _Symbol;

        public LetterDataType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        [SerializeField]
        private LetterDataType _Type;

        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        [SerializeField]
        private string _Tag;

        public string Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }
        [SerializeField]
        private string _Notes;

        public LetterDataSunMoon SunMoon
        {
            get { return _SunMoon; }
            set { _SunMoon = value; }
        }
        [SerializeField]
        private LetterDataSunMoon _SunMoon;

        public string Sound
        {
            get { return _Sound; }
            set { _Sound = value; }
        }
        [SerializeField]
        private string _Sound;

        public string NameSound
        {
            get { return _NameSound; }
            set { _NameSound = value; }
        }
        [SerializeField]
        private string _NameSound;

        public string PhonemeSound
        {
            get { return _PhonemeSound; }
            set { _PhonemeSound = value; }
        }
        [SerializeField]
        private string _PhonemeSound;

        public string SoundZone
        {
            get { return _SoundZone; }
            set { _SoundZone = value; }
        }
        [SerializeField]
        private string _SoundZone;

        public string Isolated
        {
            get { return _Isolated; }
            set { _Isolated = value; }
        }
        [SerializeField]
        private string _Isolated;

        public string Initial
        {
            get { return _Initial; }
            set { _Initial = value; }
        }
        [SerializeField]
        private string _Initial;

        public string Medial
        {
            get { return _Medial; }
            set { _Medial = value; }
        }
        [SerializeField]
        private string _Medial;

        public string Final
        {
            get { return _Final; }
            set { _Final = value; }
        }
        [SerializeField]
        private string _Final;

        public string Isolated_Unicode
        {
            get { return _Isolated_Unicode; }
            set { _Isolated_Unicode = value; }
        }
        [SerializeField]
        private string _Isolated_Unicode;

        public string Initial_Unicode
        {
            get { return _Initial_Unicode; }
            set { _Initial_Unicode = value; }
        }
        [SerializeField]
        private string _Initial_Unicode;

        public string Medial_Unicode
        {
            get { return _Medial_Unicode; }
            set { _Medial_Unicode = value; }
        }
        [SerializeField]
        private string _Medial_Unicode;

        public string Final_Unicode
        {
            get { return _Final_Unicode; }
            set { _Final_Unicode = value; }
        }
        [SerializeField]
        private string _Final_Unicode;

        public string Symbol_Unicode
        {
            get { return _Symbol_Unicode; }
            set { _Symbol_Unicode = value; }
        }
        [SerializeField]
        private string _Symbol_Unicode;

        public string InitialFix
        {
            get { return _InitialFix; }
            set { _InitialFix = value; }
        }

        [SerializeField]
        private string _InitialFix;

        public string FinalFix
        {
            get { return _FinalFix; }
            set { _FinalFix = value; }
        }
        [SerializeField]
        private string _FinalFix;

        public string MedialFix
        {
            get { return _MedialFix; }
            set { _MedialFix = value; }
        }
        [SerializeField]
        private string _MedialFix;

        public bool HasDot
        {
            get { return _HasDot; }
            set { _HasDot = value; }
        }
        [SerializeField]
        private bool _HasDot;

        public bool HasDiacritic
        {
            get { return _HasDiacritic; }
            set { _HasDiacritic = value; }
        }
        [SerializeField]
        private bool _HasDiacritic;

        public bool HasAccent
        {
            get { return _HasAccent; }
            set { _HasAccent = value; }
        }
        [SerializeField]
        private bool _HasAccent;

        public int Orientations
        {
            get { return _Orientations; }
            set { _Orientations = value; }
        }
        [SerializeField]
        private int _Orientations;


        public float Complexity
        {
            get { return _Complexity; }
            set { _Complexity = value; }
        }
        [SerializeField]
        private float _Complexity;

        public string[] LinkedWords
        {
            get { return _LinkedWords; }
            set { _LinkedWords = value; }
        }
        [SerializeField]
        private string[] _LinkedWords;

        #region Letter Forms Handling (temporary data)

        /// <summary>
        /// If set, this LetterData should be represented using the forced form.
        /// </summary>
        public LetterForm ForcedLetterForm = LetterForm.None;
        public LetterForm Form
        {
            get {
                if (ForcedLetterForm != LetterForm.None) { return ForcedLetterForm; }
                return LetterForm.Isolated;
            }
        }

        public LetterData Base => AppManager.I.VocabularyHelper.GetAccentedBase(Id);

        #endregion

        public override string ToString()
        {
            string s = "(" + Isolated + ")";
            if (ForcedLetterForm != LetterForm.None) { s += " F-" + ForcedLetterForm; }
            s += " " + Id;
            return s;
        }

        public float GetIntrinsicDifficulty()
        {
            return Complexity;
        }

        public string GetId()
        {
            return Id;
        }


        public bool IsOfKindCategory(LetterKindCategory category)
        {
            var isIt = false;
            switch (category) {
                case LetterKindCategory.Base:
                    isIt = IsBaseLetter();
                    break;
                case LetterKindCategory.LetterVariation:
                    isIt = IsVariationLetter();
                    break;
                case LetterKindCategory.Symbol:
                    isIt = IsSymbolLetter();
                    break;
                case LetterKindCategory.DiacriticCombo:
                    isIt = IsDiacriticComboLetter();
                    break;
                case LetterKindCategory.Real:
                    isIt = IsRealLetter();
                    break;
                case LetterKindCategory.BaseAndVariations:
                    isIt = IsBaseOrVariationLetter();
                    break;
                case LetterKindCategory.AccentedLetter:
                    isIt = IsAccentedLetter();
                    break;
                case LetterKindCategory.SpecialChar:
                    isIt = IsSpecialCharacter();
                    break;
            }
            return isIt;
        }

        private bool IsRealLetter()
        {
            return IsBaseLetter() || IsDiacriticComboLetter();
        }

        private bool IsBaseLetter()
        {
            return Kind == LetterDataKind.Letter;
        }

        private bool IsVariationLetter()
        {
            return Kind == LetterDataKind.LetterVariation;
        }

        private bool IsSymbolLetter()
        {
            return Kind == LetterDataKind.Symbol;
        }

        private bool IsDiacriticComboLetter()
        {
            return Kind == LetterDataKind.DiacriticCombo;
        }

        private bool IsBaseOrVariationLetter()
        {
            return Kind == LetterDataKind.Letter || Kind == LetterDataKind.LetterVariation;
        }
        private bool IsAccentedLetter()
        {
            return Kind == LetterDataKind.AccentedLetter;
        }
        private bool IsSpecialCharacter()
        {
            return Kind == LetterDataKind.SpecialChar;
        }

        public ILivingLetterData ConvertToLivingLetterData()
        {
            return new LL_LetterData(this);
        }

        public string GetUnicode(LetterForm form = LetterForm.Isolated, bool fallback = true)
        {
            switch (Kind) {
                case LetterDataKind.Symbol:
                    return Isolated_Unicode;
                default:
                    switch (form) {
                        case LetterForm.Initial:
                            return Initial_Unicode != "" ? Initial_Unicode : (fallback ? Isolated_Unicode : "");
                        case LetterForm.Medial:
                            return Medial_Unicode != "" ? Medial_Unicode : (fallback ? Isolated_Unicode : "");
                        case LetterForm.Final:
                            return Final_Unicode != "" ? Final_Unicode : (fallback ? Isolated_Unicode : "");
                        default:
                            return Isolated_Unicode;
                    }
            }
        }

        public string GetAudioFilename(LetterDataSoundType soundType = LetterDataSoundType.Phoneme)
        {
            // Debug.Log("GetAudioFilename " + Id + " " + Kind + " " + Type);
            switch (soundType) {
                case LetterDataSoundType.Phoneme:
                    if (PhonemeSound != "") {
                        return PhonemeSound;
                    } else {
                        Debug.LogWarning("LetterData(): Requested Phone Sound is missing " + Id);
                        return "";
                    }
                case LetterDataSoundType.Name:
                    if (NameSound != "") {
                        return NameSound; ;
                    } else {
                        Debug.LogWarning("LetterData(): Requested Name Sound is missing " + Id);
                        return "";
                    }
                default:
                    return "";
            }
        }
        public string GetStringForDisplay(LetterForm form = LetterForm.Isolated, bool forceShowAccent = false)
        {
            // Accented letters are always shown as non-accented
            if (!forceShowAccent && !AppManager.I.Edition.ShowAccents && HasAccent)
            {
                return Base.GetStringForDisplay(form);
            }

            // Get the string for the specific form, without fallback
            var hexunicode = GetUnicode(form, fallback: false);
            if (hexunicode == "") {
                return "";
            }

            var output = "";

            // add the "-" to diacritic symbols to indentify better if it's over or below hte mid line
            if (Type == LetterDataType.DiacriticSymbol) {
                output = "\u0640";
            }

            var unicode = int.Parse(hexunicode, NumberStyles.HexNumber);
            output += ((char)unicode).ToString();

            if (Symbol_Unicode != "") {
                var unicode_added = int.Parse(Symbol_Unicode, NumberStyles.HexNumber);
                output += ((char)unicode_added).ToString();
            }

            // add a "-" before medial and final single letters where needed
            if (form == LetterForm.Final && FinalFix != "" || form == LetterForm.Medial && MedialFix != "") {
                output = "\u0640" + output;
            }

            if (form == LetterForm.Initial && InitialFix != "" || form == LetterForm.Medial && InitialFix != "") {
                output = output + "\u0640";
            }

            return output;
        }

        public IEnumerable<LetterForm> GetAvailableForms()
        {
            if (Isolated_Unicode != "") {
                yield return LetterForm.Isolated;
            }

            if (Initial_Unicode != "") {
                yield return LetterForm.Initial;
            }

            if (Medial_Unicode != "") {
                yield return LetterForm.Medial;
            }

            if (Final_Unicode != "") {
                yield return LetterForm.Final;
            }
        }

        public LetterData Clone()
        {
            return (LetterData)MemberwiseClone();
        }

        public bool IsSameLetterAs(LetterData other, LetterEqualityStrictness strictness)
        {
            bool isEqual = false;
            switch (strictness)
            {
                case LetterEqualityStrictness.LetterBase:
                    isEqual = string.Equals(Base.Id, other.Base.Id);
                    break;
                case LetterEqualityStrictness.Letter:
                    isEqual = string.Equals(_Id, other._Id);
                    break;
                case LetterEqualityStrictness.WithActualForm:
                    isEqual = string.Equals(_Id, other._Id) && Form == other.Form;
                    break;
                case LetterEqualityStrictness.WithVisualForm:
                    isEqual = string.Equals(_Id, other._Id) && FormsLookTheSame(Form, other.Form);
                    break;
            }
            return isEqual;
        }


        private bool FormsLookTheSame(LetterForm form1, LetterForm form2)
        {
            //Debug.Log("form1: " + GetStringForDisplay(form1));
            //Debug.Log("form2: " + GetStringForDisplay(form2));
            return GetStringForDisplay(form1) == GetStringForDisplay(form2);
        }

        public override bool Equals(object obj)
        {
            var other = obj as LetterData;
            if (other == null) { return false; }
            return Equals(other);
        }

        private bool Equals(LetterData other)
        {
            // By default, LetterData uses Letter when comparing (even in collections!)
            return IsSameLetterAs(other, LetterEqualityStrictness.Letter);
        }

        public override int GetHashCode()
        {
            // By default, LetterData uses LetterOnly when comparing (even in collections!)
            var hashCode = (Id != null ? Id.GetHashCode() : 0);
            return hashCode;
        }

        public string GetDebugDiacriticFix()
        {
            string output = "";
            if (Symbol_Unicode != "") {
                output = "// " + Id + "\n";
                output += LanguageSwitcher.I.GetHelper(LanguageUse.Learning).DebugShowDiacriticFix(Isolated_Unicode, Symbol_Unicode) + "\n";
                if (Initial_Unicode != Isolated_Unicode) {
                    output += LanguageSwitcher.I.GetHelper(LanguageUse.Learning).DebugShowDiacriticFix(Initial_Unicode, Symbol_Unicode) + "\n";
                }
                if (Medial_Unicode != Isolated_Unicode && Medial_Unicode != Initial_Unicode) {
                    output += LanguageSwitcher.I.GetHelper(LanguageUse.Learning).DebugShowDiacriticFix(Medial_Unicode, Symbol_Unicode) + "\n";
                }
                if (Final_Unicode != Medial_Unicode && Final_Unicode != Initial_Unicode && Final_Unicode != Isolated_Unicode) {
                    output += LanguageSwitcher.I.GetHelper(LanguageUse.Learning).DebugShowDiacriticFix(Final_Unicode, Symbol_Unicode) + "\n";
                }
                //Debug.Log(output);
            }
            return output;
        }
    }
}