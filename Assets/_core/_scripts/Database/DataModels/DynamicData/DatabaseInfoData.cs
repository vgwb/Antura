using Antura.Helpers;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Serialized information on the database. Used for versioning.
    /// </summary>
    [System.Serializable]
    public class DatabaseInfoData : IData, IDataEditable
    {
        public const string UNIQUE_ID = "1";

        /// <summary>
        /// Primary key for the database.
        /// Unique, as there will be only one row for this table.
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier for the player. empty during game. compiled at export/import
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Timestamp of creation of the database.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Version of the MySQL database.
        /// Different versions cannot be compared.
        /// </summary>
        public string DynamicDbVersion { get; set; }

        /// <summary>
        /// Version of the Static database.
        /// Different versions cannot be compared.
        /// </summary>
        public string StaticDbVersion { get; set; }

        public DatabaseInfoData()
        {
        }

        public DatabaseInfoData(string dynamicDbVersion, string staticDbVersion)
        {
            this.Id = UNIQUE_ID;  // Only one record
            this.DynamicDbVersion = dynamicDbVersion;
            this.StaticDbVersion = staticDbVersion;
            this.Timestamp = GenericHelper.GetTimestampForNow();
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("ID{0},sqlV{1},statV{2},Ts{3}",
                Id,
                DynamicDbVersion,
                StaticDbVersion,
                Timestamp
            );
        }

        public void SetId(string _Id)
        {
            Id = _Id;
        }
    }
}
