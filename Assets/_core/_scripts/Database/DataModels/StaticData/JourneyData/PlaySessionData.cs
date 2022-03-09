using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Helpers;
using SQLite;
using UnityEngine;

namespace Antura.Database
{

    /// <summary>
    /// Data defining a Play Session.
    /// Used to define the learning journey progression.
    /// Learning Blocks contain one or more play sessions and end with an assessment.
    /// A Play Session contains one or more minigames that can be selected to play when reaching that play session.
    /// <seealso cref="StageData"/>
    /// <seealso cref="LearningBlockData"/>
    /// <seealso cref="MiniGameData"/>
    /// </summary>
    [Serializable]
    public class PlaySessionData : IData
    {
        [PrimaryKey]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [SerializeField]
        private string _Id;

        public int Stage
        {
            get { return _Stage; }
            set { _Stage = value; }
        }
        [SerializeField]
        private int _Stage;

        public int LearningBlock
        {
            get { return _LearningBlock; }
            set { _LearningBlock = value; }
        }
        [SerializeField]
        private int _LearningBlock;

        public int PlaySession
        {
            get { return _PlaySession; }
            set { _PlaySession = value; }
        }
        [SerializeField]
        private int _PlaySession;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        [SerializeField]
        private string _Type;

        public PlaySessionDataOrder Order
        {
            get { return _Order; }
            set { _Order = value; }
        }
        [SerializeField]
        private PlaySessionDataOrder _Order;

        public int NumberOfMinigames
        {
            get { return _NumberOfMinigames; }
            set { _NumberOfMinigames = value; }
        }
        [SerializeField]
        private int _NumberOfMinigames;

        [Ignore]
        public MiniGameInPlaySession[] Minigames
        {
            get { return _Minigames; }
            set { _Minigames = value; }
        }
        [SerializeField]
        private MiniGameInPlaySession[] _Minigames;
        public string Minigames_list
        {
            get { return Minigames.ToJoinedString(); }
            set { }
        }

        public int NumberOfRoundsPerMinigame
        {
            get { return _NumberOfRoundsPerMinigame; }
            set { _NumberOfRoundsPerMinigame = value; }
        }
        [SerializeField]
        private int _NumberOfRoundsPerMinigame;


        [Ignore]
        public string[] Letters
        {
            get { return _Letters; }
            set { _Letters = value; }
        }
        [SerializeField]
        private string[] _Letters;
        public string Letters_list
        {
            get { return Letters.ToJoinedString(); }
            set { }
        }

        [Ignore]
        public string[] Words
        {
            get { return _Words; }
            set { _Words = value; }
        }
        [SerializeField]
        private string[] _Words;
        public string Words_list
        {
            get { return Words.ToJoinedString(); }
            set { }
        }


        [Ignore]
        public string[] Words_previous
        {
            get { return _Words_previous; }
            set { _Words_previous = value; }
        }
        [SerializeField]
        private string[] _Words_previous;
        public string Words_previous_list
        {
            get { return Words_previous.ToJoinedString(); }
            set { }
        }

        [Ignore]
        public string[] Phrases
        {
            get { return _Phrases; }
            set { _Phrases = value; }
        }
        [SerializeField]
        private string[] _Phrases;
        public string Phrases_list
        {
            get { return Phrases.ToJoinedString(); }
            set { }
        }

        [Ignore]
        public string[] Phrases_previous
        {
            get { return _Phrases_previous; }
            set { _Phrases_previous = value; }
        }
        [SerializeField]
        private string[] _Phrases_previous;

        public bool IsMiniGamePS =>
            PlaySession != JourneyPosition.ASSESSMENT_PLAY_SESSION_INDEX
            && PlaySession != JourneyPosition.ENDGAME_PLAY_SESSION_INDEX;

        public string Phrases_previous_list
        {
            get { return Phrases_previous.ToJoinedString(); }
            set { }
        }

        public string GetId()
        {
            return Id;
        }

        public override string ToString()
        {
            string output = "";
            output += string.Format("[PlaySession: LB={0}, PS={1}]", Stage, LearningBlock, PlaySession);
            output += "\n MiniGames:";
            foreach (var minigame in Minigames)
            {
                if (minigame.Weight == 0)
                    continue;
                output += "\n      " + minigame.MiniGameCode + ": \t" + minigame.Weight;
            }
            return output;
        }

        public JourneyPosition GetJourneyPosition()
        {
            return new JourneyPosition(Stage, LearningBlock, PlaySession);
        }
    }

    [Serializable]
    public struct MiniGameInPlaySession
    {
        public MiniGameCode MiniGameCode;
        public int Weight;
        // TODO maybe pass some info straight from the PlaySession table
        // public int NumberOfRounds;
        // public float Difficulty;
        // public string SpecialParameters;

        public override string ToString()
        {
            return MiniGameCode + ":" + Weight;
        }
    }


}
