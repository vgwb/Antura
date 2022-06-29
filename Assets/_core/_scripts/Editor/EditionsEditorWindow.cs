using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Language;
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

        private static void Rebuild()
        {
            appManager = FindObjectOfType<AppManager>();
            if (appManager.AppSettingsManager == null) appManager.AppSettingsManager = new AppSettingsManager();
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
        }

        [MenuItem("Antura/Tools/Edition and Profiles")]
        public static void ShowWindow()
        {
            Rebuild();

            var w = GetWindow(typeof(EditionsEditorWindow)) as EditionsEditorWindow;
            Debug.LogError(appManager.AppEdition.editionID);
            w.selectedAppEditionIndex = appEditions.IndexOf(appManager.AppEdition.editionID);
            Debug.LogError(appManager.AppSettings.ContentID);
            w.selectedContentEditionIndex = learningContents.IndexOf(appSettings.ContentID);
            Debug.LogError(appManager.AppSettings.NativeLanguage);
            w.selectedNativeLanguageIndex = languages.IndexOf(appSettings.NativeLanguage);
        }

        public int selectedAppEditionIndex;
        public int selectedContentEditionIndex;
        public int selectedNativeLanguageIndex;
        void OnGUI()
        {
            this.titleContent.text = "Antura - Editions & Profiles";

            Rebuild();

            EditorGUI.BeginChangeCheck();
            var newAppEditionIndex = EditorGUILayout.Popup("App Edition", selectedAppEditionIndex, appEditionNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (newAppEditionIndex != selectedAppEditionIndex)
                {
                    selectedAppEditionIndex = newAppEditionIndex;
                    var ed = appEditions[newAppEditionIndex];
                    Debug.Log(ed);
                    //               appManager.AppEdition = // TODO: find it in Assets/_config
                }
            }

            EditorGUI.BeginChangeCheck();
            var newContentEditionIndex = EditorGUILayout.Popup("Content Edition", selectedContentEditionIndex, learningContentNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if (newContentEditionIndex != selectedContentEditionIndex)
                {
                    selectedContentEditionIndex = newContentEditionIndex;
                    var ed = learningContents[newContentEditionIndex];
                    Debug.Log(ed);
                    appSettings.ContentID = ed;
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
                    var ed = languages[newNativeLanguageIndex];
                    Debug.Log(ed);
                }
            }
        }
    }
}
