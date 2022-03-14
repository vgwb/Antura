namespace Antura.Minigames.ReadingGame
{
    public struct ReadingBarWord
    {
        public string word;

        public int barId;
        // Relative 0 -> 1
        public float start;
        public float end;

        public ReadingBarWord(string word, int barId, float start, float end)
        {
            this.barId = barId;
            this.word = word;
            this.start = start;
            this.end = end;
        }
    }
}
