using System;
using SQLite;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Data defining a Stage.
    /// Defines the learning journey progression.
    /// A Stage contains multiple Learning Blocks.
    /// Each Stage is shown as a specific sub-map in the Map scene.
    /// <seealso cref="LearningBlockData"/>
    /// </summary>
    [Serializable]
    public class StageData : IData
    {
        [PrimaryKey]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [SerializeField] private string _Id;

        public string Title_En
        {
            get { return _Title_En; }
            set { _Title_En = value; }
        }
        [SerializeField] private string _Title_En;

        public string Title_Ar
        {
            get { return _Title_Ar; }
            set { _Title_Ar = value; }
        }
        [SerializeField] private string _Title_Ar;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        [SerializeField] private string _Description;

        public override string ToString()
        {
            return Id + ": " + Title_En;
        }

        public string GetId()
        {
            return Id;
        }
    }
}
