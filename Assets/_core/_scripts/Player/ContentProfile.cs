using System;
using Antura.Core;
using Antura.Database;

namespace Antura.Profile
{
    [Serializable]
    public class ContentProfile
    {
        public LearningContentID ContentID;

        public bool JourneyCompleted;
        public int TotalScore;
        public ProfileCompletionState ProfileCompletion;

        public JourneyPosition MaxJourneyPosition = JourneyPosition.InitialJourneyPosition;
        public JourneyPosition CurrentJourneyPosition = JourneyPosition.InitialJourneyPosition;
        public JourneyPosition PreviousJourneyPosition = JourneyPosition.InitialJourneyPosition;

        #region Input

        public ContentProfile FromData(ContentProfileData _data)
        {
            ContentID = _data.ContentID;
            JourneyCompleted = _data.JourneyCompleted;
            //HasFinishedTheGameWithAllStars = _data.HasFinishedTheGameWithAllStars();
            ProfileCompletion = _data.ProfileCompletion;
            //HasMaxStarsInCurrentPlaySessions = _data.GetAdditionalData().HasMaxStarsInCurrentPlaySessions;
            MaxJourneyPosition = new JourneyPosition(_data.MaxStage, _data.MaxLearningBlock, _data.MaxPlaySession);
            CurrentJourneyPosition = new JourneyPosition(_data.CurrentStage, _data.CurrentLearningBlock, _data.CurrentPlaySession);
            PreviousJourneyPosition = new JourneyPosition(_data.CurrentStage, _data.CurrentLearningBlock, _data.CurrentPlaySession);    // Get from Current too
            return this;
        }

        #endregion

        #region Output

        public ContentProfileData ToData()
        {
            var newData = new ContentProfileData
            {
                Id = this.ContentID.ToString(),
                ContentID = this.ContentID,

                JourneyCompleted = this.JourneyCompleted,
                TotalScore = this.TotalScore,
                ProfileCompletion = this.ProfileCompletion,

                MaxStage = this.MaxJourneyPosition.Stage,
                MaxLearningBlock = this.MaxJourneyPosition.LearningBlock,
                MaxPlaySession = this.MaxJourneyPosition.PlaySession,

                CurrentStage = this.CurrentJourneyPosition.Stage,
                CurrentLearningBlock = this.CurrentJourneyPosition.LearningBlock,
                CurrentPlaySession = this.CurrentJourneyPosition.PlaySession
            };
            return newData;
        }

        #endregion

        public override string ToString() => $"ContentID {ContentID} | Curr{CurrentJourneyPosition} | Max{MaxJourneyPosition}";
    }
}
