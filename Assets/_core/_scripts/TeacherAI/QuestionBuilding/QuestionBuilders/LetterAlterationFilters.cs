namespace Antura.Teacher
{
    public class LetterAlterationFilters
    {
        public static readonly LetterAlterationFilters FormsOfSingleLetter = new LetterAlterationFilters()
        {
            includeForms = true
        };
        public static readonly LetterAlterationFilters VisualFormsOfSingleLetter = new LetterAlterationFilters()
        {
            includeForms = true,
            visuallyDifferentForms = true
        };
        public static readonly LetterAlterationFilters MultipleLetters = new LetterAlterationFilters()
        {
            differentBaseLetters = true
        };
        public static readonly LetterAlterationFilters PhonemesOfSingleLetter = new LetterAlterationFilters()
        {
            ExcludeDiacritics = LetterFilters.ExcludeDiacritics.None,
            ExcludeLetterVariations = LetterFilters.ExcludeLetterVariations.None,
            excludeDipthongs = false
        };
        public static readonly LetterAlterationFilters FormsOfMultipleLetters = new LetterAlterationFilters()
        {
            differentBaseLetters = true,
            includeForms = true
        };
        public static readonly LetterAlterationFilters PhonemesOfMultipleLetters = new LetterAlterationFilters()
        {
            differentBaseLetters = true,
            ExcludeDiacritics = LetterFilters.ExcludeDiacritics.None,
            ExcludeLetterVariations = LetterFilters.ExcludeLetterVariations.None,
            excludeDipthongs = false
        };
        public static readonly LetterAlterationFilters DiacriticsOfMultipleLetters = new LetterAlterationFilters()
        {
            differentBaseLetters = true,
            ExcludeDiacritics = LetterFilters.ExcludeDiacritics.None,
            ExcludeLetterVariations = LetterFilters.ExcludeLetterVariations.None,
            excludeDipthongs = false,
            requireDiacritics = true,
        };
        public static readonly LetterAlterationFilters FormsAndPhonemesOfMultipleLetters = new LetterAlterationFilters()
        {
            differentBaseLetters = true,
            includeForms = true,
            ExcludeDiacritics = LetterFilters.ExcludeDiacritics.None,
            ExcludeLetterVariations = LetterFilters.ExcludeLetterVariations.None,
            excludeDipthongs = true, // dipthongs are excluded since these alterations are used in LetterAny games, where you need to hear the correct letter and distinguish it
        };
        public static readonly LetterAlterationFilters FormsAndPhonemesOfMultipleLetters_OneForm = new LetterAlterationFilters()
        {
            differentBaseLetters = true,
            includeForms = true,
            oneFormPerLetter = true,
            ExcludeDiacritics = LetterFilters.ExcludeDiacritics.None,
            ExcludeLetterVariations = LetterFilters.ExcludeLetterVariations.None,
            excludeDipthongs = true, // dipthongs are excluded since these alterations are used in LetterPhoneme games, where you need to hear the correct letter and distinguish it
        };


        // Can add different letters as bases?
        public bool differentBaseLetters;

        // Can add the various variations on bases?
        public LetterFilters.ExcludeDiacritics ExcludeDiacritics;
        public LetterFilters.ExcludeLetterVariations ExcludeLetterVariations;
        public bool excludeDipthongs;
        public bool requireDiacritics;

        // Can add forms?
        public bool includeForms;
        public bool oneFormPerLetter;    // If true, letters will appear only in one form
        public bool visuallyDifferentForms;    // If true, forms that appear the same won't be selected as different options

        public LetterAlterationFilters() : this(false, LetterFilters.ExcludeDiacritics.All, LetterFilters.ExcludeLetterVariations.All, true, false)
        {

        }

        public LetterAlterationFilters(bool differentBaseLetters, LetterFilters.ExcludeDiacritics excludeDiacritics, LetterFilters.ExcludeLetterVariations excludeLetterVariations, bool excludeDipthongs, bool includeForms)
        {
            //this.addBaseLetterToo = addBaseLetterToo;
            this.differentBaseLetters = differentBaseLetters;
            ExcludeDiacritics = excludeDiacritics;
            ExcludeLetterVariations = excludeLetterVariations;
            this.excludeDipthongs = excludeDipthongs;
            this.includeForms = includeForms;
        }
    }
}
