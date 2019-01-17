using Antura.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Core
{
    /// <summary>
    /// App Log Manager. Use this to log any event from app.
    /// </summary>
    public class LogManager
    {
        public static LogManager I;

        int _appSession;

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public int AppSession
        {
            get { return _appSession; }
            private set { _appSession = value; }
        }

        public LogManager()
        {
            I = this;
            InitNewSession();
        }

        public void InitNewSession()
        {
            AppSession = GenericHelper.GetTimestampForNow();
        }

        #region Time Logging

        System.DateTime startPlaySessionDateTime;
        System.DateTime startMiniGameDateTime;
        System.DateTime endPlaySessionDateTime;
        System.DateTime endMiniGameDateTime;

        public void StartPlaySession()
        {
            startPlaySessionDateTime = System.DateTime.Now;
        }

        public void EndPlaySession()
        {
            endPlaySessionDateTime = System.DateTime.Now;
        }

        public void StartMiniGame()
        {
            startMiniGameDateTime = System.DateTime.Now;
        }

        public void EndMiniGame()
        {
            endMiniGameDateTime = System.DateTime.Now;
        }

        #endregion

        #region Proxy From Minigame log manager provider To App Log Intellingence

        protected internal void LogMinigameScore(string playSession, MiniGameCode miniGameCode, int score)
        {
            EndMiniGame();
            LogInfo(InfoEvent.GameEnd, JsonUtility.ToJson(new GameResultInfo() { Game = miniGameCode.ToString(), Result = score.ToString() }));


            float duration = (float)(endMiniGameDateTime - startMiniGameDateTime).TotalSeconds;

            AppManager.I.Services.Analytics.TrackMiniGameScore(miniGameCode, score, AppManager.I.NavigationManager.NavData.CurrentPlayer.CurrentJourneyPosition, duration);
            //Debug.LogError("DURATION MG: " + duration);
            AppManager.I.Teacher.logAI.LogMiniGameScore(AppSession, AppManager.I.NavigationManager.NavData.CurrentPlayer.CurrentJourneyPosition, miniGameCode, score, duration);
        }

        struct GameResultInfo
        {
            public string Game;
            public string Result;
        }

        /// @note: deprecated (unless we re-add minigame direct logplay logging)
        protected internal void LogPlay(string playSession, MiniGameCode miniGameCode, List<Teacher.LogAI.PlayResultParameters> resultsList)
        {
            AppManager.I.Teacher.logAI.LogPlay(AppSession, AppManager.I.NavigationManager.NavData.CurrentPlayer.CurrentJourneyPosition, miniGameCode, resultsList);
        }

        protected internal void LogLearn(string playSession, MiniGameCode miniGameCode,
            List<Teacher.LogAI.LearnResultParameters> resultsList)
        {
            AppManager.I.Teacher.logAI.LogLearn(AppSession, AppManager.I.NavigationManager.NavData.CurrentPlayer.CurrentJourneyPosition, miniGameCode, resultsList);
        }

        #endregion

        #region public API        

        /// <summary>
        /// Logs the play session score.
        /// </summary>
        /// <param name="playSessionId">The play session identifier.</param>
        /// <param name="score">The score.</param>
        public void LogPlaySessionScore(string playSessionId, int score)
        {
            EndPlaySession();

            //Debug.Log("LOG PLAY SESSION SCORE: " + AppManager.I.NavigationManager.NavData.CurrentPlayer.CurrentJourneyPosition + " : score: " + score);
            float duration = (float)(endPlaySessionDateTime - startPlaySessionDateTime).TotalSeconds;
            //Debug.LogError("DURATION PS: " + duration);
            AppManager.I.Teacher.logAI.LogPlaySessionScore(AppSession, AppManager.I.NavigationManager.NavData.CurrentPlayer.CurrentJourneyPosition, score, duration);
        }

        /// <summary>
        /// Logs the generic information.
        /// </summary>
        /// <param name="infoEvent">The information event.</param>
        /// <param name="parametersString">The parameters string.</param>
        public void LogInfo(InfoEvent infoEvent, string parametersString = "")
        {
            AppManager.I.Teacher.logAI.LogInfo(AppSession, infoEvent, parametersString);
        }

        /// <summary>
        /// Logs the mood.
        /// </summary>
        /// <param name="mood">The mood.</param>
        public void LogMood(int mood)
        {
            AppManager.I.Teacher.logAI.LogMood(AppSession, mood);
        }

        public void StartApp()
        {
            LogInfo(InfoEvent.AppSessionStart, "{\"AppSession\":\"" + AppSession + "\"}");
        }

        #endregion
    }
}