using Antura.Core;
using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// 1 - Uuid: the unique player id
    /// 2 - app version(json app version + platform + device type (tablet/smartphone))
    /// 3 - player age(int) - player genre(string M/F)
    ///
    /// 4 - Journey Position(string Stage.LearningBlock.PlaySession)
    /// 5 - MiniGame(string code)
    ///
    /// - playtime(int seconds how long the gameplay)
    /// - launch type(from Journey or from Book)
    /// - end type(natural game end or forced exit)
    ///
    /// - difficulty(float from minigame config)
    /// - number of rounds(int from minigame config)
    /// - result(int 0,1,2,3 bones)
    ///
    /// - good answers(comma separated codes of vocabulary data)
    /// - wrong answers(comma separated codes of vocabulary data)
    /// - gameplay errors(say the lives in ColorTickle or anything not really related to Learning data)
    ///
    /// 10 - additional(json encoded additional parameters that we don't know now or custom specific per minigame)
    [System.Serializable]
    public class LogGamePlayData : IData
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
        /// Timestamp of creation of this entry.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// app version(json app version + platform + device type (tablet/smartphone))
        /// </summary>
        public string App { get; set; }

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
        public string MiniGameCode { get; set; }

        /// <summary>
        /// Stars obtained during play.
        /// Integer in the [0,3] range
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Play time for this minigame in seconds.
        /// </summary>
        public int PlayTime { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public LogGamePlayData() { }

        public LogGamePlayData(JourneyPosition journeyPosition, MiniGameCode miniGameCode, int stars, float playTime)
        {
            Stage = journeyPosition.Stage;
            LearningBlock = journeyPosition.LearningBlock;
            PlaySession = journeyPosition.PlaySession;
            Stars = stars;
            PlayTime = (int)playTime;
            MiniGameCode = miniGameCode.ToString();
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("T{1},(ST{2},LB{3},PS{4}),M{5},S{6},PT{7}",
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
