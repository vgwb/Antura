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
        [MenuItem("Antura/Utility/Fix Language Addressables")]
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
            if (GUILayout.Button("COMMON"))
            {
                AppEditionConfig.FixAddressables("common");
            }

            foreach (var lang in Enum.GetNames(typeof(LanguageCode)))
            {
                if ((lang == "NONE") || (lang == "COUNT"))
                    continue;
                if (GUILayout.Button(lang))
                {
                    AppEditionConfig.FixAddressables(lang);
                }
            }
            EditorGUILayout.EndVertical();
        }

    }
}
