using System;

namespace Antura.Core
{
    /// <summary>
    /// Represents the position of the player in the learning journey.
    /// </summary>
    // TODO refactor: this being a class may create some pesky bugs. Make it a struct?
    // TODO refactor: merge JourneyPosition, JourneyHelper
    [System.Serializable]
    public class JourneyPosition
    {
        public int Stage = 1;
        public int LearningBlock = 1;
        public int PlaySession = 1;

        public static JourneyPosition InitialJourneyPosition = new JourneyPosition(1, 1, 1);

        public const int ASSESSMENT_PLAY_SESSION_INDEX = 100;
        public const int RECAP_PLAY_SESSION_INDEX = 200;
        public const int ENDGAME_PLAY_SESSION_INDEX = 500;

        public string Id => $"{Stage}.{LearningBlock}.{PlaySession}";

        public string LearningBlockID => $"{Stage}.{LearningBlock}";

        public JourneyPosition()
        {
            Stage = 1;
            LearningBlock = 1;
            PlaySession = 1;
        }

        public JourneyPosition(int _stage, int _lb, int _ps)
        {
            Stage = _stage;
            LearningBlock = _lb;
            PlaySession = _ps;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Antura.Core.JourneyPosition"/> class.
        /// </summary>
        /// <param name="journeyPositionId">identifier formatted like Stage.LearningBlock.PlaySession</param>
        public JourneyPosition(string journeyPositionId)
        {
            var splits = journeyPositionId.Split('.');
            Stage = int.Parse(splits[0]);
            LearningBlock = int.Parse(splits[1]);
            PlaySession = int.Parse(splits[2]);
        }

        public JourneyPosition(JourneyPosition newJourneyPosition)
        {
            Stage = newJourneyPosition.Stage;
            LearningBlock = newJourneyPosition.LearningBlock;
            PlaySession = newJourneyPosition.PlaySession;
        }

        public void SetPosition(int _stage, int _lb, int _ps)
        {
            Stage = _stage;
            LearningBlock = _lb;
            PlaySession = _ps;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // TODO refactor: this is used by part of the application to convert hourney to an ID for DB purposes. Make this more robust.
        public override string ToString()
        {
            return Id;
        }

        public string GetShortTitle()
        {
            return Stage + "-" + LearningBlock;
        }

        public string ToDisplayedString(bool withPlaySession = false)
        {
            var psCode = "";
            switch (PlaySession)
            {
                case 1:
                    psCode = "1";
                    break;
                case 2:
                    psCode = "2";
                    break;
                case ASSESSMENT_PLAY_SESSION_INDEX:
                    psCode = "*";
                    break;
                case RECAP_PLAY_SESSION_INDEX:
                    psCode = "R";
                    break;
                case ENDGAME_PLAY_SESSION_INDEX:
                    psCode = "E";
                    break;
            }

            if (withPlaySession)
            {
                return Stage + "-" + LearningBlock + "-" + psCode;
            }
            else
            {
                return Stage + "-" + LearningBlock;
            }
        }

        public override bool Equals(object obj)
        {
            var otherPos = (JourneyPosition)obj;
            return Stage == otherPos.Stage && LearningBlock == otherPos.LearningBlock && PlaySession == otherPos.PlaySession;
        }

        public bool IsMinor(JourneyPosition other)
        {
            if (Stage < other.Stage)
            {
                return true;
            }
            if (Stage <= other.Stage && LearningBlock < other.LearningBlock)
            {
                return true;
            }
            if (Stage <= other.Stage && LearningBlock <= other.LearningBlock && PlaySession < other.PlaySession)
            {
                return true;
            }
            return false;
        }

        public bool IsMinorOrEqual(JourneyPosition other)
        {
            return IsMinor(other) || Equals(other);
        }

        public bool IsGreaterOrEqual(JourneyPosition other)
        {
            return !IsMinor(other);
        }

        public bool IsAssessment()
        {
            return PlaySession == ASSESSMENT_PLAY_SESSION_INDEX;
        }

        public bool IsRecap()
        {
            return PlaySession == RECAP_PLAY_SESSION_INDEX;
        }

        public bool IsEndGame()
        {
            return PlaySession == ENDGAME_PLAY_SESSION_INDEX;
        }
    }
}
