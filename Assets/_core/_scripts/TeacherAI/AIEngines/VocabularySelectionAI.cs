using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using UnityEngine;

namespace Antura.Teacher
{
    public class VocabularyContents
    {
        private HashSet<LetterData> letters = new HashSet<LetterData>();
        private HashSet<WordData> words = new HashSet<WordData>();
        private HashSet<PhraseData> phrases = new HashSet<PhraseData>();

        public HashSet<T> GetHashSet<T>()
        {
            if (typeof(T) == typeof(LetterData))
            { return letters as HashSet<T>; }
            if (typeof(T) == typeof(WordData))
            { return words as HashSet<T>; }
            if (typeof(T) == typeof(PhraseData))
            { return phrases as HashSet<T>; }
            return null;
        }

        public bool Contains<T>(T data)
        {
            var set = GetHashSet<T>();
            return set.Contains(data);
        }

        public void UnionWith<T>(IEnumerable<T> dataList)
        {
            var set = GetHashSet<T>();
            set.UnionWith(dataList);
        }

        public void UnionWith(VocabularyContents otherContents)
        {
            UnionWith(otherContents.GetHashSet<LetterData>());
            UnionWith(otherContents.GetHashSet<WordData>());
            UnionWith(otherContents.GetHashSet<PhraseData>());
        }

        public void AddAndFilterHashSetByContents<T>(List<T> inputList, HashSet<T> outputHash)
        {
            var set = GetHashSet<T>();
            foreach (T v in inputList)
            {
                if (set.Contains(v))
                {
                    outputHash.Add(v);
                }
            }
        }

        public override string ToString()
        {
            string s = "";
            s += letters.Count + " letters available";
            s += "\n" + words.Count + " words available";
            s += "\n" + phrases.Count + " phrases available";
            return s;
        }

    }

    /// <summary>
    /// Class that contains all vocabulary contents related to the journey progression.
    /// Useful for data filtering.
    /// </summary>
    public class ProgressionVocabularyContents
    {
        /// <summary>
        /// Play sessions unlocked thus far
        /// </summary>
        private Dictionary<JourneyPosition, VocabularyContents> playSessionsContents = new Dictionary<JourneyPosition, VocabularyContents>();

        private VocabularyContents allContents = new VocabularyContents();
        public VocabularyContents AllContents
        {
            get { return allContents; }
        }

        #region Get API

        public VocabularyContents GetContentsOfPlaySession(JourneyPosition pos)
        {
            return CreateContentsFromFunc(pair => pair.Key.Equals(pos));
        }

        public VocabularyContents GetContentsUpToPlaySession(JourneyPosition pos)
        {
            return CreateContentsFromFunc(pair => pair.Key.IsMinor(pos) || pair.Key.Equals(pos));
        }

        public VocabularyContents GetContentsOfLearningBlock(JourneyPosition pos)
        {
            return CreateContentsFromFunc(pair => pair.Key.LearningBlock == pos.LearningBlock && pair.Key.Stage == pos.Stage);
        }

        public VocabularyContents GetContentsOfStage(JourneyPosition pos)
        {
            return CreateContentsFromFunc(pair => pair.Key.Stage == pos.Stage);
        }

        #endregion

        private VocabularyContents CreateContentsFromFunc(Func<KeyValuePair<JourneyPosition, VocabularyContents>, bool> function)
        {
            var contents = new VocabularyContents();
            foreach (var pair in playSessionsContents.Where(function))
            {
                contents.UnionWith(pair.Value);
            }
            return contents;
        }


        #region Building

        public void AddPlaySession(string psId, List<LetterData> letters, List<WordData> words, List<PhraseData> phrases)
        {
            var newContents = new VocabularyContents();
            newContents.UnionWith(letters);
            newContents.UnionWith(words);
            newContents.UnionWith(phrases);
            playSessionsContents[new JourneyPosition(psId)] = newContents;

            allContents.UnionWith(letters);
            allContents.UnionWith(words);
            allContents.UnionWith(phrases);
        }

        #endregion
    }


    /// <summary>
    /// Handles the selection of what vocabulary data a minigame should use, given a playsession
    /// </summary>
    public class VocabularySelectionAI
    {
        // References
        private DatabaseManager dbManager;

        private ProgressionVocabularyContents progressionContents;
        private VocabularyContents currentPlaySessionContents;
        private VocabularyContents currentLearningBlockContents;
        private VocabularyContents currentStageContents;
        private VocabularyContents currentJourneyContents;

        public VocabularySelectionAI(DatabaseManager _dbManager)
        {
            this.dbManager = _dbManager;

            // Prepare all contents
            progressionContents = new ProgressionVocabularyContents();
            foreach (var psData in dbManager.GetAllPlaySessionData())
            {
                var psId = psData.Id;
                var letters = GetLettersInPlaySession(psId, pastSessionsToo: false);
                var words = GetWordsInPlaySession(psId, previousToo: true, pastSessionsToo: false);
                var phrases = GetPhrasesInPlaySession(psId, previousToo: true, pastSessionsToo: false);
                progressionContents.AddPlaySession(psId, letters, words, phrases);
            }
        }

        public void LoadCurrentPlaySessionData(string currentPlaySessionId)
        {
            var pos = new JourneyPosition(currentPlaySessionId);
            currentJourneyContents = progressionContents.GetContentsUpToPlaySession(pos);
            currentPlaySessionContents = progressionContents.GetContentsOfPlaySession(pos);
            currentLearningBlockContents = progressionContents.GetContentsOfLearningBlock(pos);
            currentStageContents = progressionContents.GetContentsOfStage(pos);

            if (ConfigAI.VerbosePlaySessionInitialisation)
            {
                string debugString = "";
                debugString += ConfigAI.FormatTeacherReportHeader("Play Session Initalisation (" + currentPlaySessionId + ")");
                debugString += "\n Current PS:\n" + currentPlaySessionContents;
                debugString += "\n Current LB:\n" + currentLearningBlockContents;
                debugString += "\n Current ST:\n" + currentStageContents;
                debugString += "\n Current journey:\n" + currentJourneyContents;
                debugString += "\n Whole contents:\n" + progressionContents.AllContents;
                ConfigAI.AppendToTeacherReport(debugString);
            }
        }

        #region Data Selection logic

        private string debugString_selectData;

        public List<T> SelectData<T>(System.Func<List<T>> builderSelectionFunction, SelectionParameters selectionParams, bool isTest = false, bool canReturnZero = false) where T : IVocabularyData
        {
            // skip if we require 0 values
            if (selectionParams.nRequired == 0 && !selectionParams.getMaxData)
            {
                return new List<T>();
            }

            if (ConfigAI.VerboseDataSelection)
            {
                debugString_selectData = "";
                debugString_selectData += ConfigAI.FormatTeacherReportHeader("Data Selection: " + typeof(T).Name);
            }

            // (1) Filtering based on the builder's logic
            var originalDataList = builderSelectionFunction();
            var dataList = originalDataList;
            int nAfterBuilder = dataList.Count;
            if (ConfigAI.VerboseDataFiltering)
                debugString_selectData += ("\n  Builder: " + dataList.Count);

            // (2) Filtering based on journey
            if (selectionParams.useJourney && !ConfigAI.ForceJourneyIgnore)
            {
                switch (selectionParams.journeyFilter)
                {
                    case SelectionParameters.JourneyFilter.CurrentJourney:
                        dataList = dataList.FindAll(x => currentJourneyContents.Contains(x));
                        break;

                    case SelectionParameters.JourneyFilter.UpToFullCurrentStage:
                        dataList = dataList.FindAll(x => currentJourneyContents.Contains(x) || currentStageContents.Contains(x));
                        break;

                    case SelectionParameters.JourneyFilter.CurrentLearningBlockOnly:
                        dataList = dataList.FindAll(x => currentLearningBlockContents.Contains(x));
                        break;

                    case SelectionParameters.JourneyFilter.CurrentLearningBlockFallbackToCurrentJourney:
                        dataList = dataList.FindAll(x => currentLearningBlockContents.Contains(x));
                        break;
                }
            }

            bool mustCheckNumber = selectionParams.severity == SelectionSeverity.AllRequired;
            if (selectionParams.useJourney && !ConfigAI.ForceJourneyIgnore && selectionParams.journeyFilter == SelectionParameters.JourneyFilter.CurrentLearningBlockFallbackToCurrentJourney) mustCheckNumber = true;
            if (mustCheckNumber)
            {
                if (!CheckRequiredNumberReached(dataList, selectionParams, nAfterBuilder))
                {
                    if (selectionParams.journeyFilter == SelectionParameters.JourneyFilter.CurrentLearningBlockFallbackToCurrentJourney)
                    {
                        //Debug.LogWarning("The teacher could not find " + selectionParams.nRequired + " data instances in the current Learning Block. Fallback to CurrentJourney instead.");
                        dataList = originalDataList.FindAll(x => currentJourneyContents.Contains(x));
                    }
                    else
                    {
                        throw new Exception("The teacher could not find " + selectionParams.nRequired + " data instances after applying the journey logic.");
                    }
                }
            }
            if (ConfigAI.VerboseDataFiltering)
                debugString_selectData += ("\n  Journey: " + dataList.Count);
            //Debug.Log("Journey: " + dataList.ToDebugStringNewline());

            // (3) Filtering based on pack-list history
            switch (selectionParams.packListHistory)
            {
                case PackListHistory.NoFilter:
                    // we do not care which are picked, in this case
                    break;

                case PackListHistory.ForceAllDifferent:
                    // filter only by those that have not been found already in this pack, if possible
                    dataList = dataList.FindAll(x => !selectionParams.filteringIds.Contains(x.GetId()));
                    if (!CheckRequiredNumberReached(dataList, selectionParams, nAfterBuilder))
                    {
                        UnityEngine.Debug.Log(debugString_selectData);
                        throw new System.Exception("The teacher could not find " + selectionParams.nRequired + " data instances after applying the pack-history logic.");
                    }
                    break;

                case PackListHistory.RepeatWhenFull:
                    // reset the previous pack list if needed
                    var tmpDataList = dataList.FindAll(x => !selectionParams.filteringIds.Contains(x.GetId()));
                    if (tmpDataList.Count < selectionParams.nRequired)
                    {
                        // reset and re-pick
                        selectionParams.filteringIds.Clear();
                        dataList = dataList.FindAll(x => !selectionParams.filteringIds.Contains(x.GetId()));
                    }
                    else
                    {
                        dataList = tmpDataList;
                    }
                    break;
            }
            if (ConfigAI.VerboseDataFiltering)
                debugString_selectData += ("\n  History: " + dataList.Count);
            //Debug.Log("History: " + dataList.ToDebugStringNewline());
            //if(selectionParams.filteringIds != null) Debug.Log("Filtered ids:: " + selectionParams.filteringIds.ToDebugStringNewline());

            // (4) Priority filtering based on current focus
            List<T> priorityFilteredList = dataList;
            if (!isTest && !selectionParams.getMaxData && selectionParams.priorityFilter != SelectionParameters.PriorityFilter.NoPriority)
            {
                HashSet<T> priorityFilteredHash = new HashSet<T>();
                string s = ConfigAI.FormatTeacherReportHeader("Priority Filtering");

                switch (selectionParams.priorityFilter)
                {
                    case SelectionParameters.PriorityFilter.CurrentThenExpand:
                    {
                        int nBefore = selectionParams.nRequired;
                        int nRemaining = selectionParams.nRequired;
                        AddToHashSetFilteringByContents(currentPlaySessionContents, dataList, priorityFilteredHash, ref nRemaining);

                        s += "\n Required: " + nRemaining + " " + typeof(T).Name.ToString();
                        s += "\n" + (nBefore - nRemaining) + " from PS";
                        if (nRemaining > 0)
                        {
                            nBefore = nRemaining;
                            AddToHashSetFilteringByContents(currentLearningBlockContents, dataList, priorityFilteredHash, ref nRemaining);
                            s += "\n" + (nBefore - nRemaining) + " from LB";
                        }
                        if (nRemaining > 0)
                        {
                            nBefore = nRemaining;
                            AddToHashSetFilteringByContents(currentStageContents, dataList, priorityFilteredHash, ref nRemaining);
                            s += "\n" + (nBefore - nRemaining) + " from ST";
                        }
                        if (nRemaining > 0)
                        {
                            nBefore = nRemaining;
                            AddToHashSetFilteringByContents(currentJourneyContents, dataList, priorityFilteredHash, ref nRemaining);
                            s += "\n" + (nBefore - nRemaining) + " from the current Journey";
                        }
                        // @note: when journey filtering is disabled, we may still have to get some data from the rest of the journey
                        if (nRemaining > 0 && !selectionParams.useJourney)
                        {
                            nBefore = nRemaining;
                            AddToHashSetFilteringByContents(progressionContents.AllContents, dataList, priorityFilteredHash, ref nRemaining);
                            s += "\n" + (nBefore - nRemaining) + " from the complete contents.";
                        }
                    }
                    break;
                    case SelectionParameters.PriorityFilter.CurrentThenPast:
                    {
                        int nBefore = selectionParams.nRequired;
                        int nRemaining = selectionParams.nRequired;
                        var loopPos = AppManager.I.Player.CurrentJourneyPosition;
                        s += "\n Required: " + nRemaining + " " + typeof(T).Name.ToString();
                        while (nRemaining > 0)
                        {
                            nBefore = nRemaining;
                            var loopContents = progressionContents.GetContentsOfPlaySession(loopPos);
                            AddToHashSetFilteringByContents(loopContents, dataList, priorityFilteredHash, ref nRemaining);
                            s += "\n" + (nBefore - nRemaining) + " from PS " + loopPos.ToDisplayedString(true);
                            if (loopPos.Equals(JourneyPosition.InitialJourneyPosition))
                                break;
                            loopPos = AppManager.I.JourneyHelper.FindPreviousJourneyPosition(loopPos);
                        }
                    }
                    break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (ConfigAI.VerboseDataFiltering)
                    ConfigAI.AppendToTeacherReport(s);
                priorityFilteredList = new List<T>();
                priorityFilteredList.AddRange(priorityFilteredHash);
                if (ConfigAI.VerboseDataFiltering)
                    debugString_selectData += ("\n  Priority: " + priorityFilteredList.Count);
            }
            //Debug.Log("Priority: " + priorityFilteredList.ToDebugStringNewline());

            // (5) Weighted selection on the remaining number
            List<T> selectedList = null;
            if (selectionParams.getMaxData)
            {
                selectedList = priorityFilteredList;
            }
            else
            {
                selectedList = WeightedDataSelect(priorityFilteredList, selectionParams.nRequired, selectionParams.severity);
            }
            if (ConfigAI.VerboseDataFiltering)
                debugString_selectData += ("\n  Selection: " + selectedList.Count);
            //if (selectedList.Count > 0) Debug.Log("Selection: " + selectedList.ToDebugStringNewline());

            if (ConfigAI.VerboseDataFiltering && !isTest)
            {
                foreach (var selectedEntry in selectedList)
                {
                    debugString_selectData += "   [" + selectedEntry + "]";
                }
                ConfigAI.AppendToTeacherReport(debugString_selectData);
            }

            if (selectedList.Count == 0)
            {
                if (canReturnZero)
                {
                    return selectedList;
                }

                throw new System.Exception("The teacher could not find any data with the current filters. The game does not seem to be playable at the selected play session."
                     + "\n" + debugString_selectData);
            }

            // Update the filtering ids
            if (selectionParams.packListHistory != PackListHistory.NoFilter)
            {
                selectionParams.filteringIds.AddRange(selectedList.ConvertAll<string>(x => x.GetId()).ToArray());
            }

            // Reorder the selected data based on intrinsic difficulty
            if (selectionParams.sortDataByDifficulty)
            {
                selectedList.Sort((x, y) => (int)(x.GetIntrinsicDifficulty() - y.GetIntrinsicDifficulty()));
            }

            return selectedList;
        }

        private void AddToHashSetFilteringByContents<T>(VocabularyContents contents, List<T> inputList, HashSet<T> outputHash, ref int nRemaining)
        {
            int nBefore = outputHash.Count;
            contents.AddAndFilterHashSetByContents(inputList, outputHash);
            nRemaining -= outputHash.Count - nBefore;
            if (nRemaining < 0)
            {
                nRemaining = 0;
            }
        }

        private bool CheckRequiredNumberReached<T>(List<T> dataList, SelectionParameters selectionParams, int nAfterBuilder)
        {
            return (!selectionParams.getMaxData && dataList.Count >= selectionParams.nRequired)
                || (selectionParams.getMaxData && dataList.Count >= nAfterBuilder);
        }

        private string weightedData_debugString;
        private List<T> WeightedDataSelect<T>(List<T> source_data_list, int nToSelect, SelectionSeverity severity) where T : IData
        {
            VocabularyDataType dataType = VocabularyDataType.Letter;
            if (typeof(T) == typeof(LetterData))
            {
                dataType = VocabularyDataType.Letter;
            }
            else if (typeof(T) == typeof(WordData))
            {
                dataType = VocabularyDataType.Word;
            }
            else if (typeof(T) == typeof(PhraseData))
            {
                dataType = VocabularyDataType.Phrase;
            }

            // Given a (filtered) list of data, select some using weights
            List<VocabularyScoreData> score_data_list = dbManager.HasLoadedPlayerProfile() ? dbManager.Query<VocabularyScoreData>("SELECT * FROM " + nameof(VocabularyScoreData) + " WHERE VocabularyDataType = '" + (int)dataType + "'") : new List<VocabularyScoreData>();

            if (ConfigAI.VerboseDataSelection)
            {
                weightedData_debugString += ConfigAI.FormatTeacherReportHeader("Selection Weights");
                weightedData_debugString = "";
            }

            var weights_list = new List<float>();
            foreach (var sourceData in source_data_list)
            {
                float cumulativeWeight = 0;
                if (ConfigAI.VerboseDataSelection)
                    weightedData_debugString += "\n" + sourceData.GetId() + " ---";

                // Get score data
                var score_data = score_data_list.Find(x => x.ElementId == sourceData.GetId());
                float currentScore = 0;
                int daysSinceLastScore = 0;
                if (score_data != null)
                {
                    var timespanFromLastScoreToNow = GenericHelper.GetTimeSpanBetween(score_data.UpdateTimestamp, GenericHelper.GetTimestampForNow());
                    daysSinceLastScore = timespanFromLastScoreToNow.Days;
                    currentScore = score_data.Score;
                }

                //UnityEngine.Debug.Log("Data " + id + " score: " + currentScore + " days " + daysSinceLastScore);

                // Score Weight [0,1]: higher the lower the score [-1,1] is
                var scoreWeight = 0.5f * (1 - currentScore);
                cumulativeWeight += scoreWeight * ConfigAI.Vocabulary_Score_Weight;
                if (ConfigAI.VerboseDataSelection)
                    weightedData_debugString += " \tScore: " + scoreWeight * ConfigAI.Vocabulary_Score_Weight + "(" + scoreWeight + ")";

                // RecentPlay Weight  [1,0]: higher the more in the past we saw that data
                const float dayLinerWeightDecrease = 1f / ConfigAI.DaysForMaximumRecentPlayMalus;
                float weightMalus = daysSinceLastScore * dayLinerWeightDecrease;
                float recentPlayWeight = 1f - UnityEngine.Mathf.Min(1, weightMalus);
                cumulativeWeight += recentPlayWeight * ConfigAI.Vocabulary_RecentPlay_Weight;
                if (ConfigAI.VerboseDataSelection)
                    weightedData_debugString += " \tRecent: " + recentPlayWeight * ConfigAI.Vocabulary_RecentPlay_Weight + "(" + recentPlayWeight + ")";

                // Current focus weight [1,0]: higher if the data is part of the current play session / learning block / stage
                float currentPlaySessionWeight = 0;
                if (currentPlaySessionContents.Contains(sourceData))
                {
                    currentPlaySessionWeight = 1;
                }
                else if (currentLearningBlockContents.Contains(sourceData))
                {
                    currentPlaySessionWeight = 0.5f;
                }
                else if (currentStageContents.Contains(sourceData))
                {
                    currentPlaySessionWeight = 0.2f;
                }
                cumulativeWeight += currentPlaySessionWeight * ConfigAI.Vocabulary_CurrentPlaySession_Weight;
                if (ConfigAI.VerboseDataSelection)
                    weightedData_debugString += " \tFocus: " + currentPlaySessionWeight * ConfigAI.Vocabulary_CurrentPlaySession_Weight + "(" + currentPlaySessionWeight + ")";

                // If the cumulative weight goes to the negatives, we give it a fixed weight
                // TODO check if we shound use if (cumulativeWeight <= ConfigAI.Vocabulary_MinTotal_Weight)
                // TODO check the "continue" because it wont' save the data
                if (cumulativeWeight <= 0)
                {
                    cumulativeWeight = ConfigAI.Vocabulary_MinTotal_Weight;
                    continue;
                }

                // Save cumulative weight
                weights_list.Add(cumulativeWeight);
                if (ConfigAI.VerboseDataSelection)
                    weightedData_debugString += " TOTw: " + cumulativeWeight;
            }

            //return source_data_list;

            if (ConfigAI.VerboseDataSelection)
            {
                ConfigAI.AppendToTeacherReport(weightedData_debugString);
            }

            // Select data from the list
            var selected_data_list = new List<T>();
            if (source_data_list.Count > 0)
            {
                int nToSelectFromCurrentList = 0;
                List<T> chosenData = null;
                switch (severity)
                {
                    case SelectionSeverity.AsManyAsPossible:
                    case SelectionSeverity.AllRequired:
                        nToSelectFromCurrentList = UnityEngine.Mathf.Min(source_data_list.Count, nToSelect);
                        chosenData = RandomHelper.RouletteSelectNonRepeating(source_data_list, weights_list, nToSelectFromCurrentList);
                        selected_data_list.AddRange(chosenData);
                        break;
                    case SelectionSeverity.MayRepeatIfNotEnough:
                        int nRemainingToSelect = nToSelect;
                        while (nRemainingToSelect > 0)
                        {
                            var listCopy = new List<T>(source_data_list);
                            nToSelectFromCurrentList = UnityEngine.Mathf.Min(source_data_list.Count, nRemainingToSelect);
                            chosenData = RandomHelper.RouletteSelectNonRepeating(listCopy, weights_list, nToSelectFromCurrentList);
                            selected_data_list.AddRange(chosenData);
                            nRemainingToSelect -= nToSelectFromCurrentList;
                        }
                        break;
                }
            }
            return selected_data_list;
        }

        #endregion

        #region Contents Helpers

        public VocabularyContents GetContentsAtJourneyPosition(JourneyPosition jp)
        {
            return progressionContents.GetContentsOfPlaySession(jp);
        }

        public VocabularyContents GetContentsUpToJourneyPosition(JourneyPosition jp)
        {
            return progressionContents.GetContentsUpToPlaySession(jp);
        }

        public VocabularyContents GetContentsAtLearningBlock(JourneyPosition jp)
        {
            return progressionContents.GetContentsOfLearningBlock(jp);
        }

        public VocabularyContents GetContentsOfStage(int stage)
        {
            return progressionContents.GetContentsOfStage(new JourneyPosition(stage, 1, 1));
        }

        public VocabularyContents CurrentJourneyContents { get { return currentJourneyContents; } }

        #endregion


        // @todo: move these to JourneyHelper instead?
        #region LearningBlock / PlaySession -> Letter

        public List<LetterData> GetLettersInLearningBlock(string LB_Id, bool pastBlocksToo = false)
        {
            var LB_Data = dbManager.GetLearningBlockDataById(LB_Id);
            var PS_Data_list = dbManager.GetPlaySessionsOfLearningBlock(LB_Data);

            var letterData_set = new HashSet<LetterData>();
            foreach (var psData in PS_Data_list)
            {
                var ps_letterData = GetLettersInPlaySession(psData.Id, pastBlocksToo);
                letterData_set.UnionWith(ps_letterData);
            }
            return new List<LetterData>(letterData_set);
        }

        private List<LetterData> GetLettersInPlaySession(string PS_Id, bool pastSessionsToo = false)
        {
            var PlaySessionData = dbManager.GetPlaySessionDataById(PS_Id);

            var ids_set = new HashSet<string>();
            ids_set.UnionWith(PlaySessionData.Letters);
            if (pastSessionsToo)
            {
                ids_set.UnionWith(this.GetAllLetterIdsFromPreviousPlaySessions(PlaySessionData));
            }

            var ids_list = new List<string>(ids_set);
            return ids_list.ConvertAll(x => dbManager.GetLetterDataById(x));
        }

        private string[] GetAllLetterIdsFromPreviousPlaySessions(PlaySessionData current_ps)
        {
            // @note: this assumes that all play sessions are correctly ordered
            var all_ps_list = dbManager.GetAllPlaySessionData();
            int current_id = all_ps_list.IndexOf(current_ps);

            var all_ids = new List<string>();
            for (int prev_id = 0; prev_id < current_id; prev_id++)
            {
                all_ids.AddRange(all_ps_list[prev_id].Letters);
            }

            return all_ids.ToArray();
        }
        #endregion

        #region LearningBlock / PlaySession -> Word

        private List<WordData> GetWordsInLearningBlock(string lbId, bool previousToo = true, bool pastBlocksToo = false)
        {
            var lbData = dbManager.GetLearningBlockDataById(lbId);
            var psData_list = dbManager.GetPlaySessionsOfLearningBlock(lbData);

            var wordData_set = new HashSet<WordData>();
            foreach (var psData in psData_list)
            {
                var ps_wordData = GetWordsInPlaySession(psData.Id, previousToo, pastBlocksToo);
                wordData_set.UnionWith(ps_wordData);
            }
            return new List<WordData>(wordData_set);
        }

        private List<WordData> GetWordsInPlaySession(string psId, bool previousToo = false, bool pastSessionsToo = false)
        {
            var psData = dbManager.GetPlaySessionDataById(psId);

            var ids_set = new HashSet<string>();
            ids_set.UnionWith(psData.Words);
            if (previousToo)
            { ids_set.UnionWith(psData.Words_previous); }
            if (pastSessionsToo)
            { ids_set.UnionWith(this.GetAllWordIdsFromPreviousPlaySessions(psData)); }

            var ids_list = new List<string>(ids_set);
            return ids_list.ConvertAll(x => dbManager.GetWordDataById(x));
        }

        private string[] GetAllWordIdsFromPreviousPlaySessions(PlaySessionData current_ps)
        {
            // @note: this assumes that all play sessions are correctly ordered
            var all_ps_list = dbManager.GetAllPlaySessionData();
            int current_id = all_ps_list.IndexOf(current_ps);

            var all_ids = new List<string>();
            for (int prev_id = 0; prev_id < current_id; prev_id++)
            {
                all_ids.AddRange(all_ps_list[prev_id].Words);
                all_ids.AddRange(all_ps_list[prev_id].Words_previous);
            }

            return all_ids.ToArray();
        }

        #endregion

        #region LearningBlock / PlaySession -> Phrase

        private List<PhraseData> GetPhrasesInLearningBlock(string lbId, bool previousToo = true, bool pastBlocksToo = false)
        {
            var lbData = dbManager.GetLearningBlockDataById(lbId);
            var psData_list = dbManager.GetPlaySessionsOfLearningBlock(lbData);

            var phraseData_set = new HashSet<PhraseData>();
            foreach (var psData in psData_list)
            {
                var ps_phraseData = GetPhrasesInPlaySession(psData.Id, previousToo, pastBlocksToo);
                phraseData_set.UnionWith(ps_phraseData);
            }
            return new List<PhraseData>(phraseData_set);
        }

        private List<PhraseData> GetPhrasesInPlaySession(string lbId, bool previousToo = false, bool pastSessionsToo = false)
        {
            var psData = dbManager.GetPlaySessionDataById(lbId);

            var ids_set = new HashSet<string>();
            ids_set.UnionWith(psData.Phrases);
            if (previousToo)
            { ids_set.UnionWith(psData.Phrases_previous); }
            if (pastSessionsToo)
            { ids_set.UnionWith(this.GetAllPhraseIdsFromPreviousPlaySessions(psData)); }

            var ids_list = new List<string>(ids_set);
            return ids_list.ConvertAll(x => dbManager.GetPhraseDataById(x));
        }

        private string[] GetAllPhraseIdsFromPreviousPlaySessions(PlaySessionData current_ps)
        {
            // @note: this assumes that all play sessions are correctly ordered
            var all_ps_list = dbManager.GetAllPlaySessionData();
            int current_id = all_ps_list.IndexOf(current_ps);

            var all_ids = new List<string>();
            for (int prev_id = 0; prev_id < current_id; prev_id++)
            {
                all_ids.AddRange(all_ps_list[prev_id].Phrases);
                all_ids.AddRange(all_ps_list[prev_id].Phrases_previous);
            }

            return all_ids.ToArray();
        }
        #endregion

    }
}
