using System.Collections.Generic;
using Antura.Core;
using Antura.Database;

namespace Antura.Teacher
{
    // TODO refactor: merge JourneyPosition, JourneyHelper
    public class JourneyHelper
    {
        private DatabaseManager dbManager;

        public JourneyHelper(DatabaseManager _dbManager)
        {
            dbManager = _dbManager;
        }

        #region Utilities

        public bool IsAssessmentTime(JourneyPosition journeyPosition)
        {
            return journeyPosition.PlaySession == AssessmentPlaySessionIndex;
        }

        public int AssessmentPlaySessionIndex
        {
            get { return 100; }
        }

        public PlaySessionData GetCurrentPlaySessionData()
        {
            return AppManager.I.DB.GetPlaySessionDataById(AppManager.I.Player.CurrentJourneyPosition.Id);
        }

        #endregion

        #region JourneyPosition

        public JourneyPosition PlaySessionIdToJourneyPosition(string psId)
        {
            var parts = psId.Split('.');
            return new JourneyPosition(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }

        public IEnumerable<JourneyPosition> GetAllJourneyPositionsUpTo(JourneyPosition targetPosition)
        {
            var allPlaySessions = dbManager.GetAllPlaySessionData();
            for (int ps_i = 0; ps_i < allPlaySessions.Count; ps_i++) {
                if (allPlaySessions[ps_i].Id != targetPosition.Id) {
                    yield return allPlaySessions[ps_i].GetJourneyPosition();
                } else {
                    yield break;
                }
            }
        }

        public JourneyPosition FindNextJourneyPosition(JourneyPosition currentPosition)
        {
            var id = currentPosition.Id;

            var allPlaySessions = dbManager.GetAllPlaySessionData();
            int next_id = -1;
            for (int ps_i = 0; ps_i < allPlaySessions.Count; ps_i++) {
                if (allPlaySessions[ps_i].Id == id) {
                    next_id = ps_i + 1;
                    break;
                }
            }

            // Check for the last session
            if (next_id == allPlaySessions.Count) {
                return null;
            } else {
                return PlaySessionIdToJourneyPosition(allPlaySessions[next_id].Id);
            }
        }

        public JourneyPosition GetInitialJourneyPosition()
        {
            var allPlaySessions = dbManager.GetAllPlaySessionData();
            return PlaySessionIdToJourneyPosition(allPlaySessions[0].Id);
        }

        public JourneyPosition GetFinalJourneyPosition()
        {
            var allPlaySessions = dbManager.GetAllPlaySessionData();
            return PlaySessionIdToJourneyPosition(allPlaySessions[allPlaySessions.Count - 1].Id);
        }

        public bool PlayerIsAtFinalJourneyPosition()
        {
            return AppManager.I.Player.CurrentJourneyPosition.Equals(GetFinalJourneyPosition());
        }

        public JourneyPosition GetMinimumJourneyPositionForMiniGame(MiniGameCode minigameCode)
        {
            var finalPos = AppManager.I.JourneyHelper.GetFinalJourneyPosition();
            int NBasePlaySession = 2;

            for (int s = 1; s <= finalPos.Stage; s++) {
                for (int lb = 1; lb <= finalPos.LearningBlock; lb++) {
                    for (int ps = 1; ps <= NBasePlaySession; ps++) {
                        var jp = new JourneyPosition(s, lb, ps);
                        if (AppManager.I.DB.HasPlaySessionDataById(jp.Id)) {
                            if (AppManager.I.Teacher.CanMiniGameBePlayedAtPlaySession(jp, minigameCode)) {
                                return new JourneyPosition(s, lb, ps);
                            }
                        }
                    }
                    int assessmentCode = AssessmentPlaySessionIndex;
                    var jp_assessment = new JourneyPosition(s, lb, assessmentCode);

                    if (AppManager.I.DB.HasPlaySessionDataById(jp_assessment.Id)) {
                        if (AppManager.I.Teacher.CanMiniGameBePlayedAtPlaySession(jp_assessment, minigameCode)) {
                            return new JourneyPosition(s, lb, assessmentCode);
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Info getters

        public List<LearningBlockInfo> GetLearningBlockInfosForStage(int targetStage)
        {
            // @todo: this could use the new ScoreHelper methods
            // @todo: probably move this to ScoreHelper
            var learningBlockInfo_list = new List<LearningBlockInfo>();
            var learningBlockData_list = FindLearningBlockDataOfStage(targetStage);

            foreach (var learningBlockData in learningBlockData_list) {
                var LB_info = new LearningBlockInfo();
                LB_info.data = learningBlockData;
                LB_info.score = 0; // 0 if not found otherwise in the next step
                learningBlockInfo_list.Add(LB_info);
            }

            // Find all previous scores
            var scoreData_list = AppManager.I.ScoreHelper.GetCurrentScoreForLearningBlocksOfStage(targetStage);
            for (int i = 0; i < learningBlockInfo_list.Count; i++) {
                var LB_info = learningBlockInfo_list[i];
                var scoreData = scoreData_list.Find(x => x.JourneyDataType == JourneyDataType.LearningBlock && x.ElementId == LB_info.data.Id);
                LB_info.score = scoreData.GetScore();
            }

            return learningBlockInfo_list;
        }

        /// <summary>
        /// Returns a list of all play session data with its current score for the given stage and learning block.
        /// </summary>
        /// <param name="targetStage"></param>
        /// <param name="targetLearningBlock"></param>
        /// <returns></returns>
        public List<PlaySessionInfo> GetPlaySessionInfosForLearningBlock(int targetStage, int targetLearningBlock)
        {
            // @todo: this could use the new ScoreHelper methods
            // @todo: probably move this to ScoreHelper
            var playSessionInfo_list = new List<PlaySessionInfo>();
            var playSessionData_list = FindPlaySessionDataOfStageAndLearningBlock(targetStage, targetLearningBlock);
            foreach (var playSessionData in playSessionData_list) {
                var PS_info = new PlaySessionInfo();
                PS_info.data = playSessionData;
                PS_info.score = 0; // 0 if not found otherwise in the next step
                playSessionInfo_list.Add(PS_info);
            }

            // Find all previous scores
            var scoreData_list = AppManager.I.ScoreHelper.GetCurrentScoreForPlaySessionsOfLearningBlock(targetStage, targetLearningBlock);
            for (int i = 0; i < playSessionInfo_list.Count; i++) {
                var PS_info = playSessionInfo_list[i];
                var scoreData = scoreData_list.Find(x => x.JourneyDataType == JourneyDataType.PlaySession && x.ElementId == PS_info.data.Id);
                PS_info.score = scoreData.GetScore();
            }

            return playSessionInfo_list;
        }

        #endregion

        #region Stage -> LearningBlock -> PlaySession

        public List<LearningBlockData> FindLearningBlockDataOfStage(int targetStage)
        {
            return dbManager.FindLearningBlockData(x => x.Stage == targetStage);
        }

        public List<PlaySessionData> FindPlaySessionDataOfStageAndLearningBlock(int targetStage, int targetLearningBlock)
        {
            return dbManager.FindPlaySessionData(x => x.Stage == targetStage && x.LearningBlock == targetLearningBlock);
        }

        /// <summary>
        /// Given a stage, returns the list of all play session data corresponding to it.
        /// </summary>
        /// <param name="targetStage"></param>
        /// <returns></returns>
        public List<PlaySessionData> FindPlaySessionDataOfStage(int targetStage)
        {
            return dbManager.FindPlaySessionData(x => x.Stage == targetStage);
        }

        #endregion

        public bool HasFinishedTheGame()
        {
            return PlayerIsAtFinalJourneyPosition();
        }
    }
}