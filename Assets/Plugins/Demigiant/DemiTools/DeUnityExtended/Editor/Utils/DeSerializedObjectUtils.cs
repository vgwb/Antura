// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2023/12/09

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Demigiant.DemiTools.DeUnityExtended.Editor
{
    public static class DeSerializedObjectUtils
    {
        #region Public Methods

        /// <summary>
        /// Call this method to store a list of <see cref="SerializedProperty"/> objects to draw inside OnInspectorGUI via <see cref="DrawSerializedPropertiesBlock"/>
        /// </summary>
        public static SerializedProperty[] GetAllMarkedAsCustom(SerializedObject so)
        {
            FieldInfo[] customSerialized = so.targetObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(field => field.IsDefined(typeof(CustomPropertyAttribute), false)).ToArray();
            SerializedProperty[] result = new SerializedProperty[customSerialized.Length];
            for (int i = 0; i < customSerialized.Length; i++) result[i] = so.FindProperty(customSerialized[i].Name);
            return result;
        }

        /// <summary>
        /// Draws the given list of <see cref="SerializedProperty"/> objects and wraps them within Update and ApplyModifiedProperties for full serialization and undo
        /// </summary>
        public static void DrawSerializedPropertiesBlock(SerializedObject so, SerializedProperty[] props)
        {
            so.Update();
            foreach (SerializedProperty prop in props) EditorGUILayout.PropertyField(prop);
            so.ApplyModifiedProperties();
        }

        #endregion
    }
}