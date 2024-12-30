using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;

namespace Homer
{
    public static class HomerJsonParser
    {
        public static string ApiVersion = "1.4";

        public static HomerProject LoadHomerProject()
        {
            //file to be readable needs to be in streamingAssets
            //var pathAndFile = "Assets/Plugins/Homer/Resources/homer.json";

            //string jsonContent = "";
/*
#if UNITY_ANDROID
            // Android only use WWW to read file
            WWW reader = new WWW(pathAndFile);
            while (!reader.isDone){}

            jsonContent = reader.text;
#else
            jsonContent = System.IO.File.ReadAllText(pathAndFile).Trim();
#endif
*/

            string jsonContent = HomerConfig.I.Homer.text;

            HomerProject project = JsonConvert.DeserializeObject<HomerProject>(jsonContent);

            if (project._apiVersion != ApiVersion)
            {
                Debug.Log($"WARNING: API VERSION ON UNITY PROJECT IS {ApiVersion} AND DOES NOT MATCH " +
                          $"JSON SERVICE VERSION {project._apiVersion}");
                Debug.Log($"YOU SHOULD UPDATE THE UNITY CLASSES FROM HOMER SUPPORT SITE homer.open-lab.com");
            }

            return project;
        }
    }
}