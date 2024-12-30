#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Homer
{
    public class FileDownloader : EditorWindow
    {
        private string projectId = "0474c008-e8e9-4e69-a24e-1652618909210";

        private string jsonUrl = "";
        private string csUrl = "";

        private const string downloadDirectory = "_discover/Homer/ProjectData";

        public string CsUrl { get => csUrl; set => csUrl = value; }

        [MenuItem("Tools/Homer/Download Project Data")]
        public static void ShowWindow()
        {
            GetWindow<FileDownloader>("Homer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Download Project Data Files", EditorStyles.boldLabel);

            projectId = EditorGUILayout.TextField("Project ID", projectId);
            jsonUrl = "https://homer.open-lab.com/api/project/" + projectId;
            CsUrl = "https://homer.open-lab.com/api/variables/cs/" + projectId;

            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            if (GUILayout.Button("Open Homer Project"))
            {
                Application.OpenURL("https://homer.open-lab.com/?project_uid=" + projectId);
            }

            GUILayout.Space(10);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Space(10);

            if (GUILayout.Button("Download JSON Data"))
            {
                DownloadAndSaveFile(jsonUrl, Path.Combine(downloadDirectory, "homer.json"), true);
            }

            // if (GUILayout.Button("Download JSON Data Raw"))
            // {
            //     DownloadAndSaveFile(jsonUrl, Path.Combine(downloadDirectory, "homer.json"));
            // }

            if (GUILayout.Button("Download C# Vars"))
            {
                DownloadAndSaveFile(CsUrl, Path.Combine(downloadDirectory, "HomerVars.cs"));
            }
        }

        private void DownloadAndSaveFile(string url, string fileName, bool JSONFormat = false)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string downloadedFile = client.DownloadString(url);

                    if (JSONFormat)
                    {
                        // Format JSON
                        var parsedJson = JToken.Parse(downloadedFile);
                        downloadedFile = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
                    }

                    string path = Path.Combine(Application.dataPath, fileName);
                    File.WriteAllText(path, downloadedFile);

                    AssetDatabase.Refresh(); // Refresh the AssetDatabase to show the new file in the Editor

                    Debug.Log($"File downloaded and saved to {path}");
                }
                catch (WebException ex)
                {
                    Debug.LogError($"Error downloading JSON: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Error saving file: {ex.Message}");
                }
            }
        }

    }
}
#endif
