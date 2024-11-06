using Antura.Core;
using Antura.Profile;
using SQLite;

namespace Antura.Database
{
    /// <summary>
    /// Serialized information about a content. Used by the new Player Profile.
    /// </summary>
    [System.Serializable]
    public class ContentProfileData : IData, IDataEditable
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        public LearningContentID ContentID { get; set; }

        public bool JourneyCompleted { get; set; }
        public float TotalScore { get; set; }
        public ProfileCompletionState ProfileCompletion { get; set; }
        public bool HasFinishedTheGame { get; set; }
        public bool HasFinishedTheGameWithAllStars { get; set; }
        public bool HasMaxStarsInCurrentPlaySessions { get; set; }

        public int MaxStage { get; set; }
        public int MaxLearningBlock { get; set; }
        public int MaxPlaySession { get; set; }

        public int CurrentStage { get; set; }
        public int CurrentLearningBlock { get; set; }
        public int CurrentPlaySession { get; set; }

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
            return string.Format("ID{0},MaxJ({1}.{2}.{3}), CurrentJ({4}.{5}.{6}), ProfCompl:{7}, JourneyCompleted:{8}, Score:{9}",
                Id,

                MaxStage,
                MaxLearningBlock,
                MaxPlaySession,

                CurrentStage,
                CurrentLearningBlock,
                CurrentPlaySession,

                ProfileCompletion,
                JourneyCompleted,
                TotalScore
            );
        }

        #endregion

    }
}
