using Antura.Core;
using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Learning achievements obtained at a given timestamp. Logged at runtime.
    /// </summary>
    [System.Serializable]
    public class LogVocabularyScoreData : IData
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
        /// Type of vocabulary data recorded.
        /// </summary>
        public VocabularyDataType VocabularyDataType { get; set; }

        /// <summary>
        /// Id of the element for which the score has been recorded.
        /// This is related to the primary key of the Static table for the related VocabularyDataType.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// Score obtained for this element.
        /// Floating point value in the [-1,1] range.
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public LogVocabularyScoreData() { }

        public LogVocabularyScoreData(int appSession, JourneyPosition journeyPosition, MiniGameCode miniGameCode, VocabularyDataType dataType, string elementId, float score)
        {
            AppSession = appSession;
            Stage = journeyPosition.Stage;
            LearningBlock = journeyPosition.LearningBlock;
            PlaySession = journeyPosition.PlaySession;
            MiniGameCode = miniGameCode;
            VocabularyDataType = dataType;
            ElementId = elementId;
            Score = score;
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("S{0},T{1},PS{2},MG{3},VDT{4},E{5},S{6}",
                AppSession,
                Timestamp,
                Stage,
                MiniGameCode,
                VocabularyDataType,
                ElementId,
                Score
                );
        }

    }
}
