using Antura.Database;

namespace Antura.Map
{
    public class PlaySessionState
    {
        public PlaySessionData psData;
        public JourneyScoreData scoreData;

        public PlaySessionState(PlaySessionData _psData, JourneyScoreData _scoreData)
        {
            this.psData = _psData;
            this.scoreData = _scoreData;
        }
    }
}