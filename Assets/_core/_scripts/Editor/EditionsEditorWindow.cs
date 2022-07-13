using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Debugging;
using Antura.Language;
using Antura.Profile;
using UnityEditor;
using UnityEngine;

namespace Antura.Tools
{
    public class EditionsEditorWindow : EditorWindow
    {
        static List<string> appEditionNames = new List<string>();
        static List<AppEditionID> appEditions = new List<AppEditionID>();

        static List<string> learningContentNames = new List<string>();
        static List<LearningContentID> learningContents = new List<LearningContentID>();

        static List<string> languageNames = new List<string>();
        static List<LanguageCode> languages = new List<LanguageCode>();

        private static AppManager appManager;
        private static AppSettings appSettings;

        private static EditionsEditorWindow window;

        private static void Rebuild()
        {
            appManager = FindObjectOfType<AppManager>();
            if (appManager.AppSettingsManager == null)
                appManager.AppSettingsManager = new AppSettingsManager();
            appSettings = appManager.AppSettingsManager.LoadSettings();

            appEditions.Clear();
            appEditionNames.Clear();
            foreach (AppEditionID v in Enum.GetValues(typeof(AppEditionID)))
            {
                appEditions.Add(v);
                appEditionNames.Add(v.ToString());
            }

            learningContents.Clear();
            learningContentNames.Clear();
            foreach (LearningContentID v in Enum.GetValues(typeof(LearningContentID)))
            {
                learningContents.Add(v);
                learningContentNames.Add(v.ToString());
            }

            languages.Clear();
            languageNames.Clear();
            foreach (LanguageCode v in Enum.GetValues(typeof(LanguageCode)))
            {
                languages.Add(v);
                languageNames.Add(v.ToString());
            }

            if (window == null)
                window = GetWindow(typeof(EditionsEditorWindow)) as EditionsEditorWindow;
            window.selectedAppEditionIndex = appEditions.IndexOf(appManager.AppEdition.editionID);
            window.selectedContentEditionIndex = learningContents.IndexOf(appSettings.ContentID);
            window.selectedNativeLanguageIndex = languages.IndexOf(appSettings.NativeLanguage);
        }

        [MenuItem("Antura/Edition selector", false, 300)]
        public static void ShowWindow()
        {
            Rebuild();
        }

        public int selectedAppEditionIndex;
        public int selectedContentEditionIndex;
        public int selectedNativeLanguageIndex;
        void OnGUI()
        {
            this.titleContent.text = "Antura - Editions & Profiles";

            Rebuild();

            EditorGUILayout.LabelField("AppEdition: " + appEditions[selectedAppEditionIndex]);

            /*EditorGUI.BeginChangeCheck();
            var newAppEditionIndex = EditorGUILayout.Popup("App Edition", selectedAppEditionIndex, appEditionNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (newAppEditionIndex != selectedAppEditionIndex)
                {
                    selectedAppEditionIndex = newAppEditionIndex;
                    var ed = appEditions[newAppEditionIndex];
                    Debug.Log(ed);
                }
            }*/

            EditorGUI.BeginChangeCheck();
            var newContentEditionIndex = EditorGUILayout.Popup("Content Edition", selectedContentEditionIndex, learningContentNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (newContentEditionIndex != selectedContentEditionIndex)
                {
                    selectedContentEditionIndex = newContentEditionIndex;
                    appSettings.ContentID = learningContents[newContentEditionIndex];
                    appManager.AppSettingsManager.SaveSettings();
                }
            }

            EditorGUI.BeginChangeCheck();
            var newNativeLanguageIndex = EditorGUILayout.Popup("Native Language", selectedNativeLanguageIndex, languageNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (newNativeLanguageIndex != selectedNativeLanguageIndex)
                {
                    selectedNativeLanguageIndex = newNativeLanguageIndex;
                    appSettings.NativeLanguage = languages[newNativeLanguageIndex];
                    appManager.AppSettingsManager.SaveSettings();
                }
            }

            if (GUILayout.Button("Add New Profile"))
            {
                DebugManager.I.CreateTestProfile();
            }

            var iconDatas = PlayerProfileManager.FilterPlayerIconData(appSettings, appManager.AppEdition.editionID, appSettings.ContentID);
            EditorGUILayout.LabelField("Profiles:");
            if (iconDatas.Count == 0)
            {
                EditorGUILayout.LabelField("NONE FOUND");
            }
            else
            {
                foreach (var iconData in iconDatas)
                {
                    if (appSettings.LastActivePlayerUUID == iconData.Uuid)
                    {
                        EditorGUILayout.LabelField(iconData.Uuid.Substring(0, 5) + " ACTIVE");
                    }
                    else
                    {
                        EditorGUILayout.LabelField(iconData.Uuid.Substring(0, 5));
                    }
                }
            }
        }
    }
}
