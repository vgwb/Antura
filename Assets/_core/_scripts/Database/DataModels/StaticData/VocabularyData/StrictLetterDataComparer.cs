using System.Collections.Generic;

namespace Antura.Database
{
    /// <summary>
    /// Comparer used to compare two letter data using non-default strictness
    /// </summary>
    public class StrictLetterDataComparer : IEqualityComparer<LetterData>
    {
        private LetterEqualityStrictness comparisonStrictness;

        public StrictLetterDataComparer(LetterEqualityStrictness comparisonStrictness)
        {
            this.comparisonStrictness = comparisonStrictness;
        }

        public bool Equals(LetterData x, LetterData y)
        {
            return x.IsSameLetterAs(y, comparisonStrictness);
        }

        public int GetHashCode(LetterData obj)
        {
            var hashCode = (obj.Id != null ? obj.Id.GetHashCode() : 0);
            switch (comparisonStrictness)
            {
                case LetterEqualityStrictness.LetterOnly:
                    break;
                case LetterEqualityStrictness.WithActualForm:
                    hashCode = (hashCode * 397) ^ obj.Form.GetHashCode();
                    break;
                case LetterEqualityStrictness.WithVisualForm:
                    hashCode = (hashCode * 397) ^ obj.GetStringForDisplay().GetHashCode();
                    break;
            }
            return hashCode;
        }
    }
}