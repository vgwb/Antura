#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.EditorUtilities
{
    public static class QuestSubjectsEditorUtility
    {
private const int MaxSubjects = 5;

        [MenuItem("Antura/Discover/Quests/Refresh Subjects (Selected)")]
        public static void RefreshSelected()
        {
            var quests = Selection.objects.OfType<QuestData>().ToList();
            if (quests.Count == 0)
            {
                EditorUtility.DisplayDialog("Refresh Subjects", "Select one or more QuestData assets in the Project window.", "OK");
                return;
            }

            Undo.RecordObjects(quests.ToArray(), "Refresh Subjects");
            int updated = 0;
            foreach (var q in quests)
            {
                if (q == null)
                    continue;
                var list = QuestSubjectsUtility.ComputeSubjectsBreakdown(q);
                var topSubjects = list.Take(MaxSubjects).ToList();
                bool changed = q.Subjects == null || q.Subjects.Count != topSubjects.Count || !q.Subjects.SequenceEqual(topSubjects);
                if (changed)
                {
                    q.Subjects = topSubjects;
                    EditorUtility.SetDirty(q);
                    updated++;
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"Refreshed Subjects for {updated} quest(s).");
        }

        [MenuItem("Antura/Discover/Quests/Refresh Subjects (All)")]
        public static void RefreshAll()
        {
            var guids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var q = AssetDatabase.LoadAssetAtPath<QuestData>(path);
                if (q != null)
                    quests.Add(q);
            }
            if (quests.Count == 0)
            {
                Debug.Log("No QuestData assets found.");
                return;
            }

            Undo.RecordObjects(quests.ToArray(), "Refresh Subjects (All)");
            int updated = 0;
            foreach (var q in quests)
            {
                var list = QuestSubjectsUtility.ComputeSubjectsBreakdown(q);
                var topSubjects = list.Take(MaxSubjects).ToList();
                bool changed = q.Subjects == null || q.Subjects.Count != topSubjects.Count || !q.Subjects.SequenceEqual(topSubjects);
                if (changed)
                {
                    q.Subjects = topSubjects;
                    EditorUtility.SetDirty(q);
                    updated++;
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"Refreshed Subjects for {updated}/{quests.Count} quest(s).");
        }
    }
}
#endif
