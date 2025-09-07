#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Editor
{
    /// <summary>
    /// Creates a new CardData from a selected TopicData.
    /// The new card becomes the CoreCard of the Topic and the previous core (if any) is added as a related connection.
    /// Menu appears only when exactly one TopicData is selected.
    /// </summary>
    public static class CreateCardFromTopicSelection
    {
        private const string MenuPath = "Assets/Antura/Discover/Create Card from selected Topic";

        [MenuItem(MenuPath, priority = 2201)]
        public static void Create()
        {
            var topic = Selection.activeObject as TopicData;
            if (topic == null)
            {
                EditorUtility.DisplayDialog("Create Card", "Select a TopicData asset in the Project window.", "OK");
                return;
            }

            // Keep reference to previous core to convert it into a connection
            var previousCore = topic.CoreCard;

            // Determine output folder based on topic asset path
            string topicPath = AssetDatabase.GetAssetPath(topic);
            string folder = string.IsNullOrEmpty(topicPath) ? "Assets" : Path.GetDirectoryName(topicPath);

            // Build base name from topic name or asset name
            string baseName = !string.IsNullOrEmpty(topic.Name) ? topic.Name : topic.name;
            string fileName = $"Card_{SanitizeFileName(baseName)}.asset";
            string outPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, fileName));

            var card = ScriptableObject.CreateInstance<CardData>();
            card.name = Path.GetFileNameWithoutExtension(outPath);
            card.TitleEn = baseName;
            card.DescriptionEn = $"Auto-created card for topic '{baseName}'.";
            card.CoreOfTopic = topic; // mark reverse link

            AssetDatabase.CreateAsset(card, outPath);
            EditorUtility.SetDirty(card);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Assign as new core
            topic.CoreCard = card;

            // If previous core existed and is different, add it as a connection (avoid duplicate)
            if (previousCore != null && previousCore != card)
            {
                bool alreadyConnected = false;
                foreach (var c in topic.Connections)
                {
                    if (c.ConnectedCard == previousCore)
                    {
                        alreadyConnected = true;
                        break;
                    }
                }
                if (!alreadyConnected)
                {
                    topic.Connections.Add(new CardConnection
                    {
                        ConnectedCard = previousCore,
                        ConnectionType = ConnectionType.RelatedTo,
                        ConnectionStrength = 0.6f,
                        ConnectionReason = "Previous core card"
                    });
                }
            }

            EditorUtility.SetDirty(topic);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = card;
            EditorGUIUtility.PingObject(card);
        }

        [MenuItem(MenuPath, validate = true)]
        public static bool ValidateCreate()
        {
            if (Selection.objects == null || Selection.objects.Length != 1)
                return false;
            return Selection.activeObject is TopicData;
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "Card";
            foreach (var ch in Path.GetInvalidFileNameChars())
                name = name.Replace(ch, '-');
            return name.Trim();
        }
    }
}
#endif
