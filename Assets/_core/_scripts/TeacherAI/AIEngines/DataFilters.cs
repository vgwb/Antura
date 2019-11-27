using Antura.Database;

namespace Antura.Teacher
{
    // TODO refactor: remove references to Arabic

    /// <summary>
    /// Parameters used by a QuestionBuilder to filter what letters can be selected.
    /// </summary>
    public class LetterFilters
    {
        public ExcludeLetterVariations excludeLetterVariations;
        public ExcludeDiacritics excludeDiacritics;
        public bool requireDiacritics;
        public bool excludeDiphthongs;

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
            ExcludeLetterVariations excludeLetterVariations = ExcludeLetterVariations.None
            )
        {
            this.excludeDiacritics = excludeDiacritics;
            this.excludeLetterVariations = excludeLetterVariations;
            this.requireDiacritics = requireDiacritics;
            this.excludeDiphthongs = excludeDiphthongs;
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
            this.allowedCategories = allowedCategories;
        }
    }

    /// <summary>
    /// Parameters used by a QuestionBuilder to filter what phrases can be selected.
    /// </summary>
    public class PhraseFilters
    {
        public bool requireWords;
        public bool requireAnswersOrWords;
        public bool requireAtLeastTwoWords;
        public int maxLength;
        public int maxWords;

        public PhraseFilters(
            bool requireWords = false,
            bool requireAnswersOrWords = false,
            bool requireAtLeastTwoWords = false, // @todo: this could be reworked with Phrase Categories so to create better filters, or allow filters to have a numeric value
            int maxLength = 0,
            int maxWords = 0
        )
        {
            this.requireWords = requireWords;
            this.requireAnswersOrWords = requireAnswersOrWords;
            this.requireAtLeastTwoWords = requireAtLeastTwoWords;
            this.maxLength = maxLength;
            this.maxWords = maxWords;
        }
    }
}