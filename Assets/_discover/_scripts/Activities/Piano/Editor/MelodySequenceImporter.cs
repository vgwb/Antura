#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Activities
{

    public class MelodySequenceImporter : EditorWindow
    {
        [TextArea(10, 30)]
        public string inputText =
    @"tempoBPM: 100
sequence:
  - { IsRest: 0, Note: 9,  Octave: 3, Duration: 2 }
  - { IsRest: 0, Note: 7,  Octave: 3, Duration: 2 }";

        public int overrideTempoBPM = 100;
        public string assetName = "MelodySequence_Imported";
        public string saveFolder = "Assets";

        private List<MelodyEvent> previewEvents = new List<MelodyEvent>();
        private int parsedCount = 0;
        private string parseStatus = "";

        [MenuItem("Antura/PianoActivity/Import Melody From Text")]
        public static void Open()
        {
            var wnd = GetWindow<MelodySequenceImporter>("Melody Importer");
            wnd.minSize = new Vector2(600, 500);
            wnd.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Paste your melody text (YAML-like)", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                inputText = EditorGUILayout.TextArea(inputText, GUILayout.Height(220));
            }

            EditorGUILayout.Space();
            overrideTempoBPM = EditorGUILayout.IntField(new GUIContent("Tempo BPM (override)"), overrideTempoBPM);
            assetName = EditorGUILayout.TextField(new GUIContent("Asset Name"), assetName);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Save Folder", saveFolder);
                if (GUILayout.Button("Select Folder", GUILayout.Width(120)))
                {
                    var folder = EditorUtility.OpenFolderPanel("Select Folder in Project", Application.dataPath, "");
                    if (!string.IsNullOrEmpty(folder))
                    {
                        // Convert absolute path into project-relative "Assets/.."
                        string projPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
                        if (folder.StartsWith(projPath))
                        {
                            saveFolder = "Assets" + folder.Substring(projPath.Length);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Invalid Folder", "Please select a folder inside this Unity project.", "OK");
                        }
                    }
                }
            }

            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Parse / Preview", GUILayout.Height(28)))
                {
                    ParseInput(inputText);
                }
                EditorGUI.BeginDisabledGroup(parsedCount == 0);
                if (GUILayout.Button("Create Asset", GUILayout.Height(28)))
                {
                    CreateAsset();
                }
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Parse Status:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(parseStatus, MessageType.Info);

            if (parsedCount > 0)
            {
                EditorGUILayout.LabelField($"Preview ({parsedCount} events):");
                int show = Mathf.Min(12, previewEvents.Count);
                for (int i = 0; i < show; i++)
                {
                    var ev = previewEvents[i];
                    EditorGUILayout.LabelField($"#{i + 1}: IsRest={ev.IsRest}, Note={(int)ev.Note} ({ev.Note}), Octave={ev.Octave}, Duration={(int)ev.Duration}");
                }
                if (parsedCount > show)
                    EditorGUILayout.LabelField($"... ({parsedCount - show} more)");
            }
        }

        private void ParseInput(string text)
        {
            previewEvents.Clear();
            parsedCount = 0;
            parseStatus = "";

            if (string.IsNullOrWhiteSpace(text))
            {
                parseStatus = "Input is empty.";
                return;
            }

            // Try to extract tempo from "tempoBPM: N" if present
            var tempoMatch = Regex.Match(text, @"tempoBPM\s*:\s*(\d+)", RegexOptions.IgnoreCase);
            if (tempoMatch.Success && int.TryParse(tempoMatch.Groups[1].Value, out int bpm))
            {
                overrideTempoBPM = bpm;
            }

            // Regex per matchare blocchi tipo { IsRest: 0, Note: 9, Octave: 3, Duration: 2 }
            var itemRegex = new Regex(
                @"IsRest\s*:\s*(?<rest>\d+|true|false)[^{}\n\r]*?" +
                @"Note\s*:\s*(?<note>-?\d+)[^{}\n\r]*?" +
                @"Octave\s*:\s*(?<oct>-?\d+)[^{}\n\r]*?" +
                @"Duration\s*:\s*(?<dur>-?\d+)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            var matches = itemRegex.Matches(text);
            foreach (Match m in matches)
            {
                try
                {
                    // IsRest
                    bool isRest = false;
                    string restStr = m.Groups["rest"].Value.Trim().ToLowerInvariant();
                    if (restStr == "1" || restStr == "true")
                        isRest = true;

                    // Note
                    int noteIdx = Mathf.Clamp(int.Parse(m.Groups["note"].Value), 0, 11);
                    NoteName noteEnum = (NoteName)noteIdx;

                    // Octave
                    int octave = int.Parse(m.Groups["oct"].Value);

                    // Duration
                    int durVal = int.Parse(m.Groups["dur"].Value);
                    NoteDuration durEnum = (NoteDuration)durVal; // Assunto: enum usa gli stessi interi (1=Eighth, 2=Quarter, ecc.)

                    var ev = new MelodyEvent
                    {
                        IsRest = isRest,
                        Note = noteEnum,
                        Octave = octave,
                        Duration = durEnum
                    };
                    previewEvents.Add(ev);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Parse error on item: {ex.Message}");
                }
            }

            parsedCount = previewEvents.Count;
            parseStatus = parsedCount > 0
                ? $"Parsed {parsedCount} events. Tempo = {overrideTempoBPM} BPM."
                : "No events matched. Check your formatting: lines like { IsRest: 0, Note: 9, Octave: 3, Duration: 2 }";
        }

        private void CreateAsset()
        {
            if (parsedCount == 0)
            {
                EditorUtility.DisplayDialog("No data", "Parse some events before creating the asset.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(saveFolder) || !saveFolder.StartsWith("Assets"))
            {
                EditorUtility.DisplayDialog("Invalid folder", "Please pick a valid folder inside Assets/.", "OK");
                return;
            }

            var so = ScriptableObject.CreateInstance<PianoSettingsData>();
            so.tempoBPM = Mathf.Max(1, overrideTempoBPM);
            so.sequence = new List<MelodyEvent>(previewEvents);

            string safeName = string.IsNullOrWhiteSpace(assetName) ? "MelodySequence_Imported" : assetName.Trim();
            string path = AssetDatabase.GenerateUniqueAssetPath($"{saveFolder}/{safeName}.asset");
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorGUIUtility.PingObject(so);
            parseStatus = $"Created asset at: {path}";
            Debug.Log($"[MelodySequenceImporter] Created asset at: {path}");
        }
    }
}
#endif
