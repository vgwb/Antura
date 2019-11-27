using UnityEngine;

namespace Antura.LivingLetters.Sample
{
    public class StringTestData : ILivingLetterData
    {
        string text;

        public StringTestData(string text)
        {
            this.text = text;
        }

        public LivingLetterDataType DataType
        {
            get { return LivingLetterDataType.Word; }
        }

        public string DrawingCharForLivingLetter
        {
            get { return null; }
        }

        public Sprite DrawForLivingLetter
        {
            get { return null; }
        }

        public string Id { get; set; }

        public string TextForLivingLetter
        {
            get { return text; }
        }

        public bool Equals(ILivingLetterData data)
        {
            StringTestData other = data as StringTestData;
            if (other == null)
                return false;

            return other.text == text;
        }
    }
}