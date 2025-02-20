using System;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class UserProfile
    {
        public const string NoClassroomId = "-";
        public readonly string Id; // Used to get UserProfileDetail
        public readonly string Name;
        public readonly Sprite ProfilePic;
        public readonly DateTime LastAccess;

        public UserProfile(string id, string name, Sprite profilePic, DateTime lastAccess)
        {
            Id = id;
            ProfilePic = profilePic;
            Name = name;
            LastAccess = lastAccess;
        }

        #region Public Methods

        public UserProfileDetail GetProfileDetail()
        {
            // TODO: Method to get UserProfileDetail from an ID - For now returns a stub UserProfileDetail
            return UserProfileDetail.GenerateStub(Id, Name, ProfilePic, LastAccess);
        }

        #endregion
    }
}