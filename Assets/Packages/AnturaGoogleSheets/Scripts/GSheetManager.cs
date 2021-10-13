#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Antura.GoogleSheets
{
    public class GSheetManager : MonoBehaviour
    {
        [Tooltip("Import data from Google Sheet as configured")]
        public bool ImportFromGoogleSheet;
        public GoogleSheetGroup SheetsGroup;

        [Header("Debug")]
        public bool DebugOutput;
        public bool DebugWriteTmpFile;
        public bool DebugTestJsonDecode;
        public TextAsset TestJsonData;

        private string GoogleApiUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
        private string GoogleApiUrlPostfix = "?includeGridData=true";
        private string GoogleSecretApi = "&key=AIzaSyDZhLs3ds7swYw9HizLPz5M-0F584QMRcs";

        private string PathToTmp = "../_config/json_data/tmp/";
        private string PathToJson = "../_config/json_data/";
        private IEnumerator coroutine;

        void Start()
        {
            if (ImportFromGoogleSheet) {
                UpdateFromServer();
            }
            if (DebugTestJsonDecode) {
                decodeJSON("TestJsonData", TestJsonData.text);
            }
        }

        private void UpdateFromServer()
        {
            StartCoroutine(fetchData(SheetsGroup.Sheets));
        }

        private IEnumerator fetchData(GoogleSheetRef[] sheets)
        {
            foreach (var sheet in sheets) {
                var uri = GoogleApiUrl + sheet.DocID + GoogleApiUrlPostfix + GoogleSecretApi;
                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
                    webRequest.SetRequestHeader("accept", "application/json");
                    Debug.Log("get " + uri);
                    yield return webRequest.SendWebRequest();

                    if ((webRequest.result == UnityWebRequest.Result.ConnectionError) || (webRequest.result == UnityWebRequest.Result.ProtocolError)) {
                        if (DebugOutput) Debug.Log("Error: " + webRequest.error);
                        break;
                    } else {
                        if (DebugOutput) Debug.Log("Received: " + webRequest.downloadHandler.text);
                        manageReceivedData(sheet, webRequest.downloadHandler.text);
                    }
                }
            }
        }

        private void manageReceivedData(GoogleSheetRef sheet, string value)
        {
            //var varGroup = JsonUtility.FromJson<LiveDataJSONCollection>(value);
            Debug.Log("manageReceivedData per " + sheet.FileName);

            var jsonText = value;
            if (DebugWriteTmpFile) {
                var outputDirectory = Path.Combine(Application.streamingAssetsPath, PathToTmp);

                //Create directory to store the json file
                if (!outputDirectory.EndsWith("/")) {
                    outputDirectory += "/";
                }
                Directory.CreateDirectory(outputDirectory);
                StreamWriter strmWriter = new StreamWriter(outputDirectory + sheet.FileName + ".json", false, System.Text.Encoding.UTF8);
                strmWriter.Write(jsonText);
                strmWriter.Close();
            }
            decodeJSON(sheet.FileName, jsonText);
        }

        private void decodeJSON(string fileName, string jsonText)
        {
            //Debug.Log(jsonDemo.text);
            var definition = new GSheet_Spreadsheet();
            var obj = JsonConvert.DeserializeAnonymousType(jsonText, definition);

            Debug.Log("Spreadsheet: " + obj.properties.title);
            GSheet_RowData row;

            foreach (var sheet in obj.sheets) {
                var entryList = new List<JObject>();

                foreach (var gridData in sheet.data) {
                    Debug.Log("sheet: " + sheet.properties.title);
                    row = gridData.rowData[0];
                    var valueTitles = new List<string>();
                    var sum = "";
                    foreach (var cell in row.values) {
                        //Debug.Log(cell.formattedValue);
                        valueTitles.Add(cell.formattedValue);
                        sum += cell.formattedValue + ",";
                    }
                    var columnCount = valueTitles.Count;
                    Debug.Log("valueTitles lenght: " + valueTitles.Count + " : " + sum);

                    for (int i = 1; i < gridData.rowData.Length; i++) {

                        row = gridData.rowData[i];
                        var synth = "";
                        var rowLength = row.values.Length;
                        if (rowLength > 0) {

                            var jsonObject = new JObject();
                            var cellString = "";
                            for (var c = 0; c < columnCount; c++) {
                                // Debug.Log("cell : " + i + "." + c);

                                if (c < rowLength) {
                                    cellString = row.values[c].formattedValue ?? "";
                                } else {
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
                    JObject myJsonSheet = new JObject(
                        new JProperty(sheet.properties.title, entryList)
                        );

                    string jsonData = JsonConvert.SerializeObject(myJsonSheet);
                    //Debug.Log(jj);
                    writeJson(jsonData, fileName + " - " + sheet.properties.title);
                }
            }

        }

        private void writeJson(string value, string filename)
        {
            //var varGroup = JsonUtility.FromJson<LiveDataJSONCollection>(value);
            Debug.Log("writeJson " + filename);

            var jsonText = value;
            var outputDirectory = Path.Combine(Application.streamingAssetsPath, PathToJson);
            //Create directory to store the json file
            if (!outputDirectory.EndsWith("/")) {
                outputDirectory += "/";
            }
            Directory.CreateDirectory(outputDirectory);
            StreamWriter strmWriter = new StreamWriter(outputDirectory + filename + ".json", false, System.Text.Encoding.UTF8);
            strmWriter.Write(jsonText);
            strmWriter.Close();
        }
    }
}

#endif