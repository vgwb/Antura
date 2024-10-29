#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;

namespace Homer
{
    public class FileDownloader : EditorWindow
    {
        private string projectId = "0474c008-e8e9-4e69-a24e-1652618909210";

        private string jsonUrl = "";
        private string csUrl = "";

        private const string downloadDirectory = "Assets/_discover/Homer/ProjectData";

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

            if (GUILayout.Button("Download Files"))
            {
                jsonUrl = "https://homer.open-lab.com/api/project/" + projectId;
                CsUrl = "https://homer.open-lab.com/api/variables/cs/" + projectId;
                DownloadFiles(jsonUrl, CsUrl);
            }
        }

        private void DownloadFiles(string jsonFileUrl, string csFileUrl)
        {
            // Ensure the download directory exists
            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            // Download the JSON file
            DownloadFile(jsonFileUrl, Path.Combine(downloadDirectory, "homer.json"));

            // Download the .cs file
            DownloadFile(csFileUrl, Path.Combine(downloadDirectory, "HomerVars.cs"));

            // Refresh the AssetDatabase to show downloaded files in Unity
            AssetDatabase.Refresh();

            Debug.Log("Files downloaded successfully!");
        }

        private void DownloadFile(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, path);
                    Debug.Log($"Downloaded file from {url} to {path}");
                }
                catch (WebException e)
                {
                    Debug.LogError($"Failed to download file from {url}: {e.Message}");
                }
            }
        }
    }
}
#endif
