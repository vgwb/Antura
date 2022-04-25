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

        public void CheckProjectAgainstDb()
        {
            var learnEng = GetDefaultLearn(); //we'll use the 'Content - Learn_English' as a default ContentEditionConfig
            if (learnEng == null) //if its not present in the script (attachable in the inspector), we shut down the call
                return;

            foreach (string lang in folders)
            {
                //uncomment for quick debug: only test english
                //if (lang != "english")
                //    continue;

                //ASK: really necessary this? or we can use always the english version?
                dbManager = new DatabaseManager(learnEng, (Language.LanguageCode)Enum.Parse(typeof(Language.LanguageCode), lang));
                foreach (ContentEditionConfig ceg in langFolders) //we'll try to find the 'Content - Learn_...' of any specific language
                    if (ceg.name.ToLower().Contains(lang.ToLower()))
                        dbManager = new DatabaseManager(ceg, (Language.LanguageCode)Enum.Parse(typeof(Language.LanguageCode), lang));
                //END-ASK

                List<LocalizationData> localization = dbManager.GetAllLocalizationData();

                int missing_count = 0;
                string langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Dialogs/";
                if (Directory.Exists(langPath))
                {
                    if (localization != null)
                    {
                        Debug.Log("Starting to check audio files from 'Dialogs' folder of " + lang + " version to localization table...");

                        List<string> audioFiles = new List<string>();
                        var info = new DirectoryInfo(Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Dialogs/");
                        var filesInfo = info.GetFiles();
                        foreach (FileInfo file in filesInfo)
                        {
                            var fileAux = localization.FirstOrDefault(af => file.Name.Contains(af.AudioKey));
                            if (fileAux == null)
                            {
                                Debug.Log("The audio file \"" + file.Name + "\" doesn't exist in localization table of " + lang + " version");
                                missing_count++;
                            }
                        }
                        if (missing_count > 0)
                            Debug.Log("WARNING: total missing audio files in the localization table of " + lang + " version: " + missing_count.ToString());
                        else
                            Debug.Log("SUCCESS: all audio files of " + lang + " version folder are present in the localization table");

                        missing_count = 0;
                    }
                    else
                        Debug.Log("WARNING: The localization table doesn't exist for the " + lang + " version");
                }
                else
                    Debug.Log("ATTENTION: 'Dialogs' folder not found for " + lang + " version");

            }
        }

        public void CheckDbAgaistProject()
        {
            var learnEng = GetDefaultLearn();
            if (learnEng == null)
                return;

            foreach (string lang in folders)
            {
                //uncomment for quick debug: only test english
                //if (lang != "english")
                //    continue;

                dbManager = new DatabaseManager(learnEng, (Language.LanguageCode)Enum.Parse(typeof(Language.LanguageCode), lang));
                foreach (ContentEditionConfig ceg in langFolders)
                    if (ceg.name.ToLower().Contains(lang.ToLower())) //check if we have the ContentEditionConfig asset of the current lang folder
                        dbManager = new DatabaseManager(ceg, (Language.LanguageCode)Enum.Parse(typeof(Language.LanguageCode), lang));

                int missing_count = 0;
                string langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Dialogs/";
                if (Directory.Exists(langPath))
                {
                    Debug.Log("Starting to check audio files from localization table to the " + lang + " version folder...");

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
                        Debug.Log("WARNING: total missing audio files in the folder of " + lang + " version: " + missing_count.ToString());
                    else
                        Debug.Log("SUCCESS: all audio files of the localization table exist in the folder of " + lang + " version");

                    missing_count = 0;
                }
                else
                    Debug.Log("ATTENTION: The 'Dialogs' folder doesn't exist for the " + lang + " version");
            }
        }

        ContentEditionConfig GetDefaultLearn()
        {
            var learnEng = langFolders.FirstOrDefault(lf => lf.name == "Content - Learn_English");
            if (learnEng == null)
            {
                Debug.Log("ERROR: \"Content - Learn_English\" missing in NormalizedAudioFiles script");
                return null;
            }
            return learnEng;
        }
    }
}
#endif
