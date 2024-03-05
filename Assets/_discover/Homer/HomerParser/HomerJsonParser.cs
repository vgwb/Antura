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
        public static string ApiVersion = "1.3";

        public static HomerProject LoadHomerProject()
        {
            var pathAndFile = Application.streamingAssetsPath + "/Homer/ProjectData/homer.json";

            string jsonContent = "";

#if UNITY_ANDROID
            // Android only use WWW to read file
            WWW reader = new WWW(pathAndFile);
            while (!reader.isDone){}

            jsonContent = reader.text;
#else
            jsonContent = System.IO.File.ReadAllText(pathAndFile).Trim();
#endif

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