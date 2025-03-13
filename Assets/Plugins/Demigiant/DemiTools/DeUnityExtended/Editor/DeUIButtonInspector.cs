// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/10/19

using DG.DemiEditor;
using DG.DemiLib;
using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Demigiant.DemiTools.DeUnityExtended.Editor
{
    [CustomEditor(typeof(DeUIButton), true), CanEditMultipleObjects]
    public class DeUIButtonInspector : ButtonEditor
    {
        readonly GUIContent _gcMultipleDiffValues = new GUIContent("-");
        readonly GUIContent _gcKeepSelected = new GUIContent("Keep Selected", "If selected keeps the button selected even after it becomes non-interactable");
        readonly GUIContent _gcSelectOtherOnDisable = new GUIContent("On Selected Disabled", "If set and this button is disabled, tries to select another selectable in the given direction");
        readonly GUIContent _gcShowOnSelected = new GUIContent("Show OnSelect", "Eventual GameObject to activate when the button is selected, deactivate otherwise");
        readonly GUIContent _gcIsToggle = new GUIContent("Is Toggle", "If TRUE this button will behave like a toggle with an on/off state");
        readonly GUIContent _gcIsOn = new GUIContent("ON", "Activate this toggle on startup");
        readonly GUIContent _gcSecondaryTargetGraphic = new GUIContent("Extra Target Graphic",
            "Eventual secondary Graphic object whose color to control along the main Target Graphic");
        readonly GUIContent _gcHoverTargetGraphic = new GUIContent("Hover Target Graphic",
            "Eventual Graphic object whose color to change on mouse hover or select (can't be the same as Extra Target Graphic)");
        readonly GUIContent _gcHoverTargetColor = new GUIContent("└ Hover Target Color",
            "Eventual color to assign on hover/select to Hover Target Graphic");
        readonly GUIContent _gcBorderOnlyWhenOff = new GUIContent("OFF = Border Only",
            "If selected will only show the outline of the button image when it's toggled off (image must be set as sliced and have a border)");
        readonly GUIContent _gcToggleGroup = new GUIContent("Group",
            "Eventual group this toggle is part of (all toggles in the same group will be toggled off when another is toggled on)");
        readonly GUIContent _gcToggleMode = new GUIContent("",
            "- Manual ON OFF:  Can always be manually toggled both ON and OFF\n- No Manual OFF:  Can only be manually toggled ON\n- Manual OFF If No Group: Can be manually toggled OFF only if not in a group");
        readonly GUIContent _gcToggleColors = new GUIContent("Toggled Colors", "Colors activated when this toggle is on");
        readonly GUIContent _gcToggle_secondaryTarget = new GUIContent("ON Secondary Target", "Used as eventual target for extra options");
        readonly GUIContent _gcToggle_activateIfToggled = new GUIContent("└ ON Conditional Activation",
            "If selected deactivates the chosen secondary target when the toggle is OFF, activates it when ON");
        readonly GUIContent _gcToggle_changeColor = new GUIContent("└ ON Change Color", "If selected changes the color of the chosen secondary target when the toggle is ON");
        readonly GUIContent _gcToggle_sprite = new GUIContent("└ ON Sprite", "Used as eventual replacement for the chosen secondary target when the toggle is ON");
        readonly GUIContent _gcToggle_text = new GUIContent("└ ON Text", "Used as eventual replacement for for the chosen secondary target when the toggle is ON");

        SerializedProperty _p_interactable;
        SerializedProperty _p_baseInteractable;
        SerializedProperty _p_keepSelected;
        SerializedProperty _p_selectOtherOnDisable;
        SerializedProperty _p_showOnSelected;
        SerializedProperty _p_isToggle;
        SerializedProperty _p_isOn;
        SerializedProperty _p_borderOnlyWhenOff;
        SerializedProperty _p_secondaryTargetGraphic;
        SerializedProperty _p_hoverTargetGraphic;
        SerializedProperty _p_hoverTargetColor;
        SerializedProperty _p_isToggleGroup;
        SerializedProperty _p_groupId;
        SerializedProperty _p_toggleMode;
        SerializedProperty _p_toggle_secondaryTarget;
        SerializedProperty _p_toggle_activateIfToggled;
        SerializedProperty _p_toggle_changeColor;
        SerializedProperty _p_toggle_color;
        SerializedProperty _p_toggle_sprite;
        SerializedProperty _p_toggle_text;
        SerializedProperty _p_toggledColors;
        SerializedProperty _p_onToggleOn;

        #region Unity and GUI Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _p_interactable = serializedObject.FindProperty("_isInteractable");
            _p_baseInteractable = serializedObject.FindProperty("m_Interactable");
            _p_keepSelected = serializedObject.FindProperty("_keepSelected");
            _p_selectOtherOnDisable = serializedObject.FindProperty("_selectOtherOnDisable");
            _p_showOnSelected = serializedObject.FindProperty("_showOnSelected");
            _p_isToggle = serializedObject.FindProperty("_isToggle");
            _p_isOn = serializedObject.FindProperty("_isOn");
            _p_secondaryTargetGraphic = serializedObject.FindProperty("_secondaryTargetGraphic");
            _p_hoverTargetGraphic = serializedObject.FindProperty("_hoverTargetGraphic");
            _p_hoverTargetColor = serializedObject.FindProperty("_hoverTargetColor");
            _p_borderOnlyWhenOff = serializedObject.FindProperty("_borderOnlyWhenOff");
            _p_isToggleGroup = serializedObject.FindProperty("_isToggleGroup");
            _p_groupId = serializedObject.FindProperty("_groupId");
            _p_toggleMode = serializedObject.FindProperty("_toggleMode");
            _p_toggle_secondaryTarget = serializedObject.FindProperty("_toggle_secondaryTarget");
            _p_toggle_activateIfToggled = serializedObject.FindProperty("_toggle_activateIfToggled");
            _p_toggle_changeColor = serializedObject.FindProperty("_toggle_changeColor");
            _p_toggle_color = serializedObject.FindProperty("_toggle_color");
            _p_toggle_sprite = serializedObject.FindProperty("_toggle_sprite");
            _p_toggle_text = serializedObject.FindProperty("_toggle_text");
            _p_toggledColors = serializedObject.FindProperty("toggledColors");
            _p_onToggleOn = serializedObject.FindProperty("onToggleOn");
        }

        public override void OnInspectorGUI()
        {
            DeGUI.BeginGUI();
            serializedObject.Update();

            using (new DeGUI.LabelFieldWidthScope(EditorGUIUtility.labelWidth - 6))
            using (new DeGUI.ColorScope(null, null, new DeSkinColor(0.7f, 0.3f)))
            using (new GUILayout.VerticalScope(DeGUI.styles.box.roundOutline02)) {
                DeGUI.ResetGUIColors();
                using (new GUILayout.HorizontalScope()) {
                    ToggleButton(_p_isToggle, _gcIsToggle, GUILayout.Width(64));
                }
                if (_p_isToggle.boolValue) {
                    // Is Toggle
                    // GUILayout.EndHorizontal();
                    using (new GUILayout.HorizontalScope()) {
                        EditorGUILayout.PropertyField(_p_toggleMode, _gcToggleMode, GUILayout.Width(150));
                        ToggleButton(_p_isOn, _gcIsOn, GUILayout.Width(30));
                        ToggleButton(_p_borderOnlyWhenOff, _gcBorderOnlyWhenOff, GUILayout.Width(114));
                    }
                    using (new GUILayout.HorizontalScope()) {
                        ToggleButton(_p_isToggleGroup, _gcToggleGroup, GUILayout.Width(64));
                        if (_p_isToggleGroup.boolValue) {
                            using (new DeGUI.ColorScope(null, null, DeGUI.colors.bg.toggleOn)) {
                                EditorGUILayout.PropertyField(_p_groupId, GUIContent.none);
                            }
                        }
                    }
                    using (new DeGUI.LabelFieldWidthScope(EditorGUIUtility.labelWidth - 6))
                    using (new GUILayout.VerticalScope(DeGUI.styles.box.def)) {
                        EditorGUILayout.PropertyField(_p_toggle_secondaryTarget, _gcToggle_secondaryTarget);
                        if (_p_toggle_secondaryTarget.objectReferenceValue != null) {
                            EditorGUILayout.PropertyField(_p_toggle_activateIfToggled, _gcToggle_activateIfToggled);
                            using (new GUILayout.HorizontalScope()) {
                                EditorGUILayout.PropertyField(_p_toggle_changeColor, _gcToggle_changeColor);
                                if (_p_toggle_changeColor.boolValue) EditorGUILayout.PropertyField(_p_toggle_color, GUIContent.none);
                            }
                            if (_p_toggle_secondaryTarget.objectReferenceValue is Image) EditorGUILayout.PropertyField(_p_toggle_sprite, _gcToggle_sprite);
                            if (_p_toggle_secondaryTarget.objectReferenceValue is TMP_Text) EditorGUILayout.PropertyField(_p_toggle_text, _gcToggle_text);
                        }
                        GUILayout.Label(_gcToggleColors);
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(_p_toggledColors);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(_p_onToggleOn);
                }
            }
            GUILayout.Space(-2);
            using (new DeGUI.ColorScope(null, null, new DeSkinColor(0.7f, 0.3f)))
            using (new GUILayout.VerticalScope(DeGUI.styles.box.roundOutline02)) {
                DeGUI.ResetGUIColors();
                using (var check = new EditorGUI.ChangeCheckScope()) {
                    EditorGUILayout.PropertyField(_p_secondaryTargetGraphic, _gcSecondaryTargetGraphic);
                    if (check.changed) {
                        bool invalid = _p_secondaryTargetGraphic != null && _p_secondaryTargetGraphic.objectReferenceValue == _p_hoverTargetGraphic.objectReferenceValue;
                        if (invalid) _p_secondaryTargetGraphic.objectReferenceValue = null;
                    }
                }
                using (var check = new EditorGUI.ChangeCheckScope()) {
                    EditorGUILayout.PropertyField(_p_hoverTargetGraphic, _gcHoverTargetGraphic);
                    if (check.changed) {
                        bool invalid = _p_hoverTargetGraphic != null && _p_hoverTargetGraphic.objectReferenceValue == _p_secondaryTargetGraphic.objectReferenceValue;
                        if (invalid) _p_hoverTargetGraphic.objectReferenceValue = null;
                    }
                }
                if (_p_hoverTargetGraphic.objectReferenceValue != null) {
                    EditorGUILayout.PropertyField(_p_hoverTargetColor, _gcHoverTargetColor);
                }
                EditorGUILayout.PropertyField(_p_keepSelected, _gcKeepSelected);
                EditorGUILayout.PropertyField(_p_selectOtherOnDisable, _gcSelectOtherOnDisable);
                EditorGUILayout.PropertyField(_p_showOnSelected, _gcShowOnSelected);
            }

            serializedObject.ApplyModifiedProperties();

            using (var check = new EditorGUI.ChangeCheckScope()) {
                base.OnInspectorGUI();
                if (check.changed) {
                    _p_interactable.boolValue = _p_baseInteractable.boolValue;
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        void ToggleButton(SerializedProperty p, GUIContent guiContent, params GUILayoutOption[] options)
        {
            using (var check = new EditorGUI.ChangeCheckScope()) {
                bool res = DeGUILayout.ToggleButton(p.boolValue,
                    p.hasMultipleDifferentValues ? _gcMultipleDiffValues : guiContent, DeGUI.styles.button.bBlankBorderCompact, options
                );
                if (check.changed) p.boolValue = res;
            }
        }

        #endregion
    }
}