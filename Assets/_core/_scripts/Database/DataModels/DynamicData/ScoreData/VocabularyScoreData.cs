using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Summary score results relative to a vocabulary element. Updated at runtime.
    /// </summary>
    [System.Serializable]
    public class VocabularyScoreData : IData, IScoreData, IDataEditable
    {
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
        /// Has this element been unlocked in the Book?
        /// </summary>
        public bool Unlocked { get; set; }

        /// <summary>
        /// Timestamp of the last update of this entry.
        /// </summary>
        public int UpdateTimestamp { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public VocabularyScoreData() { }

        public VocabularyScoreData(string elementId, VocabularyDataType dataType, float score, bool unlocked) : this(elementId, dataType, score, unlocked, GenericHelper.GetTimestampForNow())
        {
        }

        public VocabularyScoreData(string elementId, VocabularyDataType dataType, float score, bool unlocked, int timestamp)
        {
            ElementId = elementId;
            VocabularyDataType = dataType;
            Id = VocabularyDataType + "." + ElementId;
            Score = score;
            Unlocked = unlocked;
            UpdateTimestamp = timestamp;
        }

        public float GetScore()
        {
            return Score;
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
            return string.Format("T{0},E{1},S{2},U{3},T{4}",
                VocabularyDataType,
                ElementId,
                Score,
                Unlocked,
                UpdateTimestamp
                );
        }

    }
}
