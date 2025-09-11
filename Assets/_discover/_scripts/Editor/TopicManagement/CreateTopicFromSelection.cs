#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Editor
{
    public static class CreateTopicFromSelection
    {
        private const string MenuPath = "Assets/Antura/Discover/Create Topic from selected Cards";

        [MenuItem(MenuPath, priority = 2200)]
        public static void Create()
        {
            var selected = Selection.GetFiltered<CardData>(SelectionMode.Assets)?.Where(c => c != null).ToList() ?? new List<CardData>();
            if (selected.Count == 0)
            {
                EditorUtility.DisplayDialog("Create Topic", "Select at least one CardData asset in the Project window.", "OK");
                return;
            }

            // Prefer active selection as core when possible
            CardData core = Selection.activeObject as CardData;
            if (core == null || !selected.Contains(core))
                core = selected[0];

            // Output folder: same directory as the core card asset
            string corePath = AssetDatabase.GetAssetPath(core);
            string folder = string.IsNullOrEmpty(corePath) ? "Assets" : Path.GetDirectoryName(corePath);
            string baseName = !string.IsNullOrEmpty(core.Id) ? core.Id : core.name;
            string fileName = $"Topic_{SanitizeFileName(baseName)}.asset";
            string outPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, fileName));

            var topic = ScriptableObject.CreateInstance<TopicData>();
            topic.name = Path.GetFileNameWithoutExtension(outPath);
            topic.CoreCard = core;
            topic.Name = PublishUtils.SafeLocalized(PublishUtils.GetLocalizedString(core, "Title"), fallback: baseName);

            // Add other selected cards as connections
            var unique = new HashSet<CardData>(selected);
            unique.Remove(core);
            foreach (var card in unique)
            {
                if (card == null)
                    continue;
                topic.Connections.Add(new CardConnection
                {
                    ConnectedCard = card,
                    ConnectionType = ConnectionType.RelatedTo,
                    ConnectionStrength = 0.7f,
                    ConnectionReason = string.Empty
                });
            }

            AssetDatabase.CreateAsset(topic, outPath);
            EditorUtility.SetDirty(topic);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = topic;
            EditorGUIUtility.PingObject(topic);
        }

        [MenuItem(MenuPath, validate = true)]
        public static bool ValidateCreate()
        {
            var selected = Selection.GetFiltered<CardData>(SelectionMode.Assets);
            return selected != null && selected.Length > 0;
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Topic";
            foreach (var ch in Path.GetInvalidFileNameChars())
                name = name.Replace(ch, '-');
            return name.Trim();
        }
    }
}
#endif
