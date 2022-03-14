using System;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Data defining a Learning Block.
    /// Used to define the learning journey progression.
    /// A Stage contains multiple Learning Blocks.
    /// Learning Blocks contain one or more play sessions and end with an assessment.
    /// <seealso cref="StageData"/>
    /// <seealso cref="PlaySessionData"/>
    /// </summary>
    [Serializable]
    public class LearningBlockData : IData
    {
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

        public int NumberOfPlaySessions
        {
            get { return _NumberOfPlaySessions; }
            set { _NumberOfPlaySessions = value; }
        }
        [SerializeField]
        private int _NumberOfPlaySessions;

        public string Description_NativeLang
        {
            get { return _Description_En; }
            set { _Description_En = value; }
        }
        [SerializeField]
        private string _Description_En;

        public string Description_LearningLang
        {
            get { return _Description_Ar; }
            set { _Description_Ar = value; }
        }
        [SerializeField]
        private string _Description_Ar;

        public string Title_NativeLang
        {
            get { return _Title_En; }
            set { _Title_En = value; }
        }
        [SerializeField]
        private string _Title_En;

        public string Title_LearningLang
        {
            get { return _Title_LearningLang; }
            set { _Title_LearningLang = value; }
        }
        [SerializeField]
        private string _Title_LearningLang;

        [SerializeField]
        private string _Title_Ar;

        public string AudioFile
        {
            get { return _AudioFile; }
            set { _AudioFile = value; }
        }
        [SerializeField]
        private string _AudioFile;

        public LearningBlockDataFocus Focus
        {
            get { return _Focus; }
            set { _Focus = value; }
        }
        [SerializeField]
        private LearningBlockDataFocus _Focus;

        //public string Reward;
        //public string AssessmentData;

        public string GetId()
        {
            return Id;
        }

        public override string ToString()
        {
            string output = "";
            output += string.Format("[LearningBlock: S={0}, LB={1}, description={2}]", Stage, LearningBlock, Description_NativeLang);
            return output;
        }
    }

}
