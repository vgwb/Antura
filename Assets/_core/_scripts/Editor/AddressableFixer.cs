using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Antura.Tools
{
    public class AddressableFixer : EditorWindow
    {
        [MenuItem("Antura/Tools/Fix Language Addressables")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(AddressableFixer));
        }

        void OnGUI()
        {
            this.titleContent.text = "Antura";
            EditorGUILayout.LabelField("Regenerate Addressables info for language:");
            DrawFooterLayout(Screen.width);
        }

        public void DrawFooterLayout(float width)
        {
            EditorGUILayout.BeginVertical();

            foreach (var lang in Enum.GetNames(typeof(LanguageCode)))
            {
                if ((lang == "NONE") || (lang == "COUNT"))
                    continue;
                if (GUILayout.Button(lang))
                {
                    FixAddressables(lang);
                }
            }
            EditorGUILayout.EndVertical();
        }

        public void FixAddressables(string lang)
        {
            // TODO: Get all assets in the lang paths for the current edition
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_lang_bundles/" + lang });
            Debug.Log("Fixing addressable for lang: " + lang);
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var removedPathLength = "Assets/_lang_bundles/".Length;
                var splits = path.Split('.');
                if (splits.Length > 1)
                {
                    var extension = splits[splits.Length - 1];
                    path = path.Substring(0, path.Length - (extension.Length + 1));
                }
                entry.address = path.Substring(removedPathLength, path.Length - removedPathLength);
            }
            Debug.Log("FINISHED Fixing addressable for lang: " + lang);
        }

    }
}
