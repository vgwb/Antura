using Antura.Database;

namespace Antura.Teacher
{
    // TODO refactor: remove references to Arabic

    /// <summary>
    /// Parameters used by a QuestionBuilder to filter what letters can be selected.
    /// </summary>
    [System.Serializable]
    public class LetterFilters
    {
        public ExcludeLetterVariations excludeLetterVariations;
        public ExcludeDiacritics excludeDiacritics;
        public bool requireDiacritics;
        public bool excludeDiphthongs;
        public bool includeAccentedLetters;
        public bool includeSpecialCharacters;
        public bool excludeMultiCharacterLetters;

        public enum ExcludeLetterVariations
        {
            None,
            All,
            AllButAlefHamza
        }

        public enum ExcludeDiacritics
        {
            None,
            All,
            AllButMain
        }

        public LetterFilters(
            bool requireDiacritics = false,
            bool excludeDiphthongs = false,
            ExcludeDiacritics excludeDiacritics = ExcludeDiacritics.None,
            ExcludeLetterVariations excludeLetterVariations = ExcludeLetterVariations.None,
            bool includeAccentedLetters = false,
            bool includeSpecialCharacters = false,
            bool excludeMultiCharacterLetters = false
            )
        {
            this.excludeDiacritics = excludeDiacritics;
            this.excludeLetterVariations = excludeLetterVariations;
            this.requireDiacritics = requireDiacritics;
            this.excludeDiphthongs = excludeDiphthongs;
            this.includeAccentedLetters = includeAccentedLetters;
            this.includeSpecialCharacters = includeSpecialCharacters;
            this.excludeMultiCharacterLetters = excludeMultiCharacterLetters;
        }
    }

    /// <summary>
    /// Parameters used by a QuestionBuilder to filter what words can be selected.
    /// </summary>
    public class WordFilters
    {
        public bool excludeDiacritics;
        public bool excludeLetterVariations;
        public bool requireDiacritics;
        public bool excludeArticles;
        public bool excludePluralDual;
        public bool requireDrawings;
        public bool excludeColorWords;
        public bool excludeDipthongs;
        public bool excludeDuplicateLetters;
        public bool excludeSpaces;
        public WordDataCategory[] allowedCategories;

        public WordFilters(
            bool excludeDiacritics = false,
            bool excludeLetterVariations = false,
            bool requireDiacritics = false,
            bool excludeArticles = false,
            bool excludePluralDual = false,
            bool requireDrawings = false,
            bool excludeColorWords = false,
            bool excludeDipthongs = false,
            bool excludeSpaces = false,
            WordDataCategory[] allowedCategories = null)
        {
            this.excludeDiacritics = excludeDiacritics;
            this.excludeLetterVariations = excludeLetterVariations;
            this.requireDiacritics = requireDiacritics;
            this.excludeArticles = excludeArticles;
            this.excludePluralDual = excludePluralDual;
            this.requireDrawings = requireDrawings;
            this.excludeColorWords = excludeColorWords;
            this.excludeDipthongs = excludeDipthongs;
            this.excludeSpaces = excludeSpaces;
            this.allowedCategories = allowedCategories;
        }
    }

    /// <summary>
    /// Parameters used by a QuestionBuilder to filter what phrases can be selected.
    /// </summary>
    public class PhraseFilters
    {
        public bool requireWords;
        public bool requireAnswers;
        public bool requireAnswersOrWords;
        public bool requireAtLeastTwoWords;
        public int maxLength;
        public int maxWords;

        public PhraseFilters(
            bool requireWords = false,
            bool requireAnswers = false,
            bool requireAnswersOrWords = false,
            bool requireAtLeastTwoWords = false, // @todo: this could be reworked with Phrase Categories so to create better filters, or allow filters to have a numeric value
            int maxLength = 0,
            int maxWords = 0
        )
        {
            this.requireWords = requireWords;
            this.requireAnswers = requireAnswers;
            this.requireAnswersOrWords = requireAnswersOrWords;
            this.requireAtLeastTwoWords = requireAtLeastTwoWords;
            this.maxLength = maxLength;
            this.maxWords = maxWords;
        }
    }
}
