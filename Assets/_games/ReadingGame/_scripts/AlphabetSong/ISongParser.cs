using System.Collections.Generic;
using System.IO;

namespace Antura.Minigames.ReadingGame
{
    public interface ISongParser
    {
        List<KaraokeSegment> Parse(Stream stream);
    }
}
