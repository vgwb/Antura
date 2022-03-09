using Antura.LivingLetters;

namespace Antura.Assessment
{
    public static class ElementsSize
    {
        public static readonly float LL = 3.0f;
        public static readonly float DropZoneScale = 2.3f;

        public static float Get(LivingLetterDataType dataType)
        {
            switch (dataType)
            {
                case LivingLetterDataType.Word:
                    return 1.3f;
                case LivingLetterDataType.Phrase:
                    return 3.5f;
                default:
                    return 1f;
            }
        }
    }
}
