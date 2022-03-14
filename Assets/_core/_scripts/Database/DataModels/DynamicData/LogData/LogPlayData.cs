using Antura.Core;
using Antura.Helpers;
using SQLite;

namespace Antura.Database
{

    /// <summary>
    /// Play-related measurements obtained at a given timestamp. Logged at runtime.
    /// </summary>
    [System.Serializable]
    public class LogPlayData : IData
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
        /// Stage at which the play data has been recorded.
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// LearningBlock at which the play data has been recorded.
        /// </summary>
        public int LearningBlock { get; set; }

        /// <summary>
        /// PlaySession at which the play data has been recorded.
        /// </summary>
        public int PlaySession { get; set; }

        /// <summary>
        /// MiniGame during which the play data has been recorded.
        /// </summary>
        public MiniGameCode MiniGameCode { get; set; }

        /// <summary>
        /// Event recorded.
        /// </summary>
        public PlayEvent PlayEvent { get; set; }

        /// <summary>
        /// Related skill.
        /// </summary>
        public PlaySkill PlaySkill { get; set; }

        /// <summary>
        /// Score value related to the play skill. [0,1]
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// Additional raw JSON data saved alongside the event to record more details.
        /// Example: "{playerId:0, rewardType:2}"
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public LogPlayData() { }

        public LogPlayData(int appSession, JourneyPosition journeyPosition, MiniGameCode miniGameCode, PlayEvent playEvent, PlaySkill playSkill, float score)
            : this(appSession, journeyPosition, miniGameCode, playEvent, playSkill, score, "")
        {
        }

        public LogPlayData(int appSession, JourneyPosition journeyPosition, MiniGameCode miniGameCode, PlayEvent playEvent, PlaySkill playSkill, float score, string additionalData)
        {
            AppSession = appSession;
            Stage = journeyPosition.Stage;
            LearningBlock = journeyPosition.LearningBlock;
            PlaySession = journeyPosition.PlaySession;
            MiniGameCode = miniGameCode;
            PlayEvent = playEvent;
            PlaySkill = playSkill;
            Score = score;
            AdditionalData = additionalData;
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("AS{0},T{1},PS{2},MG{3},PE{4},SK{5},S{6},RD{7}",
                AppSession,
                Timestamp,
                Stage,
                MiniGameCode,
                PlayEvent,
                PlaySkill,
                Score,
                AdditionalData
                );
        }

    }
}
