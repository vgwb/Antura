using System;
using System.Collections.Generic;
using Antura.Core;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Detail profile of player formatted for Classroom.
    /// Used as a stub for now and to represent the required data,
    /// which in the future will be added (I guess?) to ContentProfile,
    /// at which point ContentProfile will be able to replace this class
    /// </summary>
    public class ClassroomProfileDetail
    {
        public readonly PlayerProfilePreview ProfilePreview;
        public readonly DateTime LastAccess;
        public readonly List<LanguageLevel> Levels;
        public readonly List<DiscoverQuest> Quests;

        public ClassroomProfileDetail(PlayerProfilePreview playerProfilePreview)
        {
            ProfilePreview = playerProfilePreview;

            var Profile = AppManager.I.PlayerProfileManager.GetPlayerProfileByUUID(ProfilePreview.Uuid);

            // Generate STUB last access
            LastAccess = DateTime.Now;

            // Generate STUB levels and quests until they will be implemented correctly
            Levels = new List<LanguageLevel>()
            {
                // new LanguageLevel(
                //     "A language level",
                //     new List<LanguageLevelSection>() {
                //         new LanguageLevelSection("A section", 0.4f),
                //         new LanguageLevelSection("Another section", 0.2f),
                //     }
                // ),
                // new LanguageLevel(
                //     "Another language level",
                //     new List<LanguageLevelSection>() {
                //         new LanguageLevelSection("A section", 0.8f),
                //         new LanguageLevelSection("Another section", 1f),
                //     }
                // )
            };
            Quests = new List<DiscoverQuest>();

            foreach (var savedQuest in Profile.Quests)
            {
                Quests.Add(new DiscoverQuest(savedQuest.QuestCode, savedQuest.Score));
            }

        }

        #region Test

        public static ClassroomProfileDetail GenerateStub(PlayerProfilePreview profile)
        {
            return new ClassroomProfileDetail(profile);
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
