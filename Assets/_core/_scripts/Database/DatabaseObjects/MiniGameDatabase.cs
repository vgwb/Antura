#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for MiniGameData.
    /// </summary>
    public class MiniGameDatabase : AbstractDatabase
    {
        [SerializeField]
        public MiniGameTable table;

        public List<MiniGameData> ShownData = new List<MiniGameData>();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MiniGameDatabase))]
    public class MiniGameDatabaseEditor : Editor
    {
        private string filter;

        public override void OnInspectorGUI()
        {
            filter = EditorGUILayout.TextField(filter);

            serializedObject.Update();
            var db = target as MiniGameDatabase;
            db.ShownData.Clear();
            foreach (var v in db.table.GetValuesTyped())
            {
                if (filter.IsNullOrEmpty() || v.CodeName.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    db.ShownData.Add(v);
                }
            }
            var list = serializedObject.FindProperty("ShownData");
            EditorGUI.PropertyField(GUILayoutUtility.GetRect(0f, 5000f), list, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
