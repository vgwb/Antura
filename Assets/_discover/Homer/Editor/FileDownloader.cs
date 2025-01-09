#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Homer
{
    public class FileDownloader : EditorWindow
    {
        private string projectId = "0474c008-e8e9-4e69-a24e-1652618909210";

        private string jsonUrl = "";
        private string csUrl = "";

        private const string downloadDirectory = "Assets/_discover/Homer/projectData";

        public string CsUrl { get => csUrl; set => csUrl = value; }

        [MenuItem("Tools/Homer/Homer Project")]
        public static void ShowWindow()
        {
            GetWindow<FileDownloader>("Homer Project");
        }

        private GUIStyle paddedStyle;

        private void OnGUI()
        {
            paddedStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(30, 30, 30, 30) // Left, Right, Top, Bottom
            };

            GUILayout.BeginVertical(paddedStyle);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Open Homer Project"))
            {
                Application.OpenURL("https://homer.open-lab.com/?project_uid=" + projectId);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Label("Download Files", EditorStyles.boldLabel);
            GUILayout.Space(10);

            projectId = EditorGUILayout.TextField("Project ID", projectId);
            GUILayout.Space(20);

            EditorGUILayout.HelpBox("Ensure that the Homer project is set to Public in the Share Options panel.", MessageType.Info);
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Download Files", GUILayout.Width(200)))
            {
                jsonUrl = "https://homer.open-lab.com/api/project/" + projectId;
                CsUrl = "https://homer.open-lab.com/api/variables/cs/" + projectId;
                DownloadFiles(jsonUrl, CsUrl);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DownloadFiles(string jsonFileUrl, string csFileUrl)
        {
            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            // Download the JSON file with validation
            DownloadFileWithValidation(jsonFileUrl, Path.Combine(downloadDirectory, "homer.json"));

            // Download the .cs file
            DownloadFile(csFileUrl, Path.Combine(downloadDirectory, "HomerVars.cs"));

            // Refresh the AssetDatabase to show downloaded files in Unity
            AssetDatabase.Refresh();

            Debug.Log("Files downloaded successfully!");
        }

        private void DownloadFile(string url, string path)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

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

        private void DownloadFileWithValidation(string url, string targetPath)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            using (WebClient client = new WebClient())
            {
                try
                {
                    // Step 1: Download to a temporary file
                    client.DownloadFile(url, tempFilePath);
                    Debug.Log($"Downloaded file from {url} to temporary path {tempFilePath}");

                    // Step 2: Validate the JSON content
                    if (IsJsonValid(tempFilePath))
                    {
                        // Step 3: Check if the target file already exists and delete it if necessary
                        if (File.Exists(targetPath))
                        {
                            File.Delete(targetPath);
                            Debug.Log($"Existing file {targetPath} deleted.");
                        }

                        // Step 3: Clean up the JSON content
                        string cleanedJson = CleanJsonContent(tempFilePath);

                        // Step 4: Write cleaned JSON to the target path
                        File.WriteAllText(targetPath, cleanedJson);
                        Debug.Log($"Cleaned JSON file saved to {targetPath}");

                        // Move the temporary file to the target path, overwriting if exists
                        //File.Move(tempFilePath, targetPath);
                        //Debug.Log($"Moved valid file to {targetPath}");
                    }
                    else
                    {
                        // If validation fails, log the "text" property
                        Debug.LogError("Downloaded JSON file is not valid. Aborting overwrite.");
                    }
                }
                catch (WebException e)
                {
                    Debug.LogError($"Failed to download file from {url}: {e.Message}");
                }
                finally
                {
                    // Cleanup the temporary file if it exists
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
            }
        }


        private bool IsJsonValid(string filePath)
        {
            try
            {
                // Read the JSON content from the file
                var jsonContent = File.ReadAllText(filePath);

                // Parse the JSON content into a JObject (this will allow you to manipulate and validate the JSON structure)
                JObject jsonObject = JObject.Parse(jsonContent);

                // Check for the "success" property and the "_id" property
                bool hasSuccessProperty = jsonObject.ContainsKey("success");
                bool hasIdProperty = jsonObject.ContainsKey("_id");

                // The JSON is valid if "success" is not present and "_id" is present
                return !hasSuccessProperty && hasIdProperty;
            }
            catch (WebException ex)
            {
                Debug.LogError($"JSON validation failed: {ex.Message}");
            }
            return false; // Invalid JSON if the conditions are not met
        }

        private string CleanJsonContent(string filePath)
        {
            try
            {
                // Read the JSON content from the file
                var jsonContent = File.ReadAllText(filePath);

                // Parse the JSON content into a JObject (this will allow you to manipulate the JSON structure)
                JObject jsonObject = JObject.Parse(jsonContent);

                jsonObject.Remove("_info");
                jsonObject.Remove("_selectedFlowId");
                jsonObject.Remove("_modified");

                // Process the "_flows" array
                JArray flowsArray = (JArray)jsonObject["_flows"];
                foreach (var flow in flowsArray)
                {
                    // Ensure flow is a JObject, then remove "_x" and "_y" properties
                    if (flow is JObject flowObject)
                    {
                        flowObject.Remove("_modified");
                        flowObject.Remove("_x");
                        flowObject.Remove("_y");

                        // Process "_nodes" array within each flow
                        JArray nodesArray = (JArray)flowObject["_nodes"];
                        foreach (var node in nodesArray)
                        {
                            // Ensure node is a JObject, then remove "_x" and "_y" properties
                            if (node is JObject nodeObject)
                            {
                                flowObject.Remove("_modified");
                                nodeObject.Remove("_x");
                                nodeObject.Remove("_y");
                            }
                        }
                    }
                }

                // Serialize the cleaned-up JSON back to a string with indentation
                string cleanedJson = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);
                return cleanedJson;
            }
            catch (WebException ex)
            {
                Debug.LogError($"Failed to clean JSON content: {ex.Message}");
                return File.ReadAllText(filePath); // If cleaning fails, return the original content
            }
        }
    }
}
#endif
