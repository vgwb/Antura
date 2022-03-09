using Antura.Core;
using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Score (in stars) relative to a journey element or a minigame. Updated at runtime.
    /// </summary>
    [System.Serializable]
    public class JourneyScoreData : IData, IScoreData, IDataEditable
    {
        public bool IsMiniGamePS =>
            PlaySession != JourneyPosition.ASSESSMENT_PLAY_SESSION_INDEX
            && PlaySession != JourneyPosition.ENDGAME_PLAY_SESSION_INDEX;

        /// <summary>
        /// Primary key for the database.
        /// Set based on ElementId and JourneyDataType
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier for the player. empty during game. compiled at export/import
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Type of journey data recorded.
        /// </summary>
        public JourneyDataType JourneyDataType { get; set; }

        /// <summary>
        /// Id of the element for which the score has been recorded.
        /// This is related to the primary key of the Static table for the related JourneyDataType.
        /// </summary>
        public string ElementId { get; set; }

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
        /// Stars obtained during this play session.
        /// Integer in the [0,3] range
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Timestamp of the last update of this entry.
        /// </summary>
        public int UpdateTimestamp { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public JourneyScoreData() { }

        public JourneyScoreData(string elementId, JourneyDataType dataType, int stars) : this(elementId, dataType, stars, GenericHelper.GetTimestampForNow())
        {
        }

        public JourneyScoreData(string elementId, JourneyDataType dataType, int stars, int timestamp)
        {
            ElementId = elementId;
            JourneyDataType = dataType;
            Id = JourneyDataType + "." + ElementId;

            var jp = new JourneyPosition(elementId);
            Stage = jp.Stage;
            LearningBlock = jp.LearningBlock;
            PlaySession = jp.PlaySession;

            Stars = stars;
            UpdateTimestamp = timestamp;
        }

        public float GetScore()
        {
            return Stars;
        }

        public string GetId()
        {
            return Id;
        }

        public void SetId(string _Id)
        {
            Id = _Id;
        }

        public override string ToString()
        {
            return string.Format("T{0},E{1},S{2},T{3}",
                JourneyDataType,
                ElementId,
                Stars,
                UpdateTimestamp
                );
        }

    }
}
