using Antura.UI;
using Demigiant.DemiTools.DeUnityExtended.Editor;
using DG.DemiEditor;
using DG.DemiLib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Editor
{
    [CustomEditor(typeof(ButtonExtended), true), CanEditMultipleObjects]
    public class ButtonExtendedInspector : DeUIButtonInspector
    {
        readonly GUIContent gc_type = new GUIContent("Type");
        readonly GUIContent gc_url = new GUIContent("URL");

        SerializedProperty p_type, p_url;
        SerializedProperty p_isToggle, p_colors, p_toggledColors;

        const string assignColorsDialogTitle = "Button type changed";
        const string assignColorsDialogMsg = "Do you wan to assign the default colors for this type or to keep the current ones?";
        const string assignColorsDialogOk = "Assign Default";
        const string assignColorsDialogCancel = "Keep Current";
        ColorBlock defToggleColors = ColorBlock.defaultColorBlock;
        ColorBlock defToggledColors = ColorBlock.defaultColorBlock;
        ColorBlock defColors = ColorBlock.defaultColorBlock;

        #region Unity + GUI

        protected override void OnEnable()
        {
            base.OnEnable();
            
            FillColorBlockWith(ref defToggleColors, new Color(0.4f, 0.4f, 0.4f), 0.5f);
            FillColorBlockWith(ref defToggledColors, new Color(0f, 0.84f, 0.23f), 0.5f);
            FillColorBlockWith(ref defColors, new Color(0.28f, 0.61f, 0.71f), 0.5f);

            p_type = serializedObject.FindProperty("type");
            p_url = serializedObject.FindProperty("url");
            
            p_isToggle = serializedObject.FindProperty("_isToggle");
            p_colors = serializedObject.FindProperty("m_Colors");
            p_toggledColors = serializedObject.FindProperty("toggledColors");
        }

        public override void OnInspectorGUI()
        {
            DeGUI.BeginGUI();

            serializedObject.Update();

            // using (new DeGUI.ColorScope(null, null, new DeSkinColor(new Color(0.32f, 0.56f, 1f))))
            using (new DeGUI.ColorScope(new DeSkinColor(new Color(0.16f, 0.42f, 1f, 0.35f))))
            // using (new GUILayout.VerticalScope(DeGUI.styles.box.roundOutline02))
            using (new GUILayout.VerticalScope(DeGUI.styles.box.flat))
            {
                // DeGUI.ResetGUIColors();
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    ButtonExtended.ButtonType prevType = (ButtonExtended.ButtonType)p_type.enumValueIndex;
                    EditorGUILayout.PropertyField(p_type, gc_type);
                    if (check.changed)
                    {
                        switch ((ButtonExtended.ButtonType)p_type.enumValueIndex)
                        {
                            case ButtonExtended.ButtonType.Toggle:
                                p_isToggle.boolValue = true;
                                if (EditorUtility.DisplayDialog(assignColorsDialogTitle, assignColorsDialogMsg, assignColorsDialogOk, assignColorsDialogCancel))
                                {
                                    AssignColorBlock(p_colors, defToggleColors);
                                    AssignColorBlock(p_toggledColors, defToggledColors);
                                }
                                break;
                            case ButtonExtended.ButtonType.Normal:
                            case ButtonExtended.ButtonType.OpenURL:
                                p_isToggle.boolValue = false;
                                if (prevType == ButtonExtended.ButtonType.Toggle)
                                {
                                    if (EditorUtility.DisplayDialog(assignColorsDialogTitle, assignColorsDialogMsg, assignColorsDialogOk, assignColorsDialogCancel))
                                    {
                                        AssignColorBlock(p_colors, defColors);
                                    }
                                }
                                break;
                        }
                    }
                }

                if (p_type.enumValueIndex == (int)ButtonExtended.ButtonType.OpenURL)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("You can either write the URL normally as \"http://...\" OR pass an AppConfig variable by writing it as \"AppConfig.urlVariable\" (example: \"AppConfig.UrlWebsite\")", MessageType.Info);
                    using (new DeGUI.ColorScope(!string.IsNullOrEmpty(p_url.stringValue) ? Color.white : Color.red))
                    {
                        EditorGUILayout.PropertyField(p_url, gc_url);
                    }
                    EditorGUI.indentLevel--;
                }
            }

            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }

        #endregion

        #region Methods

        void FillColorBlockWith(ref ColorBlock colors, Color baseColor, float disabledAlpha)
        {
            colors.normalColor = colors.selectedColor = baseColor;
            colors.highlightedColor = colors.pressedColor = colors.normalColor.CloneAndChangeBrightness(1.2f);
            colors.disabledColor = colors.normalColor.SetAlpha(disabledAlpha);
        }

        void AssignColorBlock(SerializedProperty property, ColorBlock colors)
        {
            property.FindPropertyRelative("m_NormalColor").colorValue = colors.normalColor;
            property.FindPropertyRelative("m_HighlightedColor").colorValue = colors.highlightedColor;
            property.FindPropertyRelative("m_PressedColor").colorValue = colors.pressedColor;
            property.FindPropertyRelative("m_SelectedColor").colorValue = colors.selectedColor;
            property.FindPropertyRelative("m_DisabledColor").colorValue = colors.disabledColor;
        }

        #endregion
    }
}