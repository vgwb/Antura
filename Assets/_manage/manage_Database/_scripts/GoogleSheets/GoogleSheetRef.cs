using Antura.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.DeInspektor.Attributes;
using UnityEngine.SceneManagement;
using Antura.Database.Management;
#if UNITY_EDITOR
using System.IO;
using Unity.EditorCoroutines.Editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

namespace Antura.GoogleSheets
{
    [CreateAssetMenu(menuName = "Antura/Google Sheet Ref", order = 1)]
    public class GoogleSheetRef : ScriptableObject
    {
        public string GoogleDocID;
        [Tooltip("comma separated sheets to import. leave empty for *")]
        public string Sheets2Import;
        public string FileName;

        public ContentType ContentType
        {
            get
            {
                if (FileName.Equals("App_Common") || FileName.Equals("Common_Translations"))
                    return ContentType.Localization;
                if (FileName.Contains("PlaySession"))
                    return ContentType.Journey;
                return ContentType.Vocabulary;
            }
        }

#if UNITY_EDITOR
        [DeMethodButton("Open Sheet in Web Browser")]
        public void OpenSheetInBrowser()
        {
            Application.OpenURL("https://docs.google.com/spreadsheets/d/" + GoogleDocID + "/edit");
        }

        private readonly string GoogleApiUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
        private readonly string GoogleApiUrlPostfix = "?includeGridData=true";
        private readonly string GoogleSecretApi = "&key=AIzaSyDZhLs3ds7swYw9HizLPz5M-0F584QMRcs";
        private readonly string PathToJson = "../_config/json_data/antura_json_data_files/";

        public void ImportJSON()
        {
            EditorCoroutineUtility.StartCoroutine(fetchData(this), this);
        }

        private IEnumerator fetchData(GoogleSheetRef sheet)
        {
            var uri = GoogleApiUrl + sheet.GoogleDocID + GoogleApiUrlPostfix + GoogleSecretApi;
            using (var webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.SetRequestHeader("accept", "application/json");
                Debug.Log("get " + uri);
                yield return webRequest.SendWebRequest();

                if ((webRequest.result == UnityWebRequest.Result.ConnectionError) || (webRequest.result == UnityWebRequest.Result.ProtocolError))
                {
                    Debug.Log("Error: " + webRequest.error);
                }
                else
                {
                    //                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    manageReceivedData(sheet, webRequest.downloadHandler.text);
                }
            }
        }

        private void manageReceivedData(GoogleSheetRef sheet, string value)
        {
            // Debug.Log("manageReceivedData per " + sheet.FileName);
            decodeJSON(sheet.FileName, value);
        }

        private void decodeJSON(string fileName, string jsonText)
        {
            //Debug.Log(jsonDemo.text);
            var definition = new GSheet_Spreadsheet();
            var obj = JsonConvert.DeserializeAnonymousType(jsonText, definition);

            Debug.Log("Spreadsheet: " + obj.properties.title);
            GSheet_RowData row;

            foreach (var sheet in obj.sheets)
            {
                if (Sheets2Import == "" || (Sheets2Import != "" && Sheets2Import.Contains(sheet.properties.title)))
                {
                    //                    Debug.Log("current sheet: " + sheet.properties.title);
                    var entryList = new List<JObject>();
                    foreach (var gridData in sheet.data)
                    {
                        Debug.Log("Sheet: " + sheet.properties.title);
                        row = gridData.rowData[0];
                        var valueTitles = new List<string>();
                        var sum = "";
                        foreach (var cell in row.values)
                        {
                            //Debug.Log(cell.formattedValue);
                            valueTitles.Add(cell.formattedValue);
                            sum += cell.formattedValue + ",";
                        }
                        var columnCount = valueTitles.Count;
                        //                    Debug.Log("valueTitles lenght: " + valueTitles.Count + " : " + sum);

                        for (int i = 1; i < gridData.rowData.Length; i++)
                        {
                            row = gridData.rowData[i];
                            var synth = "";
                            var rowLength = row.values.Length;
                            if (rowLength > 0)
                            {
                                var jsonObject = new JObject();
                                var cellString = "";
                                for (var c = 0; c < columnCount; c++)
                                {
                                    // Debug.Log("cell : " + i + "." + c);
                                    if (c < rowLength)
                                    {
                                        cellString = row.values[c].formattedValue ?? "";
                                    }
                                    else
                                    {
                                        cellString = "";
                                    }
                                    row = gridData.rowData[i];
                                    jsonObject.Add(valueTitles[c], cellString);
                                    synth += cellString + ",";
                                }
                                entryList.Add(jsonObject);
                                // Debug.Log("gridData lenght: " + rowLength + " : " + synth);
                            }
                        }
                        var myJsonSheet = new JObject(
                            new JProperty(sheet.properties.title, entryList)
                            );

                        string jsonData = JsonConvert.SerializeObject(myJsonSheet);

                        //writeJson(jsonData, fileName + " - " + sheet.properties.title); //not necessary anymore because of the new direct import
                        ImportDataDirectly(fileName, sheet.properties.title, jsonData);
                    }
                }
            }
        }

        private void writeJson(string value, string filename)
        {
            //var varGroup = JsonUtility.FromJson<LiveDataJSONCollection>(value);
            Debug.Log("JSON write file: " + filename);

            var jsonText = value;
            var outputDirectory = Path.Combine(Application.streamingAssetsPath, PathToJson);
            //Create directory to store the json file
            if (!outputDirectory.EndsWith("/"))
            {
                outputDirectory += "/";
            }
            Directory.CreateDirectory(outputDirectory);
            var strmWriter = new StreamWriter(outputDirectory + filename + ".json", false, System.Text.Encoding.UTF8);
            strmWriter.Write(jsonText);
            strmWriter.Close();
        }

        private void ImportDataDirectly(string fileName, string dataType, string jData)
        {
            if (SceneManager.GetActiveScene().name == "manage_Database")
            {
                EditorContentHolder ech = GameObject.Find("DatabaseImporter").GetComponent<EditorContentHolder>();

                var contentType = ContentType;
                switch (contentType)
                {
                    case ContentType.Localization:
                        ech.gameObject.GetComponent<DatabaseLoader>().DirectLoadData(jData, fileName, ech.InputContent, contentType);
                        break;
                    case ContentType.Vocabulary:
                    case ContentType.Journey:
                        string langName = fileName.Replace("Learn", ""); //we get the clean selected lang name
                        langName = langName.Replace("_PlaySession", "");
                        langName = langName.Replace("_Vocabulary", "");

                        if (ech.InputContent.name.Contains(langName)) //check that the selected edition is the same that the one we're trying to import
                            ech.gameObject.GetComponent<DatabaseLoader>().DirectLoadData(jData, fileName, ech.InputContent, contentType, dataType, langName);
                        else
                            Debug.LogError("Be sure to have selected the corresponding Content edition in the 'Editor Content Holder' for the "+ fileName +" file");

                        break;
                }
            }
            else
                Debug.LogError("You must open the 'manage_Database' scene before importing new data from Google sheets");
        }
#endif
    }
}
