using System;
using SQLite;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Data defining a Reward that can be earned for customization of Antura.
    /// </summary>
    [Serializable]
    public class RewardData : IData
    {
        [PrimaryKey]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [SerializeField]
        private string _Id;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        [SerializeField]
        private string _Title;

        /*public RewardDataCategory Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        [SerializeField]
        private RewardDataCategory _Category;*/

        public int Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
        [SerializeField]
        private int _Weight;

        public override string ToString()
        {
            return Id + ": " + Title;
        }

        public string GetId()
        {
            return Id;
        }
    }

}
