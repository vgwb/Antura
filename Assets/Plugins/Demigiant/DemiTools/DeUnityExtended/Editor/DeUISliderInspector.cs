// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2022/04/12

using DG.DemiEditor;
using DG.DemiLib;
using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Demigiant.DemiTools.DeUnityExtended.Editor
{
    [CustomEditor(typeof(DeUISlider), true), CanEditMultipleObjects]
    public class DeUISliderInspector : SliderEditor
    {
        readonly GUIContent _gcSelectOtherOnDisable = new GUIContent("On Selected Disabled", "If set and this button is disabled, tries to select another selectable in the given direction");
        readonly GUIContent _gcShowOnSelected = new GUIContent("Show OnSelect", "Eventual GameObject to activate when the button is selected, deactivate otherwise");
        
        SerializedProperty _p_selectOtherOnDisable;
        SerializedProperty _p_showOnSelected;
        
        #region Unity and GUI Methods
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _p_selectOtherOnDisable = serializedObject.FindProperty("_selectOtherOnDisable");
            _p_showOnSelected = serializedObject.FindProperty("_showOnSelected");
        }
        
        public override void OnInspectorGUI()
        {
            DeGUI.BeginGUI();
            serializedObject.Update();

            using (new DeGUI.ColorScope(null, null, new DeSkinColor(0.7f, 0.3f)))
            using (new GUILayout.VerticalScope(DeGUI.styles.box.roundOutline02)) {
                DeGUI.ResetGUIColors();
                EditorGUILayout.PropertyField(_p_selectOtherOnDisable, _gcSelectOtherOnDisable);
                EditorGUILayout.PropertyField(_p_showOnSelected, _gcShowOnSelected);
            }

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
        
        #endregion
    }
}