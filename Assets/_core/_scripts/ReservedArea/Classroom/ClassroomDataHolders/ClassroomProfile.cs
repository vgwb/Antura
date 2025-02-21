using System;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Profile of player formatted for Classroom
    /// </summary>
    public class ClassroomProfile
    {
        public const string NoClassroomId = "-";
        public readonly string Id; // Used to get UserProfileDetail
        public readonly string Name;
        public readonly Sprite ProfilePic;
        public readonly DateTime LastAccess;

        public ClassroomProfile(string id, string name, Sprite profilePic, DateTime lastAccess)
        {
            Id = id;
            ProfilePic = profilePic;
            Name = name;
            LastAccess = lastAccess;
        }

        #region Public Methods

        public ClassroomProfileDetail GetProfileDetail()
        {
            // TODO: Method to get UserProfileDetail from an ID - For now returns a stub UserProfileDetail
            return ClassroomProfileDetail.GenerateStub(Id, Name, ProfilePic, LastAccess);
        }

        #endregion
    }
}