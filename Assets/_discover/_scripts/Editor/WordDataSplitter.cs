using System.IO;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover.Editor
{
    public class WordDataSplitter : EditorWindow
    {
        private WordsListData sourceWordsLibrary;
        private string outputFolderPath = "Assets/WordData";

        [MenuItem("Antura/Discover/Utility/Word Data Splitter")]
        public static void ShowWindow()
        {
            GetWindow<WordDataSplitter>("Word Data Splitter");
        }

        private void OnGUI()
        {
            GUILayout.Label("Word Data Splitter", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // Source WordsLibrary field
            sourceWordsLibrary = (WordsListData)EditorGUILayout.ObjectField(
                "Source Words Library",
                sourceWordsLibrary,
                typeof(WordsListData),
                false
            );

            GUILayout.Space(10);

            // Output folder path
            GUILayout.Label("Output Folder:");
            EditorGUILayout.BeginHorizontal();
            outputFolderPath = EditorGUILayout.TextField(outputFolderPath);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Output Folder", "Assets", "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    // Convert absolute path to relative path
                    if (selectedPath.StartsWith(Application.dataPath))
                    {
                        outputFolderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            // Split button
            GUI.enabled = sourceWordsLibrary != null;
            if (GUILayout.Button("Split Into Individual WordData Files", GUILayout.Height(30)))
            {
                SplitWordData();
            }
            GUI.enabled = true;

            GUILayout.Space(10);

            // Info
            if (sourceWordsLibrary != null)
            {
                GUILayout.Label($"Words to split: {sourceWordsLibrary.Words.Count}", EditorStyles.helpBox);
            }
        }

        private void SplitWordData()
        {
            if (sourceWordsLibrary == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a source WordsLibrary.", "OK");
                return;
            }

            // Create output directory if it doesn't exist
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            int successCount = 0;
            int errorCount = 0;

            foreach (var wordData in sourceWordsLibrary.Words)
            {
                try
                {
                    // Create new ScriptableObject instance
                    WordData newWordData = ScriptableObject.CreateInstance<WordData>();

                    // Copy all fields
                    newWordData.Id = wordData.Id;
                    newWordData.Active = wordData.Active;
                    newWordData.TextEn = wordData.TextEn;
                    newWordData.Kind = wordData.Kind;
                    newWordData.Category = wordData.Category;
                    newWordData.Form = wordData.Form;
                    newWordData.Value = wordData.Value;
                    newWordData.SortValue = wordData.SortValue;
                    newWordData.DrawingUnicode = wordData.DrawingUnicode;
                    newWordData.DrawingValue = wordData.DrawingValue;
                    newWordData.DrawingAtlas = wordData.DrawingAtlas;

                    // Create filename (sanitize ID for filesystem)
                    string fileName = SanitizeFileName(wordData.Id);
                    string filePath = Path.Combine(outputFolderPath, $"{fileName}.asset");

                    // Create the asset
                    AssetDatabase.CreateAsset(newWordData, filePath);
                    successCount++;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to create WordData for ID '{wordData.Id}': {e.Message}");
                    errorCount++;
                }
            }

            // Refresh the AssetDatabase to show the new files
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Show completion dialog
            string message = $"Split complete!\n\nSuccessfully created: {successCount} files";
            if (errorCount > 0)
            {
                message += $"\nErrors: {errorCount} files";
            }
            message += $"\n\nFiles saved to: {outputFolderPath}";

            EditorUtility.DisplayDialog("Split Complete", message, "OK");
        }

        private string SanitizeFileName(string fileName)
        {
            // Replace invalid characters with underscores
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '_');
            }

            // Also replace some additional characters that might cause issues
            fileName = fileName.Replace(' ', '_');
            fileName = fileName.Replace('-', '_');

            return fileName;
        }
    }
}
