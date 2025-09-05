using Antura.Dog;
using Antura.Language;
using Antura.Profile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Helpers;
using Antura.Minigames;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics;

namespace Antura.Core.Services.OnlineAnalytics
{
    public class Analytics : MonoBehaviour
    {

        /// <summary>
        ///
        /// TODO WIP: this methos saves the gameplay summary to remote/online analytics
        /// data is passed by the LogGamePlayData class
        ///
        /// 1 - Uuid: the unique player id
        /// 2 - app version(json app version + platform + device type (tablet/smartphone))
        /// 3 - player age(int) - player genre(string M/F)
        ///
        /// 4 - Journey Position(string Stage.LearningBlock.PlaySession)
        /// 5 - MiniGame(string code)
        ///
        /// - playtime(int seconds how long the gameplay)
        /// - launch type(from Journey or from Book)
        /// - end type(natural game end or forced exit)
        ///
        /// - difficulty(float from minigame config)
        /// - number of rounds(int from minigame config)
        /// - result(int 0,1,2,3 bones)
        ///
        /// - good answers(comma separated codes of vocabulary data)
        /// - wrong answers(comma separated codes of vocabulary data)
        /// - gameplay errors(say the lives in ColorTickle or anything not really related to Learning data)
        ///
        /// 10 - additional(json encoded additional parameters that we don't know now or custom specific per minigame)
        /// </summary>

        private bool AnalyticsEnabled => AppManager.I.AppEdition.OnlineAnalyticsEnabled && AppManager.I.AppSettingsManager.Settings.ShareAnalyticsEnabled;

        public Analytics()
        {
        }

        async void Awake()
        {
            var options = new InitializationOptions();
            if (DebugConfig.I.DevEnvironment)
            {
                options.SetEnvironmentName("dev");
                Debug.LogWarning("Analytics in DEV environment");
            }
            await UnityServices.InitializeAsync(options);
        }

        public void Init()
        {
            //  Debug.Log("init AnalyticsService");
        }

        private void AddSharedParameters(CustomEvent myEvent)
        {
            myEvent.Add("myPlayerUuid", AppManager.I.AppSettings.LastActivePlayerUUID.ToString());
            myEvent.Add("myEdition", AppManager.I.AppSettings.ContentID.ToString());
            myEvent.Add("myNativeLang", LanguageUtilities.GetISO2Code(AppManager.I.AppSettings.NativeLanguage));
        }

        public void TestEvent()
        {
            if (!AnalyticsEnabled)
                return;
            var myEvent = new CustomEvent("myTestEvent")
            {
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
            AnalyticsService.Instance.Flush();
            Debug.Log("Analytics TestEvent");
        }

        public void TrackCompletedRegistration(PlayerProfile playerProfile)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myCompletedRegistration")
            {
                { "myGender", playerProfile.Gender.ToString() },
                { "myAge", playerProfile.Age },
                { "myAvatar_Face", playerProfile.AvatarId },
                { "myAvatar_BgColor", ColorUtility.ToHtmlStringRGB(playerProfile.BgColor) },
                { "myAvatar_HairColor", ColorUtility.ToHtmlStringRGB(playerProfile.HairColor) },
                { "myAvatar_SkinColor", ColorUtility.ToHtmlStringRGB(playerProfile.SkinColor) },
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackReachedJourneyPosition(JourneyPosition jp)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myLevelUp")
            {
                { "myJP", jp.Id },
                { "myStage", jp.Stage },
                { "myLearningBlock", jp.LearningBlock },
                { "myPlaySession", jp.PlaySession },
                { "myTotalPlayTime", 0 },
                { "myTotalStars", 0 },
                { "myTotalBones", 0 }
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackCompletedFirstContactPhase(FirstContactPhase phase)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myTutorialComplete")
            {
                { "myPhase", phase.ToString() }
            };
            AddSharedParameters(myEvent);

            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackItemBought(int nSpent, string boughtItemKey)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myItemBought")
            {
                { "myBonesSpent", nSpent },
                { "myBoughtItem", boughtItemKey}
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackCustomization(AnturaCustomization customization, float anturaSpacePlayTime)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myAnturaCustomize")
            {
                { "myAnturaSpace_playtime", (int)anturaSpacePlayTime }
            };

            var item = customization.PropPacks.FirstOrDefault(item => item.Category == "HEAD");
            if (item != null)
                myEvent.Add($"myAntura_Head", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "EAR_L");
            if (item != null)
                myEvent.Add($"myAntura_EarL", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "EAR_R");
            if (item != null)
                myEvent.Add($"myAntura_EarR", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "NOSE");
            if (item != null)
                myEvent.Add($"myAntura_Nose", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "JAW");
            if (item != null)
                myEvent.Add($"myAntura_Jaw", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "NECK");
            if (item != null)
                myEvent.Add($"myAntura_Neck", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "BACK");
            if (item != null)
                myEvent.Add($"myAntura_Back", item.BaseId);

            item = customization.PropPacks.FirstOrDefault(item => item.Category == "TAIL");
            if (item != null)
                myEvent.Add($"myAntura_Tail", item.BaseId);

            myEvent.Add($"myAntura_Texture", customization.TexturePack.BaseId);
            myEvent.Add($"myAntura_Decal", customization.DecalPack.BaseId);

            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackMiniGameScore(MiniGameCode miniGameCode, int score, JourneyPosition currentJourneyPosition, float duration)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myMinigameEnd")
            {
                { "myMinigame", miniGameCode.ToString() },
                { "myScore", score },
                { "myDuration", (int)duration },
                { "myJP", currentJourneyPosition.Id }
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackMood(int mood)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myPlayerMood")
            {
                { "myPlayerMood", mood }
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackBook(string _action, string _object)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myBook")
            {
                { "myAction", _action },
                { "myObject", _object }
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        public void TrackVocabularyDataScore(MiniGameCode miniGameCode, JourneyPosition currentJourneyPosition, List<MinigamesLogManager.ILivingLetterAnswerData> answers)
        {
            if (!AnalyticsEnabled)
                return;

            foreach (var answer in answers)
            {
                if (answer._data == null)
                    continue;
                var myEvent = new CustomEvent("myLearning")
                {
                    { "myMinigame", miniGameCode.ToString() },
                    { "myJP", currentJourneyPosition.Id },
                    { "myStage", currentJourneyPosition.Stage },
                    { "myLearningBlock", currentJourneyPosition.LearningBlock },
                    { "myPlaySession", currentJourneyPosition.PlaySession },
                    { "myLearningDataType", answer._data.DataType },
                    { "myLearningDataId", answer._data.Id },
                    { "myLearningIsCorrect", answer._isPositiveResult },
                };
                AddSharedParameters(myEvent);
                AnalyticsService.Instance.RecordEvent(myEvent);
            }
        }

        public void TrackGenericAction(string eventName)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myGenericAction")
            {
                { "myAction", eventName }
            };
            AnalyticsService.Instance.RecordEvent(myEvent);
        }

        #region Older Events
        public void TrackScene(string sceneName)
        {
        }

        public void TrackKioskEvent(string eventName)
        {
            if (!AnalyticsEnabled)
                return;
            // var eventData = new Dictionary<string, object>{
            //         { "app", "kiosk" },
            //         {"lang", (AppManager.I.AppSettings.AppLanguage == AppLanguages.Italian ? "it" : "en")}
            //     };
            // Analytics.CustomEvent(eventName, eventData);
        }


        public void TrackPlayerSession(int age, Profile.PlayerGender gender)
        {
            if (!AnalyticsEnabled)
                return;
            //Gender playerGender = (gender == Profile.PlayerGender.F ? Gender.Female : Gender.Male);
            //Analytics.SetUserGender(playerGender);
            //int birthYear = DateTime.Now.Year - age;
            //Analytics.SetUserBirthYear(birthYear);
        }

        #endregion
    }
}
