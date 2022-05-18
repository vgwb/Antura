using Antura.Core;
using Antura.LivingLetters;
using Antura.Teacher;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Concrete implementation of the log manager, accessible to minigames.
    /// </summary>
    public class MinigamesLogManager : ILogManager
    {
        #region Runtime variables

        private string sessionName;
        private MiniGameCode miniGameCode;

        private List<ILivingLetterAnswerData> logLearnBuffer = new List<ILivingLetterAnswerData>();
        private List<LogAI.PlayResultParameters> logPlayBuffer = new List<LogAI.PlayResultParameters>();

        #endregion

        #region Initialization

        public MinigamesLogManager(MiniGameCode miniGameCode, string sessionName)
        {
            this.miniGameCode = miniGameCode;
            this.sessionName = sessionName;
        }

        #endregion

        #region API

        /// <summary>
        /// To be called to any action of player linked to learnig objective and with positive or negative vote.
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_isPositiveResult"></param>
        public void OnAnswered(ILivingLetterData _data, bool _isPositiveResult)
        {
            if (AppConfig.DebugLogDbInserts)
            {
                Debug.Log("pre-log OnAnswer " + _data.Id + " " + _isPositiveResult);
            }
            ILivingLetterAnswerData newILivingLetterAnswerData = new ILivingLetterAnswerData();
            newILivingLetterAnswerData._data = _data;
            newILivingLetterAnswerData._isPositiveResult = _isPositiveResult;
            BufferizeLogLearnData(newILivingLetterAnswerData);
        }

        /// <summary>
        /// Called when minigame is finished.
        /// </summary>
        /// <param name="_valuation">The valuation.</param>
        public void OnGameEnded(int _valuation)
        {
            FlushLogLearn();
            //FlushLogPlay();   // Unless minigames can directly log play skills, this is not needed
            LogManager.I.LogMinigameScore(sessionName, miniGameCode, _valuation);
        }

        /*
        /// <summary>
        /// Called when player perform a [gameplay skill action] action during gameplay. .
        /// </summary>
        /// <param name="_ability">The ability.</param>
        /// <param name="_score">The score.</param>
        public void OnGameplaySkillAction(PlaySkill _ability, float _score)
        {
            if (AppConstants.DebugLogInserts) Debug.Log("pre-log OnGameplaySkillAction " + _ability + " " + _score);
            BufferizeLogPlayData(new LogAI.PlayResultParameters() {
                playEvent = PlayEvent.Skill,
                skill = _ability,
                score = _score,
            });
        }
        */

        #endregion

        #region Gameplay

        /// <summary>
        /// Bufferizes the log play data.
        /// @note: deprecated (unless we re-add minigame direct logplay logging)
        /// </summary>
        /// <param name="_playResultParameters">The play result parameters.</param>
        void BufferizeLogPlayData(LogAI.PlayResultParameters _playResultParameters)
        {
            logPlayBuffer.Add(_playResultParameters);
        }

        /// <summary>
        /// Flushes the log play to app teacher log intellingence.
        /// @note: deprecated (unless we re-add minigame direct logplay logging)
        /// /// </summary>
        void FlushLogPlay()
        {
            LogManager.I.LogPlay(sessionName, miniGameCode, logPlayBuffer);
        }

        #endregion

        #region Learn

        /// <summary>
        /// Bufferizes the log learn data.
        /// </summary>
        /// <param name="_iLivingLetterAnswerData">The i living letter answer data.</param>
        void BufferizeLogLearnData(ILivingLetterAnswerData _iLivingLetterAnswerData)
        {
            logLearnBuffer.Add(_iLivingLetterAnswerData);
        }

        /// <summary>
        /// Flushes the log learn data to app teacher log intellingence.
        /// </summary>
        void FlushLogLearn()
        {
            Dictionary<string, LogAI.LearnResultParameters> resultsDict = new Dictionary<string, LogAI.LearnResultParameters>();

            /* Testing addition
            for (int i = 0; i < 100; i++)
            {
                var lp = new ILivingLetterAnswerData();
                lp._data = Random.value > 0.5f ? new LL_LetterData("alef") : new LL_LetterData("teh");
                lp._isPositiveResult = Random.value > 0.5f;
                logLearnBuffer.Add(lp);
            }*/

            foreach (var l in logLearnBuffer)
            {
                if (l._data == null)
                    continue;

                LogAI.LearnResultParameters resultsData = null;
                if (!resultsDict.ContainsKey(l._data.Id))
                {
                    resultsData = new LogAI.LearnResultParameters();
                    resultsData.elementId = l._data.Id;
                    resultsDict[l._data.Id] = resultsData;

                    switch (l._data.DataType)
                    {
                        case LivingLetterDataType.Letter:
                            resultsData.dataType = Database.VocabularyDataType.Letter;
                            break;
                        case LivingLetterDataType.Word:
                            resultsData.dataType = Database.VocabularyDataType.Word;
                            break;
                        case LivingLetterDataType.Image:
                            resultsData.dataType = Database.VocabularyDataType.Word;
                            break;
                        default:
                            // data type not found. Make soft exception.
                            break;
                    }
                }
                resultsData = resultsDict[l._data.Id];

                if (l._isPositiveResult)
                {
                    resultsData.nCorrect++;
                }
                else
                {
                    resultsData.nWrong++;
                }
            }

            var resultsList = resultsDict.Values.ToList();
            LogManager.I.LogLearn(sessionName, miniGameCode, resultsList, logLearnBuffer);
        }

        #endregion

        #region internal data structures and interfaces

        interface IBufferizableLog
        {
            string CachedType { get; }
        }

        public struct ILivingLetterAnswerData : IBufferizableLog
        {
            public string CachedType
            {
                get { return "ILivingLetterAnswerData"; }
            }

            public ILivingLetterData _data;
            public bool _isPositiveResult;
        }

        #endregion
    }
}
