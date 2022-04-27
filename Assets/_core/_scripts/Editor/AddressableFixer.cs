using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using DG.DeExtensions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Antura.Tools
{
    public class AddressableFixer : MonoBehaviour
    {
        [MenuItem("Antura/Tools/Fix Language Addressables")]
        public static void FixAddressables()
        {
            // TODO: Get all assets in the lang paths for the current edition
            var lang = "persian_dari"; // @note: change this manually
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_lang_bundles/" + lang });
            Debug.Log("Fixing addressable for lang: " + lang);
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            foreach (var guid in guids)
            {
               var path = AssetDatabase.GUIDToAssetPath(guid);
               var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
               var removedPathLength = "Assets/_lang_bundles/".Length;
               var splits = path.Split('.');
               if (splits.Length > 1)
               {
                   var extension = splits[splits.Length - 1];
                   path = path.Substring(0, path.Length - (extension.Length + 1));
               }
               entry.address = path.Substring(removedPathLength, path.Length - removedPathLength);
            }
            Debug.Log("FINISHED Fixing addressable for lang: " + lang);
        }

        [MenuItem("Antura/Tools/Fix ShapeData Addressables")]
        public static void FixShapeData()
        {
            AppManager.I.AppSettingsManager.SetLearningContentID(LearningContentID.LearnToRead_Dari);
            var db = new DatabaseManager(AppManager.I.ContentEdition);

            Debug.Log("Fixing addressable for shape data.");
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains("common"));
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_core/Fonts/" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.Contains("ShapeData/shapedata")) continue;

                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var splits = path.Split('.')[0].Split('/');
                var fontName = splits[splits.Length - 3].Split(' ')[1];
                var assetName = splits[splits.Length - 1];
                var letterId = assetName.Replace("shapedata_", "");
                char[] hexChars = { 'A', 'B', 'C', 'D', 'E', 'F' };
                bool allDigits = letterId.ToCharArray().All(ch => char.IsDigit(ch) || hexChars.Contains(ch));
                if (!allDigits)
                {
                    var letterData = db.GetLetterDataById(letterId);
                    if (letterData == null)
                    {
                        Debug.LogWarning("Could not find letter data " + letterId);
                    }
                    else
                    {
                        var unicode = letterData.GetCompleteUnicodes();
                        assetName = $"shapedata_{unicode}";
                        Debug.Log($"Renamed {letterId} to: {assetName}");
                        AssetDatabase.RenameAsset(path, assetName);
                    }
                }
                entry.address = fontName + "/" + assetName;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("FINISHED Fixing addressable for shape data");
        }
    }
}
