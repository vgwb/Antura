using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Profile
{
    public class UserProfileDetail
    {
        public readonly string Id; // Used to get UserProfileDetail
        public readonly string Name;
        public readonly Sprite ProfilePic;
        public readonly DateTime LastAccess;
        public readonly List<LanguageLevel> Levels;
        public readonly List<DiscoverQuest> Quests;

        public UserProfileDetail(string id, string name, Sprite profilePic, DateTime lastAccess, List<LanguageLevel> levels, List<DiscoverQuest> quests)
        {
            Id = id;
            Name = name;
            ProfilePic = profilePic;
            LastAccess = lastAccess;
            Levels = levels;
            Quests = quests;
        }

        #region Test

        public static UserProfileDetail GenerateStub(string id, string name, Sprite profilePic, DateTime lastAccess)
        {
            return new UserProfileDetail(
                id,
                name,
                profilePic,
                lastAccess,
                new List<LanguageLevel>() {
                    new LanguageLevel(
                        "A language level",
                        new List<LanguageLevelSection>() {
                            new LanguageLevelSection("A section", 0.4f),
                            new LanguageLevelSection("Another section", 0.2f),
                        }
                    ),
                    new LanguageLevel(
                        "Another language level",
                        new List<LanguageLevelSection>() {
                            new LanguageLevelSection("A section", 0.8f),
                            new LanguageLevelSection("Another section", 1f),
                        }
                    )
                },
                new List<DiscoverQuest>() {
                    new DiscoverQuest("Quest A", 2),
                    new DiscoverQuest("Quest B", 3)
                }
            );
        }

        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public class LanguageLevel
        {
            public readonly string Name;
            public readonly List<LanguageLevelSection> Sections;

            public LanguageLevel(string name, List<LanguageLevelSection> sections)
            {
                Name = name;
                Sections = sections;
            }
        }
        
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public class LanguageLevelSection
        {
            public readonly string Name;
            public readonly float CompletionPerc; // 0 to 1

            public LanguageLevelSection(string name, float completionPerc)
            {
                Name = name;
                CompletionPerc = completionPerc;
            }
        }
        
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public class DiscoverQuest
        {
            public readonly string Name;
            public readonly int Stars;

            public DiscoverQuest(string name, int stars)
            {
                Name = name;
                Stars = stars;
            }
        }
    }
}