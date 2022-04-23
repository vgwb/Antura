#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Database;
using System.Linq;
using System.IO;
using UnityEditor;
using Antura.Core;
using System;

namespace Antura.Test
{
    /// <summary>
    /// Automatic check for all audio files to be present for each language in both ways: from the localization table to the project folder and vice versa
    /// </summary>
    public class NormalizedAudioFiles : MonoBehaviour
    {
        List<string> folders;
        public List<ContentEditionConfig> langFolders;
        public DatabaseManager dbManager;

        void Awake()
        {
            folders = new List<string>();
            var info = new DirectoryInfo(Application.dataPath + "/_lang_bundles/");
            var dirInfo = info.GetDirectories();
            foreach (DirectoryInfo direct in dirInfo)
                folders.Add(direct.Name);
        }

        /// TODO: finish the 2nd way (CheckProjectAgainstDb() method) that check for audio files that are present in project folder, but not in the localization table for each language
        public void CheckProjectAgainstDb()
        {

        }

        public void CheckDbAgaistProject()
        {
            foreach (string lang in folders)
            {
                //uncomment for quick debug: only test english
                //if (lang != "english")
                //    continue;

                dbManager = new DatabaseManager(langFolders.First(lf => lf.name == "Content - Learn_English"), (Language.LanguageCode)Enum.Parse(typeof(Language.LanguageCode), lang));
                foreach (ContentEditionConfig ceg in langFolders)
                    if (ceg.name.ToLower().Contains(lang.ToLower())) //check if we have the ContentEditionConfig asset of the current lang folder
                        dbManager = new DatabaseManager(ceg, (Language.LanguageCode)Enum.Parse(typeof(Language.LanguageCode), lang));

                int missing_count = 0;
                string langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Dialogs/";
                if (Directory.Exists(langPath))
                {
                    Debug.Log("Starting check of audio files from localization table in the " + lang + " version folder...");

                    List<LocalizationData> localization = dbManager.GetAllLocalizationData();

                    foreach (LocalizationData data in localization)
                    {
                        if (!File.Exists(langPath + "/" + data.AudioKey + ".mp3") && data.AudioKey != "") //checking that the audio file of the localization table exist in the version audio folder
                        {
                            Debug.Log("The audio file \"" + data.AudioKey + "\" doesn't exist for " + lang + " version");
                            missing_count++;
                        }
                    }
                    if (missing_count > 0)
                        Debug.Log("Total missing audio files in the folder of " + lang + " version: " + missing_count.ToString());
                    else
                        Debug.Log("All audio files of the localization table exist in the folder of " + lang + " version");

                    missing_count = 0;
                }
                else
                    Debug.Log("The 'Dialogs' folder not even exist for the " + lang + " version");
            }
            
        }
    }
}
#endif
