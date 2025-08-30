#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

// Localization
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Metadata;
// Editor Localization APIs
using UnityEditor.Localization;
using UnityEditor.Localization.Reporting;

using Antura.Discover;
namespace Antura.Discover.Editor
{
    public static class AddCommentExample
    {

        [MenuItem("Antura/Experimental/Localization/Add Shared Comment", priority = 1000)]
        public static void AddSharedCommentExample()
        {
            AddSharedComment("Quests France", "line:0a24992", "Short context for translators.");
        }

        public static void AddSharedComment(string collectionName, string key, string commentText)
        {
            // var coll = LocalizationEditorSettings.GetStringTableCollection(collectionName);
            // if (coll == null)
            // { UnityEngine.Debug.LogError($"No StringTableCollection '{collectionName}'"); return; }

            // var shared = coll.SharedData;
            // var sharedEntry = shared.GetEntry(key);     // or GetEntry(id) if you have the numeric ID
            // if (sharedEntry == null)
            // { UnityEngine.Debug.LogError($"Key '{key}' not found in '{collectionName}'"); return; }

            // var c = sharedEntry.
            // Metadata.AddMetadata(commentText)
            // if (c == null)
            // {
            //     c = new Comment { CommentText = commentText };
            //     sharedEntry.AddMetadata(c);            // attaches Comment to the *shared* row
            // }
            // else
            // {
            //     c.CommentText = commentText;          // update existing
            // }

            // EditorUtility.SetDirty(shared);            // SharedTableData is its own asset
            // AssetDatabase.SaveAssets();
        }

    }
}
#endif
