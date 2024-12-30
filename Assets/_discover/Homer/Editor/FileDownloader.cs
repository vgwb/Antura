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

            if (GUILayout.Button("Download JSON"))
            {
                DownloadFiles(true, jsonUrl, CsUrl);
            }

            if (GUILayout.Button("Download Vars"))
            {
                DownloadFiles(false, jsonUrl, CsUrl);
            }
        }

        private void DownloadFiles(bool doJson, string jsonFileUrl, string csFileUrl)
        {
            // Ensure the download directory exists
            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            if (doJson)
            {
                // Download the JSON file
                DownloadAndSaveJson(jsonFileUrl, Path.Combine(downloadDirectory, "homer.json"));
            }
            else
            {
                // Download the .cs file
                DownloadAndSaveCs(csFileUrl, Path.Combine(downloadDirectory, "HomerVars.cs"));
            }

            Debug.Log("Files downloaded successfully!");
        }

        private void DownloadAndSaveJson(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string json = client.DownloadString(url);

                    // Format JSON
                    var parsedJson = JToken.Parse(json);
                    var formattedJson = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);

                    string path = Path.Combine(Application.dataPath, fileName);
                    File.WriteAllText(path, formattedJson);

                    AssetDatabase.Refresh(); // Refresh the AssetDatabase to show the new file in the Editor

                    Debug.Log($"JSON file downloaded and saved to {path}");
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

        private void DownloadAndSaveCs(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string csfile = client.DownloadString(url);

                    string path = Path.Combine(Application.dataPath, fileName);
                    File.WriteAllText(path, csfile);

                    AssetDatabase.Refresh(); // Refresh the AssetDatabase to show the new file in the Editor

                    Debug.Log($"CS file downloaded and saved to {path}");
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
