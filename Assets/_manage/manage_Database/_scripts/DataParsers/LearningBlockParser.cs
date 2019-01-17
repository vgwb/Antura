using System.Collections.Generic;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for LearningBlockData
    /// </summary>
    public class LearningBlockParser : DataParser<LearningBlockData, LearningBlockTable>
    {
        override protected LearningBlockData CreateData(Dictionary<string, object> dict, DatabaseObject db)
        {
            var data = new LearningBlockData();

            data.Stage = ToInt(dict["Stage"]);
            data.LearningBlock = ToInt(dict["LearningBlock"]);
            data.Id = data.Stage + "." + data.LearningBlock;

            data.Description_En = ToString(dict["Description_En"]);
            data.Description_Ar = ToString(dict["Description_Ar"]);
            data.Title_Ar = ToString(dict["Title_Ar"]);
            data.Title_En = ToString(dict["Title_En"]);
            data.AudioFile = ToString(dict["AudioFile"]);
            //data.Reward = ParseID<RewardData, RewardTable>(data, (string)dict["Reward"], db.GetRewardTable());
            data.Focus = ParseEnum<LearningBlockDataFocus>(data, (string)dict["Focus"]);

            //data.AssessmentData = ToString(dict["AssessmentData"]);

            return data;
        }

        protected override bool CanHaveSameKeyMultipleTimes
        {
            get {
                return true;
            }
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Focus", addNoneValue: true);
        }

        protected override void FinalValidation(LearningBlockTable table, DatabaseObject db)
        {
            // Field 'NumberOfPlaySessions' can be computed only at the end
            var playSessionsList = new List<PlaySessionData>(db.GetPlaySessionTable().GetValuesTyped());
            foreach (var data in table.GetValuesTyped()) {
                int nPlaySessions = playSessionsList.FindAll(x => x.Stage == data.Stage && x.LearningBlock == data.LearningBlock).Count;
                data.NumberOfPlaySessions = nPlaySessions;
            }

        }
    }
}
