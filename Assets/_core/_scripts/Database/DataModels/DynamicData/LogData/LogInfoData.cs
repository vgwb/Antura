using Antura.Core;
using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Generic information on application usage at a given timestamp. Logged at runtime.
    /// </summary>
    [System.Serializable]
    public class LogInfoData : IData
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
        /// Event recorded.
        /// </summary>
        public InfoEvent Event { get; set; }

        /// <summary>
        /// the current scene
        /// </summary>
        public AppScene Scene { get; set; }

        /// <summary>
        /// Additional raw JSON data saved alongside the event to record more details.
        /// Example: "{playerId:0, rewardType:2}"
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Empty constructor required by MySQL.
        /// </summary>
        public LogInfoData() { }

        public LogInfoData(int appSession, InfoEvent _event, AppScene _scene, string additionalData)
        {
            AppSession = appSession;
            Event = _event;
            Scene = _scene;
            AdditionalData = additionalData;
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("AS{0},T{1},E{2},JSON{3}",
                AppSession,
                Timestamp,
                Event,
                AdditionalData
            );
        }

    }
}
