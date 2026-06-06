using UnityEditor;
using UnityEngine;

namespace Antura.Discover.DEV110.EditorTools
{
    /// <summary>
    /// Adds an "Open in Pattern Designer" shortcut to the top of a <see cref="ProjectilePattern"/>
    /// asset's inspector, then falls back to the normal field list for raw editing.
    /// </summary>
    [CustomEditor(typeof(ProjectilePattern))]
    public class ProjectilePatternInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("✎  Open in Pattern Designer", GUILayout.Height(28)))
                ProjectilePatternDesigner.Open((ProjectilePattern)target);

            EditorGUILayout.Space(6);
            DrawDefaultInspector();
        }
    }
}
