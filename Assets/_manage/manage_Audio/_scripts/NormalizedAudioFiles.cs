#if UNITY_EDITOR
using Antura.Core;
using Antura.Database;
using Antura.Language;

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Antura.Database.Management;
using UnityEngine;
using UnityEditor;

namespace Antura.Test
{
    /// <summary>
    /// Automatic check for all audio files to be present for each language in both ways: from the localization table to the project folder and vice versa
    /// </summary>
    public class NormalizedAudioFiles : MonoBehaviour
    {
        public LanguageCode LanguageToCheck;
        List<string> folders;

        private ContentEditionConfig ContentTarget;
        public DatabaseManager dbManager;
        public bool CheckDialogs;
        public bool CheckLetters;
        public bool CheckPhrases;
        public bool CheckWords;

        List<LocalizationData> localization;
        List<WordData> words;
        List<PhraseData> phrases;
        List<LetterData> letters;

        void Awake()
        {
            if (PreValidateAssets()) //if the ContentTarget and LanguageToCheck are filled, we initialize all the lists we need from the db
            {
                dbManager = new DatabaseManager(ContentTarget, LanguageToCheck);

                localization = dbManager.GetAllLocalizationData();
                letters = dbManager.GetAllLetterData();
                phrases = dbManager.GetAllPhraseData();
                words = dbManager.GetAllWordData();
            }

            folders = new List<string>();
            var info = new DirectoryInfo(Application.dataPath + "/_lang_bundles/");
            var dirInfo = info.GetDirectories();
            foreach (DirectoryInfo direct in dirInfo)
                folders.Add(direct.Name);
        }

        public void CheckProjectAgainstDb()
        {
            if (!PreValidateAssets())
                return;

            foreach (string lang in folders)
            {
                if (!lang.ToLower().Equals(LanguageToCheck.ToString().ToLower()))
                    continue;

                int missing_count = 0;
                string missing_keys = "";
                string langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Dialogs/";
                if(CheckDialogs)
                {
                    if (Directory.Exists(langPath))
                    {
                        if (localization != null)
                        {
                            Debug.Log("Starting to check audio files from 'Dialogs' folder of " + lang + " version to localization table...");

                            List<string> audioFiles = new List<string>();
                            var info = new DirectoryInfo(langPath);
                            var filesInfo = info.GetFiles();
                            foreach (FileInfo file in filesInfo)
                            {
                                var fileAux = localization.FirstOrDefault(af => file.Name.Contains(af.AudioKey));
                                if (fileAux == null)
                                {
                                    Debug.LogError("The audio file \"" + file.Name + "\" doesn't exist in localization table of " + lang + " version");
                                    missing_keys += file.Name + "\n";
                                    missing_count++;
                                }
                            }
                            if (missing_count > 0)
                                Debug.LogWarning("WARNING: total missing audio files in the localization table of " + lang + " version: " + missing_count.ToString() + "\nList of missing AudioKeys in the Localization Table:\n" + missing_keys);
                            else
                                Debug.Log("SUCCESS: all audio files of " + lang + " version folder are present in the localization table");

                            missing_count = 0;
                        }
                        else
                            Debug.LogWarning("WARNING: The localization table doesn't exist for the " + lang + " version");
                    }
                    else
                        Debug.LogError("ATTENTION: 'Dialogs' folder not found for " + lang + " version");
                }

                if (CheckLetters)
                {
                    //re utilize the count vars for letters checking
                    missing_count = 0;
                    missing_keys = "";
                    langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Letters/";
                    if (Directory.Exists(langPath))
                    {
                        if (letters != null)
                        {
                            Debug.Log("Starting to check audio files from 'Letters' folder of " + lang + " version to letters table...");

                            List<string> audioFiles = new List<string>();
                            var info = new DirectoryInfo(langPath);
                            var filesInfo = info.GetFiles();
                            foreach (FileInfo file in filesInfo)
                            {
                                var fileAux = letters.FirstOrDefault(lt => file.Name.Contains(lt.GetAudioFilename()));
                                if (fileAux == null)
                                {
                                    Debug.LogError("The audio file \"" + file.Name + "\" doesn't exist in letters table of " + lang + " version");
                                    missing_keys += file.Name + "\n";
                                    missing_count++;
                                }
                            }
                            if (missing_count > 0)
                                Debug.LogWarning("WARNING: total missing audio files in the letters table of " + lang + " version: " + missing_count.ToString() + "\nList of missing AudioKeys in the Letters Table:\n" + missing_keys);
                            else
                                Debug.Log("SUCCESS: all audio files of " + lang + " version folder are present in the letters table");

                            missing_count = 0;
                        }
                        else
                            Debug.LogWarning("WARNING: The letters table doesn't exist for the " + lang + " version");
                    }
                    else
                        Debug.LogError("ATTENTION: 'Letters' folder not found for " + lang + " version");
                }

                if (CheckPhrases)
                {
                    //re utilize the count vars for phrases checking
                    missing_count = 0;
                    missing_keys = "";
                    langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Phrases/";
                    if (Directory.Exists(langPath))
                    {
                        if (phrases != null)
                        {
                            Debug.Log("Starting to check audio files from 'Phrases' folder of " + lang + " version to phrases table...");

                            List<string> audioFiles = new List<string>();
                            var info = new DirectoryInfo(langPath);
                            var filesInfo = info.GetFiles();
                            foreach (FileInfo file in filesInfo)
                            {
                                var fileAux = phrases.FirstOrDefault(lt => file.Name.Contains(lt.Id));
                                if (fileAux == null)
                                {
                                    Debug.LogError("The audio file \"" + file.Name + "\" doesn't exist in phrases table of " + lang + " version");
                                    missing_keys += file.Name + "\n";
                                    missing_count++;
                                }
                            }
                            if (missing_count > 0)
                                Debug.LogWarning("WARNING: total missing audio files in the phrases table of " + lang + " version: " + missing_count.ToString() + "\nList of missing AudioKeys in the Phrases Table:\n" + missing_keys);
                            else
                                Debug.Log("SUCCESS: all audio files of " + lang + " version folder are present in the phrases table");

                            missing_count = 0;
                        }
                        else
                            Debug.LogWarning("WARNING: The phrases table doesn't exist for the " + lang + " version");
                    }
                    else
                        Debug.LogError("ATTENTION: 'Phrases' folder not found for " + lang + " version");
                }

                if (CheckWords)
                {
                    //re utilize the count vars for words checking
                    missing_count = 0;
                    missing_keys = "";
                    langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Words/";
                    if (Directory.Exists(langPath))
                    {
                        if (words != null)
                        {
                            Debug.Log("Starting to check audio files from 'Words' folder of " + lang + " version to words table...");

                            List<string> audioFiles = new List<string>();
                            var info = new DirectoryInfo(langPath);
                            var filesInfo = info.GetFiles();
                            foreach (FileInfo file in filesInfo)
                            {
                                var fileAux = words.FirstOrDefault(lt => file.Name.Contains(lt.Id));
                                if (fileAux == null)
                                {
                                    Debug.LogError("The audio file \"" + file.Name + "\" doesn't exist in words table of " + lang + " version");
                                    missing_keys += file.Name + "\n";
                                    missing_count++;
                                }
                            }
                            if (missing_count > 0)
                                Debug.LogWarning("WARNING: total missing audio files in the words table of " + lang + " version: " + missing_count.ToString() + "\nList of missing AudioKeys in the Words Table:\n" + missing_keys);
                            else
                                Debug.Log("SUCCESS: all audio files of " + lang + " version folder are present in the words table");

                            missing_count = 0;
                        }
                        else
                            Debug.LogWarning("WARNING: The words table doesn't exist for the " + lang + " version");
                    }
                    else
                        Debug.LogError("ATTENTION: 'Words' folder not found for " + lang + " version");
                }
            }
        }

        public void CheckDbAgaistProject()
        {
            if (!PreValidateAssets())
                return;

            foreach (string lang in folders)
            {
                if (!lang.ToLower().Equals(LanguageToCheck.ToString().ToLower()))
                    continue;

                int missing_count = 0;
                string missing_keys = "";
                string langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Dialogs/";
                if (CheckDialogs)
                {
                    if (Directory.Exists(langPath))
                    {
                        DirectoryInfo root = new DirectoryInfo(langPath);
                        Debug.Log("Starting to check audio files from localization table to the " + lang + " version folder...");

                        foreach (LocalizationData data in localization)
                        {
                            FileInfo[] targetFile = root.GetFiles(data.AudioKey + ".*");
                            if (targetFile.Length <= 0 && data.AudioKey != "") //checking that the audio file of the localization table exist in the version audio folder
                            {
                                Debug.LogError("The audio file \"" + data.AudioKey + "\" doesn't exist for " + lang + " version");
                                missing_keys += data.AudioKey + "\n";
                                missing_count++;
                            }
                        }
                        if (missing_count > 0)
                            Debug.LogWarning("WARNING: total missing dialog audio files in the folder of " + lang + " version: " + missing_count.ToString() + "\nList of missing Audio files in the project:\n" + missing_keys);
                        else
                            Debug.Log("SUCCESS: all audio files of the localization table exist in the folder of " + lang + " version");

                        missing_count = 0;
                    }
                    else
                        Debug.LogError("ATTENTION: The 'Dialogs' folder doesn't exist for the " + lang + " version");
                }

                if (CheckLetters)
                {
                    //re utilize the count vars for letters checking
                    missing_count = 0;
                    missing_keys = "";
                    langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Letters/";
                    if (Directory.Exists(langPath))
                    {
                        DirectoryInfo root = new DirectoryInfo(langPath);
                        Debug.Log("Starting to check audio files from letters table to the " + lang + " version folder...");

                        foreach (LetterData data in letters)
                        {
                            FileInfo[] targetFile = root.GetFiles(data.GetAudioFilename() + ".*");
                            if (targetFile.Length <= 0 && data.GetAudioFilename() != "") //checking that the audio file of the letters table exist in the version audio folder
                            {
                                Debug.LogError("The audio file \"" + data.GetAudioFilename() + "\" doesn't exist for " + lang + " version");
                                missing_keys += data.GetAudioFilename() + "\n";
                                missing_count++;
                            }
                        }
                        if (missing_count > 0)
                            Debug.LogWarning("WARNING: total missing letter audio files in the folder of " + lang + " version: " + missing_count.ToString() + "\nList of missing Audio files in the project:\n" + missing_keys);
                        else
                            Debug.Log("SUCCESS: all audio files of the letters table exist in the folder of " + lang + " version");

                        missing_count = 0;
                    }
                    else
                        Debug.LogError("ATTENTION: The 'Letters' folder doesn't exist for the " + lang + " version");
                }

                if (CheckPhrases)
                {
                    //re utilize the count vars for phrases checking
                    missing_count = 0;
                    missing_keys = "";
                    langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Phrases/";
                    if (Directory.Exists(langPath))
                    {
                        DirectoryInfo root = new DirectoryInfo(langPath);
                        Debug.Log("Starting to check audio files from phrases table to the " + lang + " version folder...");

                        foreach (PhraseData data in phrases)
                        {
                            FileInfo[] targetFile = root.GetFiles(data.Id + ".*");
                            if (targetFile.Length <= 0 && data.Id != "") //checking that the audio file of the phrases table exist in the version audio folder
                            {
                                Debug.LogError("The audio file \"" + data.Id + "\" doesn't exist for " + lang + " version");
                                missing_keys += data.Id + "\n";
                                missing_count++;
                            }
                        }
                        if (missing_count > 0)
                            Debug.LogWarning("WARNING: total missing phrase audio files in the folder of " + lang + " version: " + missing_count.ToString() + "\nList of missing Audio files in the project:\n" + missing_keys);
                        else
                            Debug.Log("SUCCESS: all audio files of the phrases table exist in the folder of " + lang + " version");

                        missing_count = 0;
                    }
                    else
                        Debug.LogError("ATTENTION: The 'Phrases' folder doesn't exist for the " + lang + " version");
                }

                if (CheckWords)
                {
                    //re utilize the count vars for words checking
                    missing_count = 0;
                    missing_keys = "";
                    langPath = Application.dataPath + "/_lang_bundles/" + lang + "/Audio/Words/";
                    if (Directory.Exists(langPath))
                    {
                        DirectoryInfo root = new DirectoryInfo(langPath);
                        Debug.Log("Starting to check audio files from words table to the " + lang + " version folder...");

                        foreach (WordData data in words)
                        {
                            FileInfo[] targetFile = root.GetFiles(data.Id + ".*");
                            if (targetFile.Length <= 0 && data.Id != "") //checking that the audio file of the words table exist in the version audio folder
                            {
                                Debug.LogError("The audio file \"" + data.Id + "\" doesn't exist for " + lang + " version");
                                missing_keys += data.Id + "\n";
                                missing_count++;
                            }
                        }
                        if (missing_count > 0)
                            Debug.LogWarning("WARNING: total missing word audio files in the folder of " + lang + " version: " + missing_count.ToString() + "\nList of missing Audio files in the project:\n" + missing_keys);
                        else
                            Debug.Log("SUCCESS: all audio files of the words table exist in the folder of " + lang + " version");

                        missing_count = 0;
                    }
                    else
                        Debug.LogError("ATTENTION: The 'Words' folder doesn't exist for the " + lang + " version");
                }
            }
        }

        public bool PreValidateAssets()
        {
            ContentTarget = FindObjectOfType<EditorContentHolder>()?.InputContent;
            if (ContentTarget == null) //if its not present in the script (attachable in the inspector), we shut down the call
            {
                Debug.LogWarning("WARNING: There's no Content - Learn... asset attached to run the task");
                return false;
            }
            if (LanguageToCheck == LanguageCode.NONE || LanguageToCheck == LanguageCode.COUNT)
            {
                Debug.LogWarning("WARNING: There's none language selected to run the task");
                return false;
            }
            return true;
        }
    }
}
#endif
