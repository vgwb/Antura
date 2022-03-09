namespace Antura.Minigames.ReadingGame
{
    public class KaraokeSegment
    {
        public string text;
        public float start;
        public float end;
        public bool starsWithLineBreak;

        public float Length { get { return end - start; } }

        public KaraokeSegment()
        {
            text = "";
            start = 0;
            end = 0;
            starsWithLineBreak = false;
        }

        public KaraokeSegment(string text, float start, float end, bool starsWithLineBreak)
        {
            this.text = text;
            this.start = start;
            this.end = end;
            this.starsWithLineBreak = starsWithLineBreak;
        }
    }
}
