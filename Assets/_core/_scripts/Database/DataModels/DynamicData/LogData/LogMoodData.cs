using Antura.Helpers;
using Antura.Utilities;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Daily mood level of a player at a given timestamp. Logged at runtime.
    /// </summary>
    [System.Serializable]
    public class LogMoodData : IData
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
        /// Normalized mood value recorded. [0,1]
        /// </summary>
        public float MoodValue { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public LogMoodData() { }

        public LogMoodData(int appSession, float moodValue)
        {
            AppSession = appSession;
            MoodValue = moodValue;
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("S{0},T{1},MV{2}",
                AppSession,
                Timestamp,
                MoodValue
                );
        }

    }
}
