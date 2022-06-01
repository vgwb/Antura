using System;
using Antura.Core;
using Antura.Language;
using UnityEditor;
using UnityEngine;

namespace Antura.Tools
{
    public class EditionsEditorWindow : EditorWindow
    {
        [MenuItem("Antura/Tools/Edition and Profiles")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(EditionsEditorWindow));
        }

        void OnGUI()
        {
            this.titleContent.text = "Antura - Editions & Profiles";

            AppManager appManager = FindObjectOfType<AppManager>();
            appManager.AppSettingsManager = new AppSettingsManager();

            EditorGUILayout.LabelField("App Edition: " + appManager.AppEdition.editionID.ToString());
            EditorGUILayout.LabelField("Native Language: " + appManager.AppSettings.NativeLanguage.ToString());
            EditorGUILayout.LabelField("Content Edition: " + appManager.ContentEdition.ContentID.ToString());

            EditorGUILayout.LabelField("Change Native Language:");
            EditorGUILayout.BeginVertical();
            foreach (var lang in Enum.GetNames(typeof(LanguageCode)))
            {
                if ((lang == "NONE") || (lang == "COUNT"))
                    continue;
                if (GUILayout.Button(lang))
                {
                    //FixAddressables(lang);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
