using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Saved data on achievements related to a MiniGame. Updated at runtime.
    /// </summary>
    [System.Serializable]
    public class MiniGameScoreData : IData, IScoreData, IDataEditable
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier for the player. empty during game. compiled at export/import
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Related MiniGame
        /// </summary>
        public MiniGameCode MiniGameCode { get; set; }

        /// <summary>
        /// Stars obtained during play.
        /// Integer in the [0,3] range
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Total play time for this minigame in seconds.
        /// </summary>
        public float TotalPlayTime { get; set; }

        /// <summary>
        /// Timestamp of the last update of this entry.
        /// </summary>
        public int UpdateTimestamp { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public MiniGameScoreData() { }

        public MiniGameScoreData(MiniGameCode code, int stars, float totalPlayTime) : this(code, stars, totalPlayTime, GenericHelper.GetTimestampForNow())
        {
        }

        public MiniGameScoreData(MiniGameCode code, int stars, float totalPlayTime, int timestamp)
        {
            MiniGameCode = code;
            Id = ((int)code).ToString();
            Stars = stars;
            TotalPlayTime = totalPlayTime;
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
            return string.Format("MG{0},S{1},T{2},TS{3}",
                MiniGameCode,
                Stars,
                TotalPlayTime,
                UpdateTimestamp
                );
        }

    }
}
