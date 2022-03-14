// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2016/08/01 12:33
// License Copyright (c) Daniele Giardini

using Antura.UI;
using DG.DemiEditor;
using DG.DemiLib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Editor.Inspectors
{
    [CustomEditor(typeof(ContinueScreen))]
    public class ContinueScreenInspector : UnityEditor.Editor
    {
        ContinueScreen src;

        void OnEnable()
        {
            src = target as ContinueScreen;
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(src, "ContinueScreen");
            DeGUI.BeginGUI();

            GUILayout.Label("Settings", DeGUI.styles.label.bold);
            DeGUILayout.Toolbar("Center Position", new DeSkinColor(0.5f, 0.1f));
            using (new DeGUILayout.ToolbarScope(new DeSkinColor(0.5f, 0.1f), DeGUI.styles.toolbar.def))
            {
                if (GUILayout.Button("Save", DeGUI.styles.button.tool))
                {
                    src.CenterSnapshot = GetSnapshot();
                    EditorUtility.SetDirty(src);
                }
                if (GUILayout.Button("Apply", DeGUI.styles.button.tool))
                    SetSnapshot(src.CenterSnapshot);
            }
            GUILayout.Space(4);
            DeGUILayout.Toolbar("Side Position", new DeSkinColor(0.5f, 0.1f));
            using (new DeGUILayout.ToolbarScope(new DeSkinColor(0.5f, 0.1f), DeGUI.styles.toolbar.def))
            {
                if (GUILayout.Button("Save", DeGUI.styles.button.tool))
                {
                    src.SideSnapshot = GetSnapshot();
                    EditorUtility.SetDirty(src);
                }
                if (GUILayout.Button("Apply", DeGUI.styles.button.tool))
                    SetSnapshot(src.SideSnapshot);
            }

            GUILayout.Label("References", DeGUI.styles.label.bold);
            src.Bg = EditorGUILayout.ObjectField("Bg", src.Bg, typeof(Button), true) as Button;
            src.BtContinue = EditorGUILayout.ObjectField("BT Continue", src.BtContinue, typeof(Button), true) as Button;
            src.BtRetry = EditorGUILayout.ObjectField("BT Retry", src.BtRetry, typeof(Button), true) as Button;
            src.IcoContinue = EditorGUILayout.ObjectField("Ico Continue", src.IcoContinue, typeof(RectTransform), true) as RectTransform;
        }

        ButtonSnapshot GetSnapshot()
        {
            if (src.BtContinue == null)
            {
                EditorUtility.DisplayDialog("Apply", "BT Continue is not set", "Ok");
                return new ButtonSnapshot();
            }

            RectTransform rt = src.BtContinue.GetComponent<RectTransform>();
            return new ButtonSnapshot(
                rt.anchoredPosition,
                rt.anchorMin,
                rt.anchorMax,
                rt.sizeDelta,
                src.IcoContinue.anchoredPosition
            );
        }

        void SetSnapshot(ButtonSnapshot snapshot)
        {
            if (src.BtContinue == null || src.IcoContinue == null)
                return;

            snapshot.Apply(src.BtContinue.GetComponent<RectTransform>(), src.IcoContinue);
        }
    }
}
