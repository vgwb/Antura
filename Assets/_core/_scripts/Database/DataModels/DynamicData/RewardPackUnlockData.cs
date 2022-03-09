using Antura.Core;
using Antura.Helpers;
using Antura.Rewards;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Serialized data relative to a reward, used for unlocking. Updated at runtime.
    /// </summary>
    [System.Serializable]
    public class RewardPackUnlockData : IData, IDataEditable
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
        /// Identifier of the application session.
        /// </summary>
        public int AppSession { get; set; }

        /// <summary>
        /// Stage at which the reward data has been unlocked.
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// LearningBlock at which the reward data has been unlocked.
        /// </summary>
        public int LearningBlock { get; set; }

        /// <summary>
        /// PlaySession at which the reward data has been unlocked.
        /// </summary>
        public int PlaySession { get; set; }

        /// <summary>
        /// The order of playsession rewards in case of multi reward for same playsession.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// True if never used by player.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// True if not unlocked yet
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Timestamp of creation of the reward.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// JSON-serialized additional data, may be added as needed.
        /// </summary>
        public string AdditionalData { get; set; }

        public RewardPackUnlockData()
        {
        }

        public RewardPackUnlockData(int appSession, string packId, JourneyPosition journeyPosition)
        {
            AppSession = appSession;
            Id = packId;
            Stage = journeyPosition.Stage;
            LearningBlock = journeyPosition.LearningBlock;
            PlaySession = journeyPosition.PlaySession;
            Order = 0;
            IsNew = true;
            IsLocked = true;
            Timestamp = GenericHelper.GetTimestampForNow();
        }

        #region Rewards API

        public JourneyPosition GetJourneyPosition()
        {
            return new JourneyPosition(Stage, LearningBlock, PlaySession);
        }

        public void SetJourneyPosition(JourneyPosition jp)
        {
            Stage = jp.Stage;
            LearningBlock = jp.LearningBlock;
            PlaySession = jp.PlaySession;
        }

        #endregion

        #region Database API

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
            return string.Format("{0} : [{1}]", Id, PlaySession);
        }

        #endregion

    }
}
