using Antura.Core;
using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Minigame score obtained at a given timestamp. Logged at runtime.
    /// </summary>
    [System.Serializable]
    public class LogMiniGameScoreData : IData
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the player. empty during game. compiled at export/import
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Identifier of the application session.
        /// </summary>
        public int AppSession { get; set; }

        /// <summary>
        /// Timestamp of creation of this entry.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Stage of the journey position at which this score was recorded.
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// LearningBlock of the journey position at which this score was recorded.
        /// </summary>
        public int LearningBlock { get; set; }

        /// <summary>
        /// PlaySession of the journey position at which this score was recorded.
        /// </summary>
        public int PlaySession { get; set; }

        /// <summary>
        /// MiniGame during which this score was recorded.
        /// </summary>
        public MiniGameCode MiniGameCode { get; set; }

        /// <summary>
        /// Stars obtained during play.
        /// Integer in the [0,3] range
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Play time for this minigame in seconds.
        /// </summary>
        public float PlayTime { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public LogMiniGameScoreData() { }

        public LogMiniGameScoreData(int appSession, JourneyPosition journeyPosition, MiniGameCode miniGameCode, int stars, float playTime)
        {
            AppSession = appSession;
            Stage = journeyPosition.Stage;
            LearningBlock = journeyPosition.LearningBlock;
            PlaySession = journeyPosition.PlaySession;
            Stars = stars;
            PlayTime = playTime;
            MiniGameCode = miniGameCode;
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("S{0},T{1},(ST{2},LB{3},PS{4}),M{5},S{6},PT{7}",
                AppSession,
                Timestamp,
                Stage,
                LearningBlock,
                PlaySession,
                MiniGameCode,
                Stars,
                PlayTime
                );
        }
    }
}
